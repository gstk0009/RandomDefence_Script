using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private Player _player;
    public Player Player { get { return _player; } set { _player = value; } }
    public ObjectPool ObjectPool { get; private set; }

    public List<GameObject> CreateUnit = new List<GameObject>();
    public List<int> CreateUnitId = new List<int>();
    public List<int> CreateUnitRank = new List<int>();
    public List<int> CreateUnitGold = new List<int>();

    public UnitSO[] unitData;

    public bool IsBoss = false;
    public bool IsBossClear = false;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            ObjectPool = GetComponent<ObjectPool>();
        }
        else
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    public void AddCrateUnit(GameObject obj, int id, int rank, int gold)
    {
        CreateUnit.Add(obj);
        CreateUnitId.Add(id);
        CreateUnitRank.Add(rank);
        CreateUnitGold.Add(gold);
    }

    public void RemoveCreateUnit(int id)
    {
        CreateUnit.RemoveAt(id);
        CreateUnitId.RemoveAt(id);
        CreateUnitRank.RemoveAt(id);
        CreateUnitGold.RemoveAt(id);
    }

    // RetryBtn ¿¬°á
    public void ResetGame()
    {
        IsBoss = false;
        IsBossClear = false;

        if (CreateUnit.Count > 0)
        {
            for (int ids = 0; ids < CreateUnit.Count; ids++)
            {
                GameObject obj = CreateUnit[ids];

                MapArea unitMapArea = obj.GetComponentInParent<MapArea>();
                if (unitMapArea != null)
                {
                    unitMapArea.hasUnit = false;
                    unitMapArea.unitObject = null;
                }
                
                if (obj == null) return;
                Destroy(obj);
            }
        }
        ResetData();
        CreateUnit = new List<GameObject>();
        CreateUnitGold = new List<int>();
        CreateUnitId = new List<int>();
        CreateUnitRank = new List<int>();
    }
    void OnApplicationQuit()
    {
        ResetData();
    }

    public void ResetData()
    {
        foreach (UnitSO unit in unitData)
        {
            if (unit != null)
            {
                unit.enhanceCnt = 0;
                unit.attackSpeed = 15;
                unit.attackRange = 5;
            }
        }
    }

    public bool isBossCleared()
    {
        if (IsBoss && !IsBossClear)
            return false;
        else
        {
            IsBossClear = false;
            return true;
        }
    }
}
