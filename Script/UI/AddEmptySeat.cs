using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEmptySeat : MonoBehaviour
{
    [SerializeField] private GameObject noSeatTxtUI;
    private Coroutine noemty;

    public void SetNoemptySeatUI()
    {
        noSeatTxtUI.SetActive(true);
        noemty = StartCoroutine(NoEmpty());
    }

    IEnumerator NoEmpty()
    {
        yield return new WaitForSeconds(0.5f);
        noSeatTxtUI.SetActive(false);
        StopCoroutine(noemty);
    }
}
