using System.Collections;
using UnityEngine;

public class Fireball : Spell
{
    public string spellName = "Fireball";
    public int bFManaCost = 40;
    public int maxPower = 30;
    public float spellRadius = 5f;
    public bool ranged = true;
    public int enemyCount;
    public AudioSource audioSource;
    public GameObject spellEffectPrefab;
    public BattleFieldManaDisplay bFMDisplay;
    public GameObject spellBook;
    public TMPro.TMP_Text battleLog;
    public GameObject playerTransform;
    public LayerMask obstacleLayer;
    public MakeDamageNumbers mdn;
    public ScreenShake screenShake;
    public override IEnumerator Cast()
    {
        enemyCount = 0;
        // Implementation of Fireball effect
        if (bFMDisplay.GetCurrentMana() >= bFManaCost)
        {
            GameObject spellEffect = Instantiate(spellEffectPrefab, playerTransform.transform.position, Quaternion.identity);
            spellBook.SetActive(false);
            spellEffect.transform.localScale = new Vector3(spellRadius, spellRadius, spellRadius);
            battleLog.text = "Ranged Spell selected, left click to select location or press f to cancel.";
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F));
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                battleLog.text = "Weapon Art cancelled.";
                Destroy(spellEffect);
                yield break;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                
                spellEffect.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellEffect.transform.position = new Vector3(spellEffect.transform.position.x, spellEffect.transform.position.y, 0);
                float distance = Vector2.Distance(playerTransform.transform.position, spellEffect.transform.position);
                Vector2 directionToThreat = (spellEffect.transform.position - (Vector3)playerTransform.transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(playerTransform.transform.position, directionToThreat, distance, obstacleLayer);
                if (hit.collider == null)
                {
                    battleLog.text = "Spell location set. click again to cast or press f to cancel.";
                    yield return new WaitWhile(() => Input.GetMouseButton(0));
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F));
                    if (Input.GetMouseButton(0))
                    {
                        int damageRoll = DiceManager.DiceManager.D6(6);
                        // Apply effect to enemies within radius
                        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spellEffect.transform.position, spellRadius / 2);
                        foreach (var hitCollider in hitColliders)
                        {
                            if (hitCollider.CompareTag("Enemy"))
                            {
                                hitCollider.GetComponent<StatsManager>().HealthChange(-damageRoll);
                                //StartCoroutine(mdn.MakeDamageNumber(maxPower.ToString(), hitCollider.transform.position));
                                enemyCount++;
                            }
                        }
                        audioSource.Play();
                        screenShake.StartShake();
                        Destroy(spellEffect);
                        bFMDisplay.ReduceMana(bFManaCost);
                        battleLog.text = "dealt " + damageRoll + " to " + enemyCount + " enemies";
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Destroy(spellEffect);
                        yield break;
                    }
                }
                else
                {
                    battleLog.text = "Can't see target position";
                    Destroy(spellEffect);
                }
            }
        }
        else
            battleLog.text = "Not enough mana to cast " + spellName;
    }
}
