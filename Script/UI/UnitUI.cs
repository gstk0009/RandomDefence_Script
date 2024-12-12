using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

enum TextName
{
    Class,
    Tribe,
    Damage,
    AttackSpeed,
    Gold
}

public class UnitUI : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] private Image tribeImage; // 종족 이미지를 표시할 UI Image
    [SerializeField] private Sprite[] tribeSprites; // 종족에 맞는 이미지를 저장할 배열
    // Unit을 Click 했을 때 Unit 정보가 들어있는 Class 받아오기
    // 일단은 Object로 Test
    private GameObject selectUnit;
    private Unit selectUnitComponent;
    private int selectUnitGold;

    private void Awake()
    {
        spawner = spawner.GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnUIUpdate += HandleUIUpdate;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnUIUpdate -= HandleUIUpdate;
    }

    // Unit Click 했을 때 불러올 함수
    public void UpdateUnitStatus(string classs, string tribe, int upgrade, int damage ,int attackSpeed, int gold, GameObject select)
    {
        gameObject.SetActive(true);

        selectUnit = select;
        selectUnitComponent = selectUnit.GetComponent<Unit>();

        if (selectUnitComponent != null)
        {
            float attackSpeedFloat = selectUnitComponent.GetAttackSpeedBasedOnRank();
            int attackSpeedInt = Mathf.RoundToInt(attackSpeedFloat);
            texts[(int)TextName.Class].text = classs;
            texts[(int)TextName.Tribe].text = tribe;
            texts[(int)TextName.Damage].text = damage.ToString();
            texts[(int)TextName.AttackSpeed].text = attackSpeedInt.ToString();
            texts[(int)TextName.Gold].text = gold.ToString();

            SetTribeImage(selectUnitComponent.unitData.race);
            SetRankColor(selectUnitComponent.rank);
        }
    }
    private void SetTribeImage(Race race)
    {
        if (tribeSprites.Length >= 3)
        {
            // 종족에 맞는 이미지를 배열에서 선택
            switch (race)
            {
                case Race.Elf:
                    tribeImage.sprite = tribeSprites[0]; // 엘프 종족 이미지
                    break;
                case Race.Orc:
                    tribeImage.sprite = tribeSprites[1]; // 오크 종족 이미지
                    break;
                case Race.Human:
                    tribeImage.sprite = tribeSprites[2]; // 인간 종족 이미지
                    break;
            }

            tribeImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("종족 이미지 배열에 충분한 이미지가 없습니다.");
        }
    }
    // UnitStatus - SellBtn에 연결
    public void SellUnit()
    {
        int id = selectUnit.GetComponent<Unit>().CreateUnitId;

        spawner.createUnitId -= 1;

        for (int index = 0; index < GameManager.Instance.CreateUnit.Count; index++)
        {
            if (GameManager.Instance.CreateUnitId[index] == id)
                GameManager.Instance.RemoveCreateUnit(index);
        }

        MapArea unitMapArea = selectUnit.GetComponentInParent<MapArea>();
        if (unitMapArea != null)
        {
            unitMapArea.hasUnit = false;
            unitMapArea.unitObject = null;
        }

        Destroy(selectUnit);

        int unitGold = selectUnitComponent.GetGoldValue();
        GameManager.Instance.Player.uiPlayerSpec.AddGold(unitGold);
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(GameManager.Instance.Player.uiPlayerSpec.playerGold);
        selectUnit = null;
        selectUnitGold = 0;
        gameObject.SetActive(false);
    }
    private void HandleUIUpdate()
    {
        if (selectUnitComponent != null)
        {
            float attackSpeedFloat = selectUnitComponent.GetAttackSpeedBasedOnRank();
            int attackSpeedInt = Mathf.RoundToInt(attackSpeedFloat);

            UpdateUnitStatus(
                selectUnitComponent.rank.ToString(),
                selectUnitComponent.unitData.race.ToString(),
                selectUnitComponent.unitData.enhanceCnt,
                selectUnitComponent.currentAttack,
                attackSpeedInt,
                selectUnitComponent.GetGoldValue(),
                selectUnitComponent.gameObject);
        }
    }
    private void SetRankColor(Rank rank)
    {
        Color rankColor;

        // 랭크별로 색상 정의
        switch (rank)
        {    
            case Rank.A:
                rankColor = Color.magenta; 
                break;
            case Rank.S:
                rankColor = Color.yellow; 
                break;
            case Rank.SS:
                rankColor = Color.red;
                break;
            case Rank.SSS:
                rankColor = new Color(1.0f, 0.64f, 0.0f);
                break;
            default:
                rankColor = Color.white;
                break;
        }

        texts[(int)TextName.Class].color = rankColor;
    }
}

