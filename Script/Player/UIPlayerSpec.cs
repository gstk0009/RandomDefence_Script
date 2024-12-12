using System;
using System.Linq;
using UnityEngine;

public class UIPlayerSpec : MonoBehaviour
{
    public UIManager UImanager;
    public int playerGold;
    public int startPlayerGold;
    public int curStage;
    public int LeftMonsters;//남아있는 몬스터 수 

    public int[] enforceValue = Enumerable.Repeat(1,3).ToArray<int>();//크기가 3인 배열(종족별), 0으로 초기화시킴
    private void Start()
    {
        playerGold = startPlayerGold;
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(playerGold);
        curStage = 1;
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(playerGold);
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetStageUI(curStage);
        ResetEnforceValues();
    }

    public void AddGold(int value)//플레이어 골드 증가
    {
        playerGold += value;
    }

    public void AddEnforce(int raceIndex)//여기에 매개변수로 종족 enum을 넣기, 필요하다면 value 매개변수 추가해도 됨
    {
        enforceValue[raceIndex] += 1;
    }

    public int GetEnforceValue(int raceIndex)
    {
        return enforceValue[raceIndex];
    }

    public void AddMonsterCount()
    {
        LeftMonsters++;
        UImanager.SetMonsterCountUI(LeftMonsters);
        if (LeftMonsters >= 150)
        {
            UImanager.PopUpEndingUI();
        }
    }

    public void SubMonsterCount()
    {
        LeftMonsters--;
        UImanager.SetMonsterCountUI(LeftMonsters);
    }
    public void ResetEnforceValues()
    {
        for (int i = 0; i < enforceValue.Length; i++)
        {
            enforceValue[i] = 1;
        }
    }

    // RetryBtn 연결
    public void ResetGame()
    {
        curStage = 1;
        playerGold = startPlayerGold;
        LeftMonsters = 0;

        UImanager.SetGoldUI(playerGold);
        UImanager.SetStageUI(curStage);
        UImanager.SetMonsterCountUI(LeftMonsters);

        for (int race = 0; race < enforceValue.Length; race++)
        {
            enforceValue[race] = 1;
        }
    }
}