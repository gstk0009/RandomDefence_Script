using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcUnits : Unit
{
    private void Start()
    {
        StartCoroutine(AttackInRange());
    }



    private IEnumerator AttackInRange()
    {
        while (true)
        {
            float attackSpeed = GetAttackSpeedBasedOnRank();
            yield return new WaitForSeconds(2.0f / attackSpeed); // 랭크별 공격 속도 적용

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, unitData.attackRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Attack(collider.transform);
                    break;
                }
            }
        }
    }

    private bool isRegistered = false;

    private void OnEnable()
    {
        if (!isRegistered)
        {
            EventHandler.Instance.OnUpgradeOrcUnits += UpgradeIfActive;
            isRegistered = true;
        }
    }

    private void OnDisable()
    {
        if (isRegistered)
        {
            EventHandler.Instance.OnUpgradeOrcUnits -= UpgradeIfActive;
            isRegistered = false;
        }
    }

    private void UpgradeIfActive()
    {
        if (isActiveAndEnabled)
        {
            unitData.enhanceCnt = EventHandler.Instance.orcEnhanceCnt;
            UpgradeAttack();
        }
    }

    public override void Attack(Transform target)
    {
        if (unitData != null)
        {
            GameObject bullet = GameManager.Instance.ObjectPool.SpawnFromPool("OrcBullet");

            if (bullet == null)
            {
                bullet = Instantiate(unitData.bulletObject, transform.position, Quaternion.identity);
            }
            else
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
            }

            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetTarget(target, unitData, Race.Orc, currentAttack);
            }
           
        }
        SoundManager.PlayClip("OrcShooting");
    }



    public override void UpgradeAttack()
    {
        if (unitData != null)
        {
            // Calculate bonus based on rank and enhanceCnt
            int bonus = CalculateBonusBasedOnRank();

            // Update currentAttack based on unitData and bonuses
            currentAttack = unitData.baseAttack + bonus;
        }
    
    }


    void OnApplicationQuit()
    {
        EventHandler.Instance.OnUpgradeOrcUnits -= UpgradeIfActive;
        Destroy(gameObject);
    }
}
