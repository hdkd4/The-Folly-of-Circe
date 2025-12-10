using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
public enum BattleState { PlayerTurn, EnemyTurn }

public class TurnManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip missSound;
    public MakeDamageNumbers mdn;
    public CameraFollowTarget cameraFollowTarget;
    [Header("Targeting")]
    public BattleFieldManaDisplay bFMDisplay;
    private bool selectingTarget = false;
    private bool selectingTargetRanged = false;
    private StatsManager pendingAttackTarget;
    //private bool enemyPendingAttack = false;
    [Header("References")]
    public TextMeshProUGUI combatLog;
    public StatsManager playerStats;
    public List<StatsManager> enemyStats;
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float moveDistanceMax = 9.0f;
    public float moveDistanceTracker;
    public Transform playerTransform;
    public Transform moveToPreview;
    public GameObject pauseMenu;
    public float enemyDistanceTracker = 9.0f;
    [Header("State")]
    public BattleState state = BattleState.PlayerTurn;
    private StatsManager currentEnemy;
    [Header("Pathfinding")]
    public Pathfinding pathfinding;
    private CreateMoveDots cmd;
    public bool doNotMove;
    public LayerMask obstacleLayer;
    private Coroutine moveRoutine;
    private int deadEnemies;
    private int enemyCount;
    public bool inCombat = false;
    public GameObject winScreen;
    public GameObject lossScreen;

    void Start()
    {
        cmd = GetComponent<CreateMoveDots>();
        combatLog.text = "Your Turn";
        state = BattleState.PlayerTurn;
        moveSpeed = 4f;
        ResetMovement();
    }

    void Update()
    {
        foreach (StatsManager enemy in enemyStats)
        {
            EnemyActions EA = enemy.GetComponent<EnemyActions>();
            if (enemy.health <= 0) EA.Die();
            if (enemy == null || enemy.health <= 0 || EA.inCombat || EA.seeAttempts <1) continue;
            EA.DetectEnemy(playerStats.transform);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
        }

        foreach (var enemy in enemyStats)
        {
            if (enemy != null && enemy.health <= 0)
            {
                bFMDisplay.IncreaseMana(enemy.storedMana);
                enemy.storedMana = 0;
            }
        }
        
        if(playerStats.health <=0)
        {
            combatLog.text = "Game Over";
            lossScreen.SetActive(true);
            return;
        }
        
        // Preview move position with mouse
        if (state == BattleState.PlayerTurn && Input.GetMouseButtonDown(0) && pendingAttackTarget == null && !doNotMove)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            
            Vector2 mouseScreen = Input.mousePosition;
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            Vector2 playerPos = playerTransform.position;


            float distance = Vector2.Distance(playerPos, mouseWorld);

            moveToPreview.position = mouseWorld;
            cmd.MakeDots(playerPos, mouseWorld, moveDistanceTracker);

            combatLog.text = "Move preview set. Click 'Move' to confirm.";

            // If we’re moving towards an attack target
        }
        if (pendingAttackTarget != null)
        {
            float distance = Vector3.Distance(playerTransform.position, pendingAttackTarget.transform.position);
            if (distance <= 1.5f)
            {
                DoAttack(pendingAttackTarget);
                playerStats.actionPoints -= 1;
                pendingAttackTarget = null;
                doNotMove = false;
                cameraFollowTarget.target = null;
                cameraFollowTarget.follow = false;
            }
        }

        //if (state == BattleState.EnemyTurn && enemyPendingAttack && Vector2.Distance(currentEnemy.transform.position, playerTransform.position) < 1.5f)
        //{
        //    EnemyMeleeAttack(currentEnemy);
        //}

    }

    public void ResetMovement()
    {
        moveDistanceTracker = moveDistanceMax;
    }

    // Player Actions
    public void OnPlayerAttackButton()
    {
        if (state != BattleState.PlayerTurn) return;
        if (playerStats.actionPoints >= 1)
        {
            selectingTarget = true;
            doNotMove = true;
            combatLog.text = "Select an enemy to attack.";
        }
        else
        {
            combatLog.text = "Not enough resources (actions)";
        }
    }

    public void OnPlayerRangedAttackButton()
    {
        if (state != BattleState.PlayerTurn) return;
        if (playerStats.actionPoints >= 1)
        {
            selectingTargetRanged = true;
            doNotMove = true;
            combatLog.text = "Select an enemy to attack.";
        }
        else
        {
            combatLog.text = "Not enough resources (actions)";
        }
    }

    public void SelectEnemyTarget(StatsManager targetEnemy)
    {
        if (!selectingTarget) return;
        if (targetEnemy == null) return;

        selectingTarget = false;
        StartCoroutine(PlayerMeleeAttack(targetEnemy));
    }

    public void SelectEnemyTargetRanged(StatsManager targetEnemy)
    {
        if (!selectingTargetRanged) return;
        if (targetEnemy == null) return;

        selectingTargetRanged = false;
        PlayerRangedAttack(targetEnemy);
    }

    public IEnumerator PlayerMeleeAttack(StatsManager targetEnemy)
    {
        cameraFollowTarget.target = playerTransform;
        cameraFollowTarget.follow = true;
        float distance = Vector3.Distance(playerTransform.position, targetEnemy.transform.position);

        if (playerStats.actionPoints >= 1 && distance <= 1.5f)
        {
            DoAttack(targetEnemy);
            playerStats.actionPoints -= 1;
            cameraFollowTarget.target = null;
            cameraFollowTarget.follow = false;
        }
        else if (distance > 1.5f && moveDistanceTracker >= distance - 1.45f)
        {
            // Try to move to that spot
            Vector2 playerPos = playerTransform.position;
            Vector2 targetPos = targetEnemy.transform.position;

            cmd.MakeDots(playerPos, targetPos, moveDistanceTracker);

            Vector2 stopPos = cmd.finalReachablePosition;

            pathfinding.grid.UpdateDynamicObstacles(playerTransform, enemyStats.ToArray());
            var path = pathfinding.FindPath(playerPos, stopPos);
            combatLog.text = "Moving!";
            yield return StartCoroutine(FollowPath(path));
            if(Vector2.Distance(playerTransform.position, targetEnemy.transform.position) <= 1.5f)
                DoAttack(targetEnemy);
        }
        else
        {
            combatLog.text = "Not enough movement!";
        }
    }

    private void PlayerRangedAttack(StatsManager targetEnemy)
    {
        cameraFollowTarget.target = playerTransform;
        cameraFollowTarget.follow = true;

        if (playerStats.actionPoints >= 1 && CanSeeTarget(targetEnemy.transform.position))
        {
            DoAttack(targetEnemy);
            playerStats.actionPoints -= 1;
        }
        else
        {
            combatLog.text = "Can't See Target!";
        }
        cameraFollowTarget.target = null;
        cameraFollowTarget.follow = false;
    }

    private void DoAttack(StatsManager targetEnemy)
    {
        int diceRoll = DiceManager.DiceManager.D20(1);
        int attackRoll = diceRoll + GetModifier(playerStats.strength);

        if(diceRoll == 20)
        {
            int damage = DiceManager.DiceManager.D8(1) + 8 + (2*GetModifier(playerStats.strength));
            targetEnemy.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString() + "!", targetEnemy.transform.position, true));
            combatLog.text = "Critical Hit! You deal " + damage + " to " + targetEnemy.name;
            audioSource.clip = hitSound;
            audioSource.Play();
        }

        else if (attackRoll >= targetEnemy.armorClass)
        {
            int damage = DiceManager.DiceManager.D8(1) + GetModifier(playerStats.strength);
            targetEnemy.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString(), targetEnemy.transform.position, true));
            combatLog.text = "Hit! You deal " + damage + " to " + targetEnemy.name;
            audioSource.clip = hitSound;
            audioSource.Play();
        }
        else
        {
            StartCoroutine(mdn.MakeDamageNumber("Miss!", targetEnemy.transform.position, true));
            combatLog.text = "Miss! " + targetEnemy.name + " takes no damage.";
            audioSource.clip = missSound;
            audioSource.Play();
        }
        cameraFollowTarget.target = null;
        cameraFollowTarget.follow = false;
    }

    private void DoRangedAttack(StatsManager targetEnemy)
    {
        int diceRoll = DiceManager.DiceManager.D20(1);
        int attackRoll = diceRoll + GetModifier(playerStats.strength);

        if(diceRoll == 20)
        {
            int damage = DiceManager.DiceManager.D6(1) + 6 + (2*GetModifier(playerStats.dexterity));
            targetEnemy.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString() + "!", targetEnemy.transform.position, true));
            combatLog.text = "Critical Hit! You deal " + damage + " to " + targetEnemy.name;
        }

        else if (attackRoll >= targetEnemy.armorClass)
        {
            int damage = DiceManager.DiceManager.D6(1) + GetModifier(playerStats.strength);
            targetEnemy.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString(), targetEnemy.transform.position, true));
            combatLog.text = "Hit! You deal " + damage + " to " + targetEnemy.name;
        }
        else
        {
            StartCoroutine(mdn.MakeDamageNumber("Miss!", targetEnemy.transform.position, true));
            combatLog.text = "Miss! " + targetEnemy.name + " takes no damage.";
        }
        cameraFollowTarget.target = null;
        cameraFollowTarget.follow = false;
    }

    // Enemy AI
    public void EndPlayerTurn()
    {
        if (state != BattleState.PlayerTurn) return;

        cmd.DestroyDots();
        moveToPreview.position = playerTransform.position;
        StopAllCoroutines();
        state = BattleState.EnemyTurn;
        combatLog.text = "Enemy's turn...";
        cameraFollowTarget.target = null;
        cameraFollowTarget.follow = false;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        deadEnemies = 0;
        enemyCount = 0;
        foreach (StatsManager enemy in enemyStats)
        {
            enemyCount++;
            EnemyActions EA = enemy.GetComponent<EnemyActions>();
            if (enemy.health < 1) deadEnemies++;
            EA.seeAttempts=1;
            if (enemy == null || enemy.health <= 1 || !EA.inCombat) continue;
            pathfinding.grid.UpdateDynamicObstacles(playerTransform, enemyStats.ToArray());
            cameraFollowTarget.target = enemy.transform;
            cameraFollowTarget.follow = true;
            yield return new WaitForSeconds(1f);
            //currentEnemy = enemy;
            yield return StartCoroutine(EA.TakeAction(playerStats));
            yield return new WaitForSeconds(1f);
        }
        if(deadEnemies == enemyCount && inCombat)
        {
            //Victory State
            winScreen.SetActive(true);
        }

        state = BattleState.PlayerTurn;
        combatLog.text = "Your turn!";
        ResetMovement();
        playerStats.actionPoints = playerStats.actionPointsMax;
        cameraFollowTarget.target = playerTransform;
        cameraFollowTarget.follow = true;
        yield return new WaitForSeconds(0.5f);
        cameraFollowTarget.follow = false;
    }

    public void OnMoveButton()
    {
        doNotMove = false;
        if (state != BattleState.PlayerTurn) return;
        if (moveToPreview == null) { Debug.LogError("No preview marker set!"); return; }
        

        Vector2 playerPos = playerTransform.position;
        Vector2 targetPos = moveToPreview.position;

        cmd.MakeDots(playerPos, targetPos, moveDistanceTracker);

        Vector2 stopPos = cmd.finalReachablePosition;

        pathfinding.grid.UpdateDynamicObstacles(playerTransform, enemyStats.ToArray());
        var path = pathfinding.FindPath(playerPos, stopPos);

        if (path != null && path.Count > 0)
        {
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(FollowPath(path));
            combatLog.text = "Moving to selected location...";
        }
        else
        {
            combatLog.text = "No valid path!";
            pendingAttackTarget = null;
            doNotMove = false;
        }
    }

    private IEnumerator FollowPath(List<Node> path)
    {
        foreach (var node in path)
        {
            while (Vector2.Distance(playerTransform.position, node.worldPosition) > 0.05f)
            {
                playerTransform.position = Vector2.MoveTowards(
                    playerTransform.position,
                    node.worldPosition,
                    moveSpeed * Time.deltaTime
                );
                moveDistanceTracker -= moveSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator EnemyFollowPath(List<Node> path, Transform enemy)
    {
        if (enemy == null)
    {
        Debug.LogError("Enemy reference is null in EnemyFollowPath!");
        yield break;
    }

    if (path == null || path.Count == 0)
    {
        Debug.Log("Enemy " + enemy.name + " has no path to follow.");
        yield break;
    }
        foreach (var node in path)
        {
            if (node == null) continue;

            while (Vector2.Distance(enemy.position, node.worldPosition) > 0.05f)
            {
                enemy.position = Vector2.MoveTowards(
                    enemy.position,
                    node.worldPosition,
                    moveSpeed * Time.deltaTime
                );
                enemyDistanceTracker -= moveSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }

    public void CastSpell(Spell spell)
    {
        if (state != BattleState.PlayerTurn) return;
        StartCoroutine(spell.Cast());
    }

    public void CastWeaponArt(WeaponArt art)
    {
        if (state != BattleState.PlayerTurn) return;
        StartCoroutine(art.Cast());
    }

    // --- Utility ---
    private int GetModifier(int abilityScore)
    {
        return Mathf.FloorToInt((abilityScore - 10) / 2f);
    }

    public bool CanSeeTarget(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(targetPosition, playerTransform.position);
        Vector2 directionToThreat = (targetPosition - (Vector2)playerTransform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, directionToThreat, distance, obstacleLayer);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    public void AddToList(StatsManager enemy)
    {
        enemyStats.Add(enemy);
    }

    public void ResetList()
    {
        enemyStats = new List<StatsManager> { };
    }

    public void StartCombat(){ inCombat = true; }
}
