using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public MonsterSO monsterSO;
    public MonsterMovement monsterMovement;
    public MonsterHealth monsterHealth;
    public Animator animator;

    private void Start()
    {
        // ���� ������ ����
        monsterMovement = GetComponent<MonsterMovement>();
        monsterHealth = GetComponent<MonsterHealth>();
        if (monsterMovement != null)
        {
            Vector3[] waypoints = new Vector3[] { new Vector3(-8f, 4f, 0), new Vector3(-8f, -4.5f, 0), new Vector3(8f, -4.5f, 0), new Vector3(8f, 4f, 0) };
            monsterMovement.Initialize(monsterSO.speed, waypoints);
        }

    }

    public void OnEnable()
    {
        //monsterMovement = GetComponent<MonsterMovement>();
        //monsterHealth = GetComponent<MonsterHealth>();

        if (monsterHealth != null)
        {
            monsterHealth.maxHp = monsterSO.health;
            monsterHealth.currentHp = monsterSO.health;
            monsterHealth.monsterSize = monsterSO.size;
            animator.runtimeAnimatorController = monsterSO.animatorController;
        }

        //Ÿ��Ʋ ��ũ�� ����� ����
        if (GameManager.Instance.Player.uiPlayerSpec.curStage == 1)
        {
            monsterHealth.maxHp = 8;
            monsterHealth.currentHp = 8;
            monsterHealth.monsterSize = PlayerUnitType.DRAGON;
            animator.runtimeAnimatorController = monsterSO.animatorController;
        }

        // ���� ��ġ �ʱ�ȭ
        if (monsterMovement != null)
        {
            monsterMovement.ResetPosition();
        }
    }

    public void OnDisable()
    {
        if (monsterHealth != null)
            monsterHealth.isImBoss = false;
    }
}
