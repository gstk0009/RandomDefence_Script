using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static EventHandler _instance;

    public int elfEnhanceCnt { get; private set; }
    public int orcEnhanceCnt { get; private set; }
    public int humanEnhanceCnt { get; private set; }

    // �̺�Ʈ ����
    public event Action OnUpgradeElfUnits;
    public event Action OnUpgradeOrcUnits;
    public event Action OnUpgradeHumanUnits;
    public event Action OnUIUpdate;



    // �ν��Ͻ� �Ӽ�
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
        // �̱��� ó��
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

    // ���� ���� ��ȭ �̺�Ʈ ȣ��
    public void TriggerUpgradeElfUnitsEvent()
    {
        elfEnhanceCnt++;
        OnUpgradeElfUnits?.Invoke();
        TriggerUIUpdate();
    }

    // ��ũ ���� ��ȭ �̺�Ʈ ȣ��
    public void TriggerUpgradeOrcUnitsEvent()
    {
        orcEnhanceCnt++;
        OnUpgradeOrcUnits?.Invoke();
        TriggerUIUpdate();
    }

    // �ΰ� ���� ��ȭ �̺�Ʈ ȣ��
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

    // ��ü�� �ı��� �� ȣ��Ǵ� �޼���
    private void OnDestroy()
    {
        // �̺�Ʈ ������ ����
        OnUpgradeElfUnits = null;
        OnUpgradeOrcUnits = null;
        OnUpgradeHumanUnits = null;

        // �̱��� �ν��Ͻ� ����
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
