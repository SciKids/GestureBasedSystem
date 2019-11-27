using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveWithHands : MonoBehaviour
{
    bool active = false;

    [Header("Raycasting")]

    [SerializeField]
    Camera cam;
    
    ImageItem selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    // My stuff
    Vector3 rightHandPos, leftHandPos;
    public GameObject leftHandPic, rightHandPic;
    nuitrack.HandTrackerData handTrackerDataTest;
    public int ZPosition;

    [SerializeField]
    float dragSensitivity = 5f;

    private void Start()
    {
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
        Cursor.visible = true;
    }

    private void Update()
    {
       // nuitrack.UserHands userHands = handTrackerDataTest.GetUserHandsByID(CurrentUserTracker.CurrentUser);

       // Vector3 rightHandPos = new Vector3(userHands.RightHand.Value.X * Screen.width, -userHands.RightHand.Value.Y * Screen.height, userHands.RightHand.Value.ZReal);
       // Vector3 leftHandPos = new Vector3(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height, userHands.LeftHand.Value.ZReal);

       // Debug.Log("Right: " + rightHandPos + ", Left: " + leftHandPos);
    }

    private void NuitrackManager_onHandsTrackerUpdate(nuitrack.HandTrackerData handTrackerData)
    {
        active = false;

        if (handTrackerData != null)
        {
            nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

            if (userHands != null)
            {
                // Right hand
                if (userHands.RightHand != null)
                {
                    //baseRect.anchoredPosition = new Vector2(userHands.RightHand.Value.X * Screen.width, -userHands.RightHand.Value.Y * Screen.height);
                    rightHandPos = new Vector3(userHands.RightHand.Value.X * Screen.width, -userHands.RightHand.Value.Y * Screen.height, ZPosition);

                    //Debug.Log("Right: " + rightHandPos);
                    
                    rightHandPic.transform.position = rightHandPos;

                    active = true;
                }
                // Left hand
                if (userHands.LeftHand != null)
                {
                    //baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    leftHandPos = new Vector3(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height, ZPosition);

                    leftHandPic.transform.position = leftHandPos;

                    //Debug.Log("Left: " + leftHandPos);

                    active = true;
                }
            }
        }
        

        
    }
}
