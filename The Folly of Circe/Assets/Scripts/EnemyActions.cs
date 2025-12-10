using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Globalization;

public class EnemyActions : MonoBehaviour
{
    public AudioSource audioSource;
    public bool vampiric = false;
    public AudioClip hitSound;
    public AudioClip missSound;
    public Animator animator;
    public StatsManager statsManager;
    public GridManager gridManager;
    public Pathfinding pathfinding;
    public EnemyClass ec;
    public CreateMoveDots createMoveDots;
    public LayerMask obstacleLayer;
    public MakeDamageNumbers mdn;
    public GameObject self;
    public int actionPointsMax = 1;
    public int actionPoints = 1;
    public float moveDistanceMax = 9f;
    public float moveDistanceTracker = 9f;
    public float moveSpeed = 2f;
    public bool inCombat = false;
    public int seeAttempts = 1;

    void Start()
    {
        moveSpeed = 4f;
    }

    public void DetectEnemy(Transform player)
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if(CanSeeTarget(player.position) && distanceToPlayer < 9.0) 
        {
            int percCheck = DiceManager.DiceManager.D20(1) + GetModifier(statsManager.wisdom);
            if(percCheck > 12)
            {
                inCombat = true;
                mdn.MakeDamageNumber("!",transform.position ,true);
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        EnemyActions EA = hitCollider.GetComponent<EnemyActions>();
                        if(EA.inCombat) continue;
                        else EA.inCombat = true;
                    }
                }
            }
            seeAttempts--;
        }
    }

    public IEnumerator MoveToPosition(Node targetNode)
    {
        if (targetNode == null)
        {
            Debug.Log("Target node is null, cannot move.");
            yield break;
        }
        createMoveDots.MakeDots(self.transform.position, targetNode.worldPosition, moveDistanceTracker);
        Vector2 stopPos = createMoveDots.finalReachablePosition;
        createMoveDots.DestroyDots();
        var path = pathfinding.FindPath(self.transform.position, stopPos);
        yield return StartCoroutine(FollowPath(path, transform));
    }

    public bool CanSeeTarget(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(targetPosition, self.transform.position);
        Vector2 directionToThreat = (targetPosition - (Vector2)self.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(self.transform.position, directionToThreat, distance, obstacleLayer);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    public Node FindUncover(Vector2 threatPosition, float moveDistance)
    {
        float bestNodeWeight = 0;
        Node bestNode = null;
        foreach (Node node in pathfinding.ReachableNodes(self.transform.position, moveDistance))
        {
            float nodeWeight = 0;
            float distance = Vector2.Distance(node.worldPosition, self.transform.position);
            float distanceToTarget = Vector2.Distance(node.worldPosition, threatPosition);
            Vector2 directionToThreat = (threatPosition - (Vector2)node.worldPosition).normalized;

            RaycastHit2D hit = Physics2D.Raycast(node.worldPosition, directionToThreat, distanceToTarget, obstacleLayer);
            if (hit.collider == null)
                nodeWeight += 100f;
            
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(node.worldPosition, 2.5f);
            foreach (var hitCollider in hitColliders)
            {
                if (CompareTag("Enemy"))
                    nodeWeight -= 5f;
            }
            nodeWeight -= distance;
            
            if (nodeWeight > bestNodeWeight)
            {
                bestNodeWeight = nodeWeight;
                bestNode = node;
            }
        }
        return bestNode;
    }

    public Node FindMeleePosition(Vector2 target, float moveDistance)
    {
        float bestNodeWeight = 0;
        Node bestNode = null;
        float distanceToTarget;
        foreach (Node node in pathfinding.ReachableNodes(self.transform.position, moveDistance))
        {
            float nodeWeight = 0;
            distanceToTarget = Vector2.Distance(node.worldPosition, target);
            if( distanceToTarget >= 1.5f)
                continue;
            float distance = Vector2.Distance(node.worldPosition, self.transform.position);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(node.worldPosition, .75f);
            foreach (var hitCollider in hitColliders)
            {
                if (CompareTag("Enemy"))
                    nodeWeight -= 5f;
            }

            if(distanceToTarget < .75)
            {
                nodeWeight -= nodeWeight;
            }

            nodeWeight -= distance;

            if (bestNodeWeight > nodeWeight)
            {
                bestNodeWeight = nodeWeight;
                bestNode = node;
            }
        }
        return bestNode;
    }
    
    public Node FindCover(Vector2 threatPosition, float moveDistance)
    {
        float bestNodeWeight = 0;
        Node bestNode = null;
        foreach (Node node in pathfinding.ReachableNodes(self.transform.position, moveDistance))
        {
            float nodeWeight = 0;
            float distance = Vector2.Distance(node.worldPosition, self.transform.position);
            Vector2 directionToThreat = (threatPosition - (Vector2)node.worldPosition).normalized;
            float distanceToTarget = Vector2.Distance(node.worldPosition, threatPosition);

            RaycastHit2D hit = Physics2D.Raycast(node.worldPosition, directionToThreat, distanceToTarget, obstacleLayer);
            if (hit.collider != null)
                nodeWeight += 100f;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(node.worldPosition, 3.0f);
            foreach (var hitCollider in hitColliders)
            {
                if (CompareTag("Enemy"))
                    nodeWeight -= 5f;
                if (CompareTag("Player"))
                    nodeWeight -= nodeWeight;
            }

            nodeWeight -= distance;
            nodeWeight += distanceToTarget;

            if (nodeWeight > bestNodeWeight)
            {
                bestNodeWeight = nodeWeight;
                bestNode = node;
            }
        }
        return bestNode;
    }

    public IEnumerator UseRangedAttack(StatsManager target)
    {
        Vector2 directionToThreat = ((Vector2)target.transform.position - (Vector2)self.transform.position).normalized;
        float dot = Vector2.Dot(transform.right, directionToThreat);
        if(dot > 0) animator.SetBool("AttackRight", true);
        else animator.SetBool("AttackLeft", true);
        int diceRoll = DiceManager.DiceManager.D20(1);
        int attackRoll = diceRoll + GetModifier(statsManager.strength);
        yield return new WaitForSeconds(0.85f);
        if (diceRoll == 20)
        {
            int damage = 6 + DiceManager.DiceManager.D6(1) + (2 * GetModifier(statsManager.dexterity));
            target.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString() + "!", target.transform.position, true));
            audioSource.clip = hitSound;
            audioSource.Play();
        }
        else if (attackRoll + GetModifier(statsManager.dexterity) >= target.armorClass)
        {
            int damage = DiceManager.DiceManager.D6(1) + GetModifier(statsManager.dexterity);
            target.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString(), target.transform.position, true));
            audioSource.clip = hitSound;
            audioSource.Play();
        }
        else if(attackRoll < target.armorClass)
        {
            StartCoroutine(mdn.MakeDamageNumber("Miss!", target.transform.position, true));
            audioSource.clip = missSound;
            audioSource.Play();
        }
        animator.SetBool("AttackRight", false);
        animator.SetBool("AttackLeft", false);
        animator.SetBool("Idle", true);
    }

    public IEnumerator UseMeleeAttack(StatsManager target)
    {
        Vector2 directionToThreat = ((Vector2)target.transform.position - (Vector2)self.transform.position).normalized;
        float dot = Vector2.Dot(transform.forward, directionToThreat);
        float dotR = Vector2.Dot(transform.right, directionToThreat);
            if(dotR > dot)animator.SetBool("AttackRight", true);
            else animator.SetBool("AttackLeft", true);
        int diceRoll = DiceManager.DiceManager.D20(1);
        int attackRoll = diceRoll + GetModifier(statsManager.strength);
        yield return new WaitForSeconds(0.7f);
        if (diceRoll == 20)
        {
            int damage = 4 + DiceManager.DiceManager.D8(1) + (2 * GetModifier(statsManager.strength));
            target.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString() + "!", target.transform.position, true));
            if(vampiric) statsManager.health += Mathf.FloorToInt(damage/2);
            audioSource.clip = hitSound;
            audioSource.Play();
        }
        else if (attackRoll + GetModifier(statsManager.strength) >= target.armorClass)
        {
            int damage = DiceManager.DiceManager.D8(1) + GetModifier(statsManager.strength);
            target.HealthChange(-damage);
            StartCoroutine(mdn.MakeDamageNumber(damage.ToString(), target.transform.position, true));
            if(vampiric) statsManager.health += Mathf.FloorToInt(damage/2);
            audioSource.clip = hitSound;
            audioSource.Play();
        }
        else if(attackRoll < target.armorClass)
        {
            StartCoroutine(mdn.MakeDamageNumber("Miss!", target.transform.position, true));
            audioSource.clip = missSound;
            audioSource.Play();
        }
        animator.SetBool("AttackRight", false);
        animator.SetBool("AttackLeft", false);
        animator.SetBool("Idle", true);
    }

    public IEnumerator TakeAction(StatsManager target)
    {
        yield return StartCoroutine(DecideAction(target));
    }

    public IEnumerator DecideAction(StatsManager target)
    {
        if (ec.noAI)
        {
            StartCoroutine(mdn.MakeDamageNumber("Hey!", self.transform.position, false));
        }
        else if (ec.rangedAttackWeight >= ec.meleeAttackWeight && ec.rangedAttackWeight >= ec.spellAttackWeight)
        {
            bool threatened = (Vector2.Distance(target.transform.position, self.transform.position) <= 3.0f);
            bool attacked = false;
            if (CanSeeTarget(target.transform.position))
            {
                yield return StartCoroutine(UseRangedAttack(target));
                attacked = true;
                actionPoints--;
            }
            else
            {
                yield return StartCoroutine(MoveToPosition(FindUncover(target.transform.position, 100f)));
                if (CanSeeTarget(target.transform.position))
                {
                    yield return StartCoroutine(UseRangedAttack(target));
                    attacked = true;
                    actionPoints--;
                }
                else
                {
                    actionPoints--;
                    StartCoroutine(mdn.MakeDamageNumber("Dash", self.transform.position, false));
                    moveDistanceTracker += moveDistanceMax;
                    yield return StartCoroutine(MoveToPosition(FindUncover(target.transform.position, 100f)));
                }
            }
            if(attacked && threatened)
                yield return StartCoroutine(MoveToPosition(FindCover(target.transform.position, 100f)));
        }
        else if (ec.meleeAttackWeight >= ec.rangedAttackWeight && ec.meleeAttackWeight >= ec.spellAttackWeight)
        {
            Node canMeleePos = FindMeleePosition(target.transform.position, 100f);
            if (Vector2.Distance(self.transform.position, target.transform.position) <= 1.5f && CanSeeTarget(target.transform.position))
            {
                yield return StartCoroutine(UseMeleeAttack(target));
            }
            else
            {
                yield return StartCoroutine(MoveToPosition(canMeleePos));
                if (Vector2.Distance(self.transform.position, target.transform.position) <= 1.5f && CanSeeTarget(target.transform.position))
                    yield return StartCoroutine(UseMeleeAttack(target));
                else
                {
                    StartCoroutine(mdn.MakeDamageNumber("Dash", self.transform.position, false));
                    moveDistanceTracker += moveDistanceMax;
                    yield return StartCoroutine(MoveToPosition(FindMeleePosition(target.transform.position, 100f)));
                }

            }
            actionPoints--;
        }
        //else if (ec.spellAttackWeight >= ec.rangedAttackWeight && ec.spellAttackWeight >= ec.meleeAttackWeight)
        //{
        //    CastSpell(target);
        //}
        if (actionPoints > 0)
        {
            yield return StartCoroutine(TakeAction(target));
        }
        actionPoints = actionPointsMax;
        moveDistanceTracker = moveDistanceMax;
    }

    public int GetModifier(int abilityScore)
    {
        return Mathf.FloorToInt((abilityScore - 10) / 2);
    }
    
    private IEnumerator FollowPath(List<Node> path, Transform enemy)
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
                moveDistanceTracker -= moveSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }

    public void Die()
    {
        animator.SetBool("Dead", true);
        animator.SetBool("MovUp", false);
        animator.SetBool("MovDown", false);
        animator.SetBool("MovLeft", false);
        animator.SetBool("MovRight", false);
        animator.SetBool("Idle", false);
    }
}
