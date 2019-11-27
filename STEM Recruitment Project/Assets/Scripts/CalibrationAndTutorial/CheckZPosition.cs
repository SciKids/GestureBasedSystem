using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckZPosition : MonoBehaviour
{
    public nuitrack.JointType typeJoint;
    private nuitrack.Joint joint;
    private string message = "";
    
    // Update is called once per frame
    void Update()
    {
         if(CurrentUserTracker.CurrentUser != 0)
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            joint = skeleton.GetJoint(typeJoint);
            Vector3 position = 0.001f * joint.ToVector3();

            if (joint.ToVector3().z < 800)
            {
                message = "Please step away from camera";
            }
            else if (joint.ToVector3().z > 1500)
            {
                message = "Please step towards the camera";
            }
            else
            {
                message = "";
            }
        }
         else
        {
            message = "User not found";
        }
    }

    // Display the message on the screen
    void OnGUI()
    {
        GUI.color = Color.red;
        GUI.skin.label.fontSize = 50;
        GUILayout.Label(message);
    }
}
