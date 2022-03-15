using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class CollisionLogger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerMoveHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
        //Debug.Log(eventData.pointerEnter.GetComponentInParent<HierarchyObject>().transform.name);
        //if (eventData.pointerEnter.GetComponentInParent<HierarchyObject>() is HierarchyObject draggedObject)
        //{
        //    if (draggedObject != GetComponentInParent<HierarchyObject>())
        //    {
        //        draggedObject.HighlightObject(true);
        //    }
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }
}
