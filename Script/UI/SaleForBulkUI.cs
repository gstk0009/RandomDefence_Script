using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaleForBulk : MonoBehaviour
{
    private List<int> sellIds = new List<int>();
    private string species = null;

    public void PopUpSaleForBulk()
    {
        if (gameObject == null) return;

        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public void SelectSpeciesElf()
    {
        species = "Elf";
    }

    public void SelectSpeciesOrc()
    {
        species = "Orc";
    }

    public void SelectSpeciesHuman()
    {
        species = "Human";
    }

    public void SellBulk()
    {
        if (species == null) return;
        for (int ids = 0; ids < GameManager.Instance.CreateUnit.Count; ids++)
        {
            if (GameManager.Instance.CreateUnit[ids].CompareTag(species) && GameManager.Instance.CreateUnitRank[ids] <= (int)Rank.D)
            {
                GameManager.Instance.Player.uiPlayerSpec.playerGold += GameManager.Instance.CreateUnitGold[ids];
                sellIds.Add(ids);
            }
        }

        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(GameManager.Instance.Player.uiPlayerSpec.playerGold);

        for (int sellId = 0; sellId < sellIds.Count; sellId++)
        {
            int ids = sellIds[sellId];
            GameObject obj = GameManager.Instance.CreateUnit[ids];

            MapArea unitMapArea = obj.GetComponentInParent<MapArea>();
            if (unitMapArea != null)
            {
                unitMapArea.hasUnit = false;
                unitMapArea.unitObject = null;
            }

            Destroy(GameManager.Instance.CreateUnit[ids]);
        }

        for (int sellId = sellIds.Count-1; sellId >= 0 ; sellId--)
        {
            int removeids = sellIds[sellId];
            GameManager.Instance.RemoveCreateUnit(removeids);
        }

        species = null;
        sellIds = new List<int>();
    }
}
