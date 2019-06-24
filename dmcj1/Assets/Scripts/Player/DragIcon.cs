using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //目前正在拖动的UI
    private GameObject currentUI;

    private void Start()
    {
        currentUI = null;
    }

    /// <summary>
    /// 开始拖动时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //限制可拖动的对象
        if (eventData.pointerCurrentRaycast.gameObject.tag == "NormalItems"||
            eventData.pointerCurrentRaycast.gameObject.tag == "HelmetImage" ||
            eventData.pointerCurrentRaycast.gameObject.tag == "BackpageImage" ||
            eventData.pointerCurrentRaycast.gameObject.tag == "WeasponImage")
        {
            currentUI = eventData.pointerCurrentRaycast.gameObject;
        }
        else
        {
            currentUI = null;
        }
    }

    /// <summary>
    /// 拖动中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (currentUI != null)
        {
            currentUI.transform.position = Input.mousePosition;
        }
    }
    /// <summary>
    /// 结束拖动时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentUI.tag == "NormalItems")
        {
            if (eventData.pointerCurrentRaycast.gameObject.tag == "StockUI")
            {
                currentUI.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            }
        }
        else if (currentUI.tag == "HelmetImage")
        {
            if (eventData.pointerCurrentRaycast.gameObject.tag == "HelmetUI")
            {
                currentUI.gameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                currentUI.gameObject.transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            }
        }
        else if (currentUI.tag == "BackpageImage")
        {
            if (eventData.pointerCurrentRaycast.gameObject.tag == "BackpageUI")
            {
                currentUI.gameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                currentUI.gameObject.transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            }
        }
        else if (currentUI.tag == "WeasponImage")
        {
            if (eventData.pointerCurrentRaycast.gameObject.tag == "WeasponBar" && eventData.pointerCurrentRaycast.gameObject.transform.childCount <= 0)
            {
                currentUI.transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                currentUI.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            }
        }

    }
}
