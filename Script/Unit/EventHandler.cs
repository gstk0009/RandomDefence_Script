using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static EventHandler _instance;

    public int elfEnhanceCnt { get; private set; }
    public int orcEnhanceCnt { get; private set; }
    public int humanEnhanceCnt { get; private set; }

    // 이벤트 정의
    public event Action OnUpgradeElfUnits;
    public event Action OnUpgradeOrcUnits;
    public event Action OnUpgradeHumanUnits;
    public event Action OnUIUpdate;



    // 인스턴스 속성
    public static EventHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventHandler>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("EventHandler");
                    _instance = obj.AddComponent<EventHandler>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 싱글톤 처리
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 엘프 유닛 강화 이벤트 호출
    public void TriggerUpgradeElfUnitsEvent()
    {
        elfEnhanceCnt++;
        OnUpgradeElfUnits?.Invoke();
        TriggerUIUpdate();
    }

    // 오크 유닛 강화 이벤트 호출
    public void TriggerUpgradeOrcUnitsEvent()
    {
        orcEnhanceCnt++;
        OnUpgradeOrcUnits?.Invoke();
        TriggerUIUpdate();
    }

    // 인간 유닛 강화 이벤트 호출
    public void TriggerUpgradeHumanUnitsEvent()
    {
        humanEnhanceCnt++;
        OnUpgradeHumanUnits?.Invoke();
        TriggerUIUpdate();
    }
    private void TriggerUIUpdate()
    {
        OnUIUpdate?.Invoke();
    }

    // 객체가 파괴될 때 호출되는 메서드
    private void OnDestroy()
    {
        // 이벤트 리스너 해제
        OnUpgradeElfUnits = null;
        OnUpgradeOrcUnits = null;
        OnUpgradeHumanUnits = null;

        // 싱글톤 인스턴스 해제
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
