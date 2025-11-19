using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    EventSystem eventSystem;
    private bool initialized = false;

    protected virtual void Start()
    {
        raycaster = UIManager.Instance.GraphicRaycaster;
        eventSystem = EventSystem.current;
    }

    protected virtual void OnDestroy()
    {
        UIManager.Instance.RemoveUIInList(GetType().Name);
    }

    protected void CheckOuterClickAndDestroy(GameObject obj)
    {
        if (!initialized)
        {
            initialized = true;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭된 UI 요소 체크
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            // 클릭된 UI가 이 패널을 포함하지 않으면 닫기
            bool clickedOnPanel = false;
            foreach (var res in results)
            {
                if (res.gameObject == obj || res.gameObject.transform.IsChildOf(obj.transform))
                {
                    clickedOnPanel = true;
                    break;
                }
            }

            if (!clickedOnPanel)
            {
                Destroy(gameObject);
            }
        }
    }
}
