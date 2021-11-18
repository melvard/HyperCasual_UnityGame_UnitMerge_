//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class DragDrop : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
//{
//    private Transform unitTansform;
//    private Vector3 beginDragUnitPosition;
//    private Vector3 onDragPointerWorldSpacePosition;

//    private void Start()
//    {
//        unitTansform = gameObject.GetComponent<Transform>();
//    }
//    public void OnPointerDown(PointerEventData eventData)
//    {
//        Debug.Log("Pointer is down");
//        GetComponent<Unit>().DecreaseSpriteTransparency();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        Debug.Log("Begining dragging");
//        GetComponent<BoxCollider>().enabled = false;
//        beginDragUnitPosition = unitTansform.position;
//        onDragPointerWorldSpacePosition = Camera.allCameras[0].ScreenToWorldPoint(eventData.pressPosition);
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        Debug.Log("On drag");
//        Vector3 pointerCurrentWorldSpace = Camera.allCameras[0].ScreenToWorldPoint(eventData.position);
//        Vector3 pointerDeltaWorldSpace = pointerCurrentWorldSpace - onDragPointerWorldSpacePosition;
//        unitTansform.position += pointerDeltaWorldSpace;
//        onDragPointerWorldSpacePosition = pointerCurrentWorldSpace;

//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        Debug.Log("Ending dragging");
//        GetComponent<BoxCollider>().enabled = true;
//        if(eventData.pointerEnter == null)
//        {
//            ResetBegginPosition();
//        }
        
//    }


//    public void ResetBegginPosition()
//    {
//        transform.position = new Vector3(beginDragUnitPosition.x, beginDragUnitPosition.y, transform.position.z);
//    }
//    public void OnPointerUp(PointerEventData eventData)
//    {
//        Debug.Log("Pointer is up");
//        GetComponent<Unit>().IncreaseSpriteTransparency();
//    }
//}
