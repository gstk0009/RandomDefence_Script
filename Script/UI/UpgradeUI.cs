using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] upgradeGoldTxt;
    private Race species;
    private bool isUpgrade;

    private void Start()
    {
        upgradeGoldTxt[0].text = 10.ToString();
        upgradeGoldTxt[1].text = 10.ToString();
        upgradeGoldTxt[2].text = 10.ToString();
    }

    public void UpgradeUIBtn()
    {
        if (gameObject == null) return;

        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public void ZergBtn()
    {
        species = Race.Elf;
        Upgrade();
        if(isUpgrade)
            EventHandler.Instance.TriggerUpgradeElfUnitsEvent();
        isUpgrade = false;
    }

    public void TerranBtn()
    {
        species = Race.Orc;
        Upgrade();
        if(isUpgrade)
            EventHandler.Instance.TriggerUpgradeOrcUnitsEvent();
        isUpgrade = false;
    }

    public void ProtossBtn()
    {
        species = Race.Human;
        Upgrade();
        if (isUpgrade)
            EventHandler.Instance.TriggerUpgradeHumanUnitsEvent();
        isUpgrade = false;
    }

    public void Upgrade()
    {
        int userGold = GameManager.Instance.Player.uiPlayerSpec.playerGold;
        int upgradeGold = GameManager.Instance.Player.uiPlayerSpec.GetEnforceValue((int)species);
        int nowUpgradeGold = upgradeGold * 10;
        int nextUpgradeGold = (upgradeGold + 1) * 10;

        if (userGold < nowUpgradeGold)
        {
            GameManager.Instance.Player.uiPlayerSpec.UImanager.SetNoGoldUI();
            return;
        }
        isUpgrade = true;
        GameManager.Instance.Player.uiPlayerSpec.AddEnforce((int)species);
        GameManager.Instance.Player.uiPlayerSpec.playerGold = userGold - nowUpgradeGold;
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(GameManager.Instance.Player.uiPlayerSpec.playerGold);
        upgradeGoldTxt[(int)species].text = (nextUpgradeGold).ToString();
    }

    // Retry Btn ¿¬°á
    public void ResetGame()
    {
        upgradeGoldTxt[0].text = 10.ToString();
        upgradeGoldTxt[1].text = 10.ToString();
        upgradeGoldTxt[2].text = 10.ToString();
    }
}
