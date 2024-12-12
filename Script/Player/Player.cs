using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public UIPlayerSpec uiPlayerSpec;

    private void Awake()
    {
        GameManager.Instance.Player = this;
        uiPlayerSpec = GetComponent<UIPlayerSpec>();
    }
}