using System.Collections;
using UnityEngine;

public class EarthQuake : WeaponArt
{
    public string artName = "Earthquake";
    public int bFManaCost = 27;
    public int maxPower = 20;
    public float effectRadius = 7f;
    public bool ranged = false;
    public int enemyCount;
    public AudioSource audioSource;
    public GameObject spellEffectPrefab;
    public BattleFieldManaDisplay bFMDisplay;
    public GameObject weaponArts;
    public TMPro.TMP_Text battleLog;
    public Transform playerTransform;
    public MakeDamageNumbers mdn;
    public ScreenShake screenShake;
    public override IEnumerator Cast()
    {
        // Implementation of Fireball effect
        if (bFMDisplay.GetCurrentMana() >= bFManaCost)
        {
            GameObject spellEffect = Instantiate(spellEffectPrefab, playerTransform.position, Quaternion.identity);
            weaponArts.SetActive(false);
            spellEffect.transform.localScale = new Vector3(effectRadius, effectRadius, effectRadius);
            battleLog.text = "Weapon Art selected, left click to cast or press f to cancel.";
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F));
            if (Input.GetKeyDown(KeyCode.F))
            {
                battleLog.text = "Weapon Art cancelled.";
                Destroy(spellEffect);
                yield break;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                int damageRoll = DiceManager.DiceManager.D4(5);
                // Apply effect to enemies within radius
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerTransform.position, effectRadius/2);
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
            
        }
        else
            battleLog.text = "Not enough mana to cast " + artName;
    }
}
