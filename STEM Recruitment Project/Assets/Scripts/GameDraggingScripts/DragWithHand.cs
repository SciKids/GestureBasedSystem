using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class DragWithHand : MonoBehaviour
{

    bool active = false;
    bool press = false;
    //bool dragging = false;

    public const string DRAGGABLE_TAG = "UIDraggable";
    private bool dragging = false;
    private Vector2 originalPosition;
    private Transform objectToDrag;
    private Image objectToDragImage;

    List<RaycastResult> hitObjects = new List<RaycastResult>();


    private void Start()
    {
        Application.runInBackground = true;
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        //Cursor.visible = false;
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
    }

    bool pressed = false;

    private void NuitrackManager_onHandsTrackerUpdate(nuitrack.HandTrackerData handTrackerData)
    {
        active = false;
        press = false;

        ItemDragHandler drag = GetComponent<ItemDragHandler>();

        if (handTrackerData != null)
        {
            nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

            if (userHands != null)
            {
                if (userHands.RightHand != null)
                {
                    Vector2 curpos = new Vector2(userHands.RightHand.Value.X * Screen.currentResolution.width, userHands.RightHand.Value.Y * Screen.currentResolution.height);
                    MouseOperations.SetCursorPosition((int)(curpos.x), (int)(curpos.y));
                    active = true;
                    press = userHands.RightHand.Value.Pressure == 1.0f;

                    if (press)
                    {
                        objectToDrag = drag.GetDraggableTransformUnderMouse();

                        drag.dragObj(objectToDrag);
                        dragging = true;
                    }
                    if(dragging)
                    {
                        drag.dragObj(objectToDrag);
                    }
                    else
                    {
                        drag.endDrag(objectToDrag);
                        dragging = false;
                    }
                }
            }
        }
    }
}