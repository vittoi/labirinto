using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class seguraItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler, IBeginDragHandler
{
    private Transform draggedItemBox;
    private CanvasGroup canvasGroup;
    private GameObject oldSlot;
    private int oldSlotIndex;
    private inventario inv;
    private RectTransform rectTransform;
    private Vector2 pointerOffset;

    void Start(){
        draggedItemBox = GameObject.FindGameObjectWithTag("DraggingItem").transform;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inv = GameObject.Find("menuCanvas").GetComponent<inventario>();
    }
    public void OnBeginDrag(PointerEventData data){
        if (data.button == PointerEventData.InputButton.Left)
        {
            
            GameObject btn = Instantiate(inv.getEmptyBt());
            oldSlot = btn.transform.gameObject;
            inv.setBtnOnIndex(btn, inv.getIndexByGameObject(this.gameObject.transform));
            btn.name = this.transform.name;
            btn.transform.SetParent(this.transform.parent);
            btn.transform.SetSiblingIndex(oldSlotIndex);
            this.gameObject.name += "D"; 
            btn.transform.localScale = new Vector3(1,1,1);
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
         if (data.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = true;
            Transform newSlot = null;
            if (data.pointerEnter != null)
                newSlot = data.pointerEnter.transform;

            if (newSlot != null){
                inv.trocaItens(this.transform.GetChild(1).gameObject, newSlot.transform.gameObject, oldSlot);
                
            }else{
                inv.esvaziaSlot(oldSlot);
            }
            Destroy(this.gameObject);
        }

    }

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out pointerOffset);
            oldSlotIndex = this.transform.GetSiblingIndex();
            
        }
    }

    public void OnDrag(PointerEventData data)
    {

        RectTransform rectTransformSlot = GameObject.FindGameObjectWithTag("DraggingItem").GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        if (data.button == PointerEventData.InputButton.Left)
        {
            

            rectTransform.SetAsLastSibling();
            transform.SetParent(draggedItemBox);
            Vector2 localPointerPosition;
            canvasGroup.blocksRaycasts = false;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformSlot, Input.mousePosition, data.pressEventCamera, out localPointerPosition))
            {
                rectTransform.localPosition = localPointerPosition - pointerOffset;
            }
        }
    }

}
