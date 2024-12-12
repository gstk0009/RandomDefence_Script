using System;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Transform spawnLocation;
    private float currTime = 0;
    public float spawnFrequency = 0.5f;
    public MonsterSO monsterSO;

    private bool isBossSpawn;
    private int bossCount = 1;
    private int Spawnlength = 10;

    public TextAsset csvData;
    private string[] data;
    private string[] elements;

    public RuntimeAnimatorController[] DragonAnimators;
    public RuntimeAnimatorController[] HumanoidAnimators;
    public RuntimeAnimatorController[] MagicalAnimators;
    public RuntimeAnimatorController[] BossAnimators;

    void Start()
    {
        data = csvData.text.Split(new char[] { '\n' });
        monsterSO.animatorController = DragonAnimators[0];
    }

    void Update()
    {
        if (Spawnlength != 0)
        {
            currTime += Time.deltaTime;
            if (currTime >= spawnFrequency) 
            {
                MonsterSpawn(GameManager.Instance.Player.uiPlayerSpec.curStage);
                GameManager.Instance.Player.uiPlayerSpec.AddMonsterCount();
                Spawnlength--;
                currTime = 0;
            }
        }
        else if (GameManager.Instance.Player.uiPlayerSpec.UImanager.StageLimitTime <= 0)
        {
            StageLevelUp();
        }
    }

    private void StageLevelUp()
    {
        int nowcurStage = GameManager.Instance.Player.uiPlayerSpec.curStage += 1;
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetStageUI(nowcurStage);
        elements = data[GameManager.Instance.Player.uiPlayerSpec.curStage].Split(new char[] { ',' });
        Spawnlength = int.Parse(elements[4]);
        GameManager.Instance.Player.uiPlayerSpec.UImanager.StageLimitTime = float.Parse(elements[3]);//공식은 추후 수정
        monsterSO.health = float.Parse(elements[2]);
        monsterSO.size = (PlayerUnitType)Enum.Parse(typeof(PlayerUnitType), elements[1]);
        monsterSO.speed = float.Parse(elements[6]);
        monsterSO.Name = elements[5];
        if (elements[7] == "TRUE\r")
        {
            isBossSpawn = true;
            bossCount++;
            monsterSO.animatorController = DragonAnimators[UnityEngine.Random.Range(0, BossAnimators.Length)];
        }
        else
        {
            switch (monsterSO.size)
            {
                case PlayerUnitType.DRAGON:
                    {
                        monsterSO.animatorController = DragonAnimators[UnityEngine.Random.Range(0, DragonAnimators.Length)];
                        break;
                    }
                case PlayerUnitType.MAGICAL:
                    {
                        monsterSO.animatorController = MagicalAnimators[UnityEngine.Random.Range(0, MagicalAnimators.Length)];
                        break;
                    }
                case PlayerUnitType.HUMANOID:
                    {
                        monsterSO.animatorController = HumanoidAnimators[UnityEngine.Random.Range(0, HumanoidAnimators.Length)];
                        break;
                    }
            }
        }

    }

    private void MonsterSpawn(int curStage)
    {
        GameObject monster = GameManager.Instance.ObjectPool.SpawnFromPool("Monster",monsterSO);
        if (isBossSpawn)
        {
            GameManager.Instance.IsBoss = isBossSpawn;
            isBossSpawn = false;
            monster.gameObject.GetComponent<MonsterHealth>().SetBossCount(bossCount, true);
        }
        monster.transform.position = spawnLocation.position;
    }

    public void ResetSpawn()
    {
        Spawnlength = 10;
        bossCount = 1;
        isBossSpawn = false;
        monsterSO.animatorController = DragonAnimators[0];
    }
}
