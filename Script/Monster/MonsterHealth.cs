using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    public float maxHp;
    public float currentHp;

    public Slider hpSlider;

    public PlayerUnitType monsterSize;

    [HideInInspector] public bool isImBoss;
    [HideInInspector] public int DieGold = 1;
    private int bossCount;

    private void OnEnable()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        UpdateHpUI();
    }

    public void Die()
    {
        GameManager.Instance.Player.uiPlayerSpec.SubMonsterCount();
        if(isImBoss)
        {
            GameManager.Instance.Player.uiPlayerSpec.playerGold += (20 * bossCount);
            GameManager.Instance.IsBossClear = true;
            GameManager.Instance.IsBoss = false;
            isImBoss = false;
        }
        else
        {
            GameManager.Instance.Player.uiPlayerSpec.playerGold += DieGold;
        }
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(GameManager.Instance.Player.uiPlayerSpec.playerGold);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.currentAttack, bullet.attackerRace); // currentAttack °ª »ç¿ë
            }
            else
            {
                Debug.LogError("Enemy does not have a MonsterHealth component.");
            }
        }
    }

    public void TakeDamage(float damage, Race race)
    {
        float finalDamage = ApplyDamageModifiers(damage, race, monsterSize);

        if (currentHp <= 0) return;

        currentHp -= finalDamage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

        UpdateHpUI();
    }

    private float ApplyDamageModifiers(float baseDamage, Race race, PlayerUnitType monsterSize)
    {
        float damageModifier = 1f;

        switch (race)
        {
            case Race.Human:
                if (monsterSize == PlayerUnitType.MAGICAL) damageModifier = 1.5f;
                else if (monsterSize == PlayerUnitType.DRAGON) damageModifier = 0.5f;
                break;
            case Race.Elf:
                if (monsterSize == PlayerUnitType.DRAGON) damageModifier = 1.5f;
                else if (monsterSize == PlayerUnitType.HUMANOID) damageModifier = 0.5f;
                break;
            case Race.Orc:
                if (monsterSize == PlayerUnitType.HUMANOID) damageModifier = 1.5f;
                else if (monsterSize == PlayerUnitType.MAGICAL) damageModifier = 0.5f;
                break;
        }

        return baseDamage * damageModifier;
    }

    private void UpdateHpUI()
    {
        if (hpSlider != null) hpSlider.value = currentHp / maxHp;
    }

    public void SetBossCount(int count, bool isBoss)
    {
        bossCount = count;
        isImBoss = isBoss;
    }
}
