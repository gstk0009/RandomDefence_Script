using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    public List<MapArea> areas = new List<MapArea>();
    public Spawner spawner;

    private MapArea draggingArea;
    private GameObject draggingUnitObject; // 드래그 중인 unitObject를 저장할 변수

    [SerializeField] private GameObject noSeatTxtUI;
    private Coroutine noemty;

    public void PlaceUnit() // 유닛 뽑을 때 빈 자리에 배치시키기
    {
        foreach (var area in areas)
        {
            if (!area.hasUnit)
            {
                if(GameManager.Instance.Player.uiPlayerSpec.playerGold >= 10)
                {
                    spawner.SpawnRandomUnit(area.transform.position - new Vector3(-0.05f, 0.3f, 0), area); // 해당 위치에 유닛 생성

                    area.hasUnit = true;
                }
                else
                {
                    GameManager.Instance.Player.uiPlayerSpec.UImanager.SetNoGoldUI();
                    Debug.Log("Gold가 부족합니다.");
                }

                return; // 유닛 1개 생성 후 메서드 종료
            }
        }
        // 배치할 자리가 없다는 UI 띄우기
        SetNoemptySeatUI();
    }

    private void SetNoemptySeatUI()
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

    public void StartDragging(MapArea area, GameObject unitObject)
    {
        draggingArea = area;
        draggingUnitObject = unitObject; // 드래그 중인 unitObject 설정
      
    }

    public void EndDragging(MapArea targetArea)
    {
   

        if (draggingArea != null && targetArea != draggingArea) // draggingArea와 area가 다를 경우에만 실행
        {
      
            if (targetArea.transform.childCount == 0)
            {
     
                MoveChild(draggingArea, targetArea);
            }
            else
            {
             
                SwapChildren(draggingArea, targetArea); // 두 area에 자식이 있는 경우, 자식을 교환
            }
        }

        // 드래그 상태 초기화
        draggingArea = null;
        draggingUnitObject = null;
    }

    public void CancelDragging()
    {
        // 드래그 취소 시 처리할 로직 추가 가능
     
        draggingArea = null;
        draggingUnitObject = null;
    }

    private void MoveChild(MapArea fromArea, MapArea toArea)
    {
        if (fromArea.transform.childCount == 0) // fromArea에 자식 오브젝트가 없는 경우 함수 종료
        {
          
            return;
        }


        // fromArea의 첫 번째 자식을 가져옴
        Transform child = fromArea.transform.GetChild(0);

        // DOTween을 사용하여 이동 애니메이션 실행
        Vector3 targetWorldPosition = toArea.transform.position - new Vector3(-0.05f, 0.3f, 0);
        child.DOMove(targetWorldPosition, 0.5f).OnComplete(() =>
        {
            // 애니메이션이 완료된 후에 부모를 변경
            child.SetParent(toArea.transform, false);
            child.localPosition = new Vector3(0.05f, -0.3f, 0); // 부모의 위치에 맞추기 위해 로컬 포지션 설정

            // fromArea와 toArea의 hasUnit 상태 업데이트
            toArea.hasUnit = true;
            fromArea.hasUnit = false;

            // toArea와 fromArea의 unitObject 상태 업데이트
            toArea.unitObject = child.gameObject;
            fromArea.unitObject = null;

        });
    }

    // 두 MapArea의 자식 오브젝트를 교환하는 메서드
    private void SwapChildren(MapArea area1, MapArea area2)
    {
        if (area1.transform.childCount == 0 || area2.transform.childCount == 0) // area1과 area2 모두 자식이 없는 경우 함수 종료
        {
         
            return;
        }

       

        // 각 MapArea의 첫 번째 자식을 가져와 위치 교환 애니메이션 시작
        Transform child1 = area1.transform.GetChild(0);
        Transform child2 = area2.transform.GetChild(0);

        // DOTween을 사용하여 위치 교환 애니메이션 실행
        Vector3 child1TargetPosition = area2.transform.position - new Vector3(-0.05f, 0.3f, 0);
        Vector3 child2TargetPosition = area1.transform.position - new Vector3(-0.05f, 0.3f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(child1.DOMove(child1TargetPosition, 0.5f));
        sequence.Join(child2.DOMove(child2TargetPosition, 0.5f));

        sequence.OnComplete(() =>
        {
            // 애니메이션이 완료된 후 부모를 교환
            child1.SetParent(area2.transform, false);
            child2.SetParent(area1.transform, false);

            // 자식의 로컬 포지션을 0으로 설정하여 부모의 위치에 맞춤
            child1.localPosition = new Vector3(0.05f, -0.3f, 0);
            child2.localPosition = new Vector3(0.05f, -0.3f, 0);

            // area1과 area2의 unitObject 상태 업데이트
            area1.unitObject = child2.gameObject;
            area2.unitObject = child1.gameObject;

            // hasUnit 상태 업데이트
            area1.hasUnit = child2 != null;
            area2.hasUnit = child1 != null;

         
        });
    }
}