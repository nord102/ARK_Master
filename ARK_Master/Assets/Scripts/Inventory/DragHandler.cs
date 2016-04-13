using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        itemBeingDragged.transform.SetParent(StateMachine.instance.GoInventory.transform);

        GetComponent<RectTransform>().localScale = new Vector3(.9f, .9f, 1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //if (transform.parent == startParent)
        //{
        //    transform.position = startPosition;
        //    transform.SetParent(startParent);
        //}
        //Reset if it wasn't dropped on a slot
        if (transform.parent.tag != "InventorySlot")
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        //Set active/inactive. Makes sense to do it when dropped, but the ifs are already here and the parent's set
        Skills mySkill = GetComponent<Data>().MySkill;
        if (transform.parent.transform.parent.tag == "Equipped")
        {
            transform.localScale = new Vector3(.7f, .7f, 1);
            StateMachine.instance.SetSkillActive((SkillType)mySkill.skillID, true);
        }
        else
        {
            StateMachine.instance.SetSkillActive((SkillType)mySkill.skillID, false);
        }

        itemBeingDragged = null;
    }
}