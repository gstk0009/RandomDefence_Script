using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool hasUnit; // true = 배치한 유닛 있음, false = 유닛 없음
    public GameObject unitObject; // 유닛 오브젝트를 가지고 있어야 함
    public MapManager mapManager;

    private void Update()
    {
        GetUnitObject();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mapManager.StartDragging(this, unitObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중에는 transform.position을 변경하지 않습니다.
        // 드래그 시 시각적 피드백을 추가할 수 있습니다.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 현재 마우스 위치에서 타겟 MapArea를 찾아서 설정
        MapArea targetArea = GetMapAreaUnderPointer(eventData);
        if (targetArea != null)
        {
            mapManager.EndDragging(targetArea);
        }
        else
        {
            mapManager.CancelDragging();
        }
    }

    private MapArea GetMapAreaUnderPointer(PointerEventData eventData)
    {
        // 현재 마우스 위치에서 타겟 MapArea를 찾는 로직
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = eventData.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            MapArea area = result.gameObject.GetComponent<MapArea>();
            if (area != null)
            {
                return area;
            }
        }

        return null;
    }

    public void GetUnitObject()
    {
        if (hasUnit) // 배치한 유닛이 있으면 유닛 오브젝트 가져오기
        {
            Transform child = transform.GetChild(0);
            unitObject = child.gameObject;
        }
    }
}
