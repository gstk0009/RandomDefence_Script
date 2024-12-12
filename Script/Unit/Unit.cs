using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum Rank
{
    F,
    E,
    D,
    C,
    B,
    A,
    S,
    SS,
    SSS
}
public interface ISoundPlayer
{
    void PlaySummonSound();
}

public abstract class Unit : MonoBehaviour
{
    public UnitSO unitData;
    public int currentAttack;
    public Rank rank;
    public int CreateUnitId;
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    public void Initialize(UnitSO data)
    {
        unitData = data;
        if (unitData != null)
        {
            animator = GetComponent<Animator>();

          
            if (animator != null && unitData.animatorController != null)
            {
                animator.runtimeAnimatorController = unitData.animatorController;
            }

            AssignRandomRank();

            UpgradeAttack(); // 강화된 공격력 초기화
            ChangeColorBasedOnRank();

        }

    }

    private void AssignRandomRank()
    {
        float[] rankProbabilities = new float[] { 50.0f, 33.2f, 10.2f, 5.1f, 0.8f, 0.5f, 0.2f, 0.08f, 0.019f };
        Rank[] ranks = (Rank[])System.Enum.GetValues(typeof(Rank));
         
        float totalProbability = 0f;
        foreach (float prob in rankProbabilities)
        {
            totalProbability += prob;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        for (int i = 0; i < ranks.Length; i++)
        {
            cumulativeProbability += rankProbabilities[i];

            if (randomValue <= cumulativeProbability)
            {
                rank = ranks[i];
                break;
            }
        }
        PlaySoundBasedOnRank();
    }

    private void ChangeColorBasedOnRank()
    {
        if (animator == null)
            return;

        Color newColor = Color.white; // 기본 색상은 흰색

        switch (rank)
        {
            case Rank.S:
                newColor = Color.green; // 초록색
                break;
            case Rank.SS:
                newColor = Color.blue; // 파란색
                break;
            case Rank.SSS:
                newColor = Color.red; // 빨간색
                break;
                // 추가적인 색상들에 대해 설정
        }

        // 각 애니메이션에 사용되는 Material을 변경
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.material.color = newColor;
        }
    }
    public int CalculateBonusBasedOnRank()
    {
        Dictionary<Rank, int> rankBonusMap = new Dictionary<Rank, int>
        {
            { Rank.F, 2 },
            { Rank.E, 5 },
            { Rank.D, 7 },
            { Rank.C, 10 },
            { Rank.B, 15 },
            { Rank.A, 20 },
            { Rank.S, 25 },
            { Rank.SS, 30 },
            { Rank.SSS, 40 }
        };

        int bonus = 0;
        if (rankBonusMap.TryGetValue(rank, out int rankBonus))
        {
            bonus = rankBonus * unitData.enhanceCnt;
        }

        return bonus;
    }
    public float GetAttackSpeedBasedOnRank()
    {
        Dictionary<Rank, float> rankAttackSpeedMap = new Dictionary<Rank, float>
        {
            { Rank.F, 0.25f },
            { Rank.E, 0.375f },
            { Rank.D, 0.5f },
            { Rank.C, 0.625f },
            { Rank.B, 0.75f },
            { Rank.A, 0.875f },
            { Rank.S, 1.0f },
            { Rank.SS, 1.25f },
            { Rank.SSS, 1.5f }
        };

        if (rankAttackSpeedMap.TryGetValue(rank, out float attackSpeedMultiplier))
        {
            return unitData.attackSpeed * attackSpeedMultiplier;
        }

        return unitData.attackSpeed; 
    }

    private void PlaySoundBasedOnRank()
    {
        switch (rank)
        {
            case Rank.S:
            case Rank.SS:
            case Rank.SSS:
                SoundManager.PlayClip("PickSound");
                break;
        }
    }
    public int GetGoldValue()
    {
        int index = (int)rank;
        if (index >= 0 && index < unitData.goldValues.Length)
        {
            return unitData.goldValues[index];
        }
        else
        {
            return 0;
        }
    }
    public abstract void UpgradeAttack();
    public abstract void Attack(Transform target);
}

