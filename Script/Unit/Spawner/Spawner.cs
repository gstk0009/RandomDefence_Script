using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> unitPrefabs; // 유닛의 프리팹 리스트
    public int createUnitId = 0;
    public Transform GetUnitTxtParentObject;
    public GameObject GetUnitTxt;

    public void SpawnRandomUnit(Vector3 position, MapArea area)
    {
        GameObject selectedPrefab = unitPrefabs[Random.Range(0, unitPrefabs.Count)];
        GameObject newUnitObj = Instantiate(selectedPrefab, position, Quaternion.identity, area.transform); // 회전 X
        Unit newUnit = newUnitObj.GetComponent<Unit>();
        newUnit.CreateUnitId = createUnitId;
        
        if (newUnit != null && newUnit.unitData != null)
        {
            newUnit.Initialize(newUnit.unitData); // 프리팹에 할당된 UnitSO를 사용하여 초기화
            GetUnitTxt.GetComponent<TextMeshProUGUI>().text = $"{newUnit.rank}등급의 {newUnit.unitData.race} 소환";
            Instantiate(GetUnitTxt,GetUnitTxtParentObject);
            
        }
        else
        {
            Debug.LogError("Prefab does not have Unit component or UnitSO is not assigned!");
        }

        GameManager.Instance.AddCrateUnit(newUnitObj, createUnitId, (int)newUnit.rank, newUnit.GetGoldValue());
        GameManager.Instance.Player.uiPlayerSpec.playerGold -= 10;
        GameManager.Instance.Player.uiPlayerSpec.UImanager.SetGoldUI(GameManager.Instance.Player.uiPlayerSpec.playerGold);

        createUnitId += 1;
    }

    public void ResetText()
    {
        for (var i = GetUnitTxtParentObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(GetUnitTxtParentObject.transform.GetChild(i).gameObject);
        }
    }

}
