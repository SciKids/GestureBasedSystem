using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Drags image under mouse
public class ItemDragHandler : MonoBehaviour
{
    public const string DRAGGABLE_TAG = "UIDraggable";

    private bool dragging = false;

    private Vector2 originalPosition;

    private Transform objectToDrag;

    private Image objectToDragImage;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    // NUITRACK STUFF
    bool active = false;
    bool press = false;
    bool pressed = false;
    
    void Start()
    {
        Application.runInBackground = true;
    }
 
    void Update()
    {
        active = false;
        press = false;
        
        // Begin to drag object
        if (Input.GetMouseButtonDown(0) && dragging == false)
        {
            objectToDrag = GetDraggableTransformUnderMouse();

            beginDrag(objectToDrag);
            
        }
        // Drag object
        if (dragging)
        {
            objectToDrag.position = Input.mousePosition;
        }

        // Drop object
        if (Input.GetMouseButtonUp(0) && dragging == true)
        {
            endDrag(objectToDrag);
        }
       
    } // end Update()

    public void beginDrag(Transform objectToDrag)
    {
        if (objectToDrag != null)
        {
            dragging = true;

            objectToDrag.SetAsLastSibling();

            originalPosition = objectToDrag.position;

            objectToDragImage = objectToDrag.GetComponent<Image>();

            objectToDragImage.raycastTarget = false;
        }
    }

    public void dragObj(Transform objectToDrag)
    {
        objectToDrag.position = Input.mousePosition;
    }

    public void endDrag(Transform objectToDrag)
    {
        objectToDrag.position = Input.mousePosition;

        objectToDragImage.raycastTarget = true;

        dragging = false;
    }

    public GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        return hitObjects.First().gameObject;


    }

    public Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        {
            return clickedObject.transform;
        }

        return null;
}
    


} // End ItemDragHandler class
/*
if (handTrackerData != null)
        {
            nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

            if(userHands != null)
            {
                if(userHands.RightHand != null)
                {
                    Vector2 curpos = new Vector2(userHands.RightHand.Value.X * Screen.currentResolution.width, userHands.RightHand.Value.Y * Screen.currentResolution.height);
MouseOperations.SetCursorPosition((int)(curpos.x), (int) (curpos.y));
                    active = true;
                    press = userHands.RightHand.Value.Click;

                    if(pressed != press)
                    {
                        pressed = press;

                        if(pressed)

    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                        {*/
