using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private UnitSO unitData;
    public Race attackerRace;
    public int currentAttack; // �߰�: currentAttack �ʵ�

    public void Initialize()
    {
        target = null;
        unitData = null;
        attackerRace = Race.Orc;
        currentAttack = 0; // �ʱ�ȭ
    }

    public void SetTarget(Transform targetTransform, UnitSO unitSO, Race attackerRace, int attack)
    {
        if (targetTransform != null && unitSO != null)
        {
            target = targetTransform;
            unitData = unitSO;
            this.attackerRace = attackerRace;
            this.currentAttack = attack; // currentAttack ����
            Debug.Log($"Target set: {target.name}, attackSpeed: {unitData.attackSpeed}, attackerRace: {attackerRace}, currentAttack: {currentAttack}");

            // 1.5�� �Ŀ� ��Ȱ��ȭ
            StartCoroutine(DeactivateBulletAfterTime(1.5f));//���:�ٷ� ������� �ؼ� ���� �� �����Ǵ� ���� ���ĺ��� �õ�1
        }
        else
        {
            Debug.LogError("Target or UnitSO is null!");
        }
    }

    private void Update()
    {
        if (target != null && unitData != null)
        {
            if (target.gameObject.activeInHierarchy)
            {
                float speed = unitData.attackSpeed;
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                Debug.Log($"Bullet moving towards: {target.name} at speed: {speed}");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //MonsterHealth monsterHealth = collision.GetComponent<MonsterHealth>();
            //if (monsterHealth != null)
            //{
            //    monsterHealth.TakeDamage(currentAttack, attackerRace); // currentAttack �� ���
            //}
            //else
            //{
            //    Debug.LogError("Enemy does not have a MonsterHealth component.");
            //}

            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        target = null;
        unitData = null;
        attackerRace = Race.Orc;
        currentAttack = 0;
    }

    private IEnumerator DeactivateBulletAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
