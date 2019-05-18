using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter*/
{
    private bool openBackpage = true;

    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (openBackpage == true)
            {
                this.GetComponent<UIManager>().itemCanvas.gameObject.SetActive(true);
                this.GetComponent<UIManager>().miniMap.SetActive(false);
                openBackpage = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                this.GetComponent<UIManager>().itemCanvas.gameObject.SetActive(false);
                this.GetComponent<UIManager>().miniMap.SetActive(true);
                openBackpage = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;             
            }
        }
    }
}
