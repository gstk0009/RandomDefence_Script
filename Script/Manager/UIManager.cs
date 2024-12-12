using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // GameManager or Stage �����ϴ� ������ �ð� �޾ƿ���
    public float StageLimitTime = 10;
    public TextMeshProUGUI Stagetxt;
    public TextMeshProUGUI Goldtxt;
    public TextMeshProUGUI LimitTimeTxt;
    public TextMeshProUGUI MonsterCountTxt;
    public GameObject EndingUI;
    public GameObject ClearUI;
    public bool isEndingStage = false;
    [SerializeField] private GameObject noGoldTxtUI;

    private Coroutine nogoldCoroutine;
    private Coroutine countCoroutine;
    private int min;
    private float sec;
    private float countTime = 1f;
    public UnitSO[] UnitDataArray;

    private bool isGameClear;

    private void Start()
    {
        GameManager.Instance.Player.uiPlayerSpec.UImanager = this;
        isGameClear = false;
        UpdateLimitTime();
        countCoroutine = StartCoroutine(TimeCount(countTime));
    }

    IEnumerator TimeCount(float time)
    {
        while (!isGameClear)
        {
            StageLimitTime -= time;
            yield return new WaitForSeconds(time);

            UpdateLimitTime();
        }
    }

    private void UpdateLimitTime()
    {
        if (StageLimitTime > 0f)
        {
            int totalSeconds = (int)StageLimitTime;
            min = totalSeconds / 60;
            sec = totalSeconds % 60;

            LimitTimeTxt.text = min.ToString("00") + ":" + sec.ToString("00");
        }

        if(StageLimitTime <= 1 && !GameManager.Instance.isBossCleared())
        {
            PopUpEndingUI();
        }

        if (isEndingStage)
        {
            if (StageLimitTime > 1 && GameManager.Instance.Player.uiPlayerSpec.LeftMonsters == 0)
                PopUpClearUI();
            else if(StageLimitTime == 1 && GameManager.Instance.Player.uiPlayerSpec.LeftMonsters != 0)
                PopUpEndingUI();
        }
    }

    public void PopUpClearUI()
    {
        StopCoroutine(countCoroutine);
        Time.timeScale = 0f;
        ClearUI.SetActive(true);
    }

    public void PopUpEndingUI()
    {
        StopCoroutine(countCoroutine);
        Time.timeScale = 0f;
        EndingUI.SetActive(true);
    }

    // UIs - Ending - ExitBtn
    public void LoadLobbyScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // UIs - Ending - RetryBtn
    public void ResetUI()
    {
        StopCoroutine(countCoroutine);

        StageLimitTime = 10f;
        Time.timeScale = 1f;
        isEndingStage = false;
        UpdateLimitTime();

        ResetAllUnitEnhanceCount();
        countCoroutine = StartCoroutine(TimeCount(countTime));
    }

    public void SetGoldUI(int gold)
    {
        Goldtxt.text = gold.ToString();
    }

    public void SetStageUI(int stage)
    {
        if (stage == 50)
        {
            isEndingStage = true;
        }
        Stagetxt.text = stage.ToString();
    }

    public void SetMonsterCountUI(int monsterCount)
    {
        MonsterCountTxt.text = monsterCount.ToString();
    }

    // UI 중 게임 진행을 멈춰야 할 때 사용하는 함수
    // Btn에 연결
    public void UIOpend()
    {
        Time.timeScale = 0f;
    }

    public void UIClosed()
    {
        Time.timeScale = 1f;
    }

    private void ResetAllUnitEnhanceCount()
    {
        foreach (UnitSO unit in UnitDataArray)
        {
            if (unit != null)
            {
                unit.enhanceCnt = 0;
            }
        }
    }

    public void SetNoGoldUI()
    {
        noGoldTxtUI.SetActive(true);
        nogoldCoroutine = StartCoroutine(NoGoldUIPopUp());
    }

    IEnumerator NoGoldUIPopUp()
    {
        yield return new WaitForSeconds(0.5f);
        noGoldTxtUI.SetActive(false);
        StopCoroutine(nogoldCoroutine);
    }
}
