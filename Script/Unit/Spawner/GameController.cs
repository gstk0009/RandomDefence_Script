using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public MapManager mapManager;
    public Spawner Spawner;
    public LayerMask targetLayer;
    public UnitUI unitUI;

    private void Awake()
    {
        unitUI = unitUI.GetComponent<UnitUI>();
    }

    void Update()
    {
        // `E` 키 입력 감지
        if (Input.GetKeyDown(KeyCode.E))
        {
            mapManager.PlaceUnit();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, targetLayer);
            Debug.Log("클릭");
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                Unit unitComponent = clickedObject.GetComponent<Unit>();
                float attackSpeed = unitComponent.GetAttackSpeedBasedOnRank();
                int attackSpeedInt = Mathf.RoundToInt(attackSpeed);
                if (unitComponent != null)
                {
                    
                    unitUI.UpdateUnitStatus(
                        unitComponent.rank.ToString(),
                        unitComponent.unitData.race.ToString(),
                        unitComponent.unitData.enhanceCnt,
                        unitComponent.currentAttack,
                        attackSpeedInt,
                        unitComponent.GetGoldValue(),
                        unitComponent.gameObject);

                }

            }
        }
    }
}
