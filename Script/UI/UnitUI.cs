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
    [SerializeField] private Image tribeImage; // ���� �̹����� ǥ���� UI Image
    [SerializeField] private Sprite[] tribeSprites; // ������ �´� �̹����� ������ �迭
    // Unit�� Click ���� �� Unit ������ ����ִ� Class �޾ƿ���
    // �ϴ��� Object�� Test
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

    // Unit Click ���� �� �ҷ��� �Լ�
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
            // ������ �´� �̹����� �迭���� ����
            switch (race)
            {
                case Race.Elf:
                    tribeImage.sprite = tribeSprites[0]; // ���� ���� �̹���
                    break;
                case Race.Orc:
                    tribeImage.sprite = tribeSprites[1]; // ��ũ ���� �̹���
                    break;
                case Race.Human:
                    tribeImage.sprite = tribeSprites[2]; // �ΰ� ���� �̹���
                    break;
            }

            tribeImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("���� �̹��� �迭�� ����� �̹����� �����ϴ�.");
        }
    }
    // UnitStatus - SellBtn�� ����
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

        // ��ũ���� ���� ����
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

