using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quick : MonoBehaviour
{
    [SerializeField] private GameObject quickUI;
    [SerializeField] private TextMeshProUGUI quickTxt;
    private bool isQuick = false;
    private Coroutine quickUICoroutine;
    private string quick;

    public void QuickGame()
    {
        if (isQuick)
        {
            isQuick = false;
            quick = "1 배속";
            Time.timeScale = 1.0f;
        }
        else
        {
            isQuick = true;
            quick = "2 배속";
            Time.timeScale = 2.0f;
        }
        quickTxt.text = quick;
        SetQuickUI();
    }

    public void SetQuickUI()
    {
        quickUI.SetActive(true);
        quickUICoroutine = StartCoroutine(PopUpQuickUI());
    }

    IEnumerator PopUpQuickUI()
    {
        yield return new WaitForSeconds(0.5f);
        quickUI.SetActive(false);
        StopCoroutine(quickUICoroutine);
    }

    public void ResetQuick()
    {
        isQuick = false;
        quick = "1 배속";
        quickTxt.text = quick;
    }
}
