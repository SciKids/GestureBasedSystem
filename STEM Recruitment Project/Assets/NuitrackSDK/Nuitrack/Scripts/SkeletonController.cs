using nuitrack;
using UnityEngine;
using System.Collections.Generic;

public class SkeletonController : MonoBehaviour {

    [Range(0, 6)]
    public int skeletonCount = 6;         //maximum skeletons 6
    [SerializeField] SimpleSkeletonAvatar skeletonAvatar;

    List<SimpleSkeletonAvatar> avatars = new List<SimpleSkeletonAvatar>();

    SkeletonData skeletonData;

    void OnEnable()
    {
        NuitrackManager.SkeletonTracker.OnSkeletonUpdateEvent += OnSkelUpdate;
    }

    void Start()
    {
        for (int i = 0; i < skeletonCount; i++)
        {
            GameObject newAvatar = Instantiate(skeletonAvatar.gameObject);
            newAvatar.transform.parent = transform;
            SimpleSkeletonAvatar simpleSkeleton = newAvatar.GetComponent<SimpleSkeletonAvatar>();
            simpleSkeleton.autoProcessing = false;
            avatars.Add(simpleSkeleton);
        }

        NuitrackManager.SkeletonTracker.SetNumActiveUsers(skeletonCount);
    }

    void OnSkelUpdate(SkeletonData skelData)
    {
        skeletonData = skelData;
    }

    void Update()
    {
        for (int i = 0; i < avatars.Count; i++)
        {
            if (skeletonData != null && i < skeletonData.Skeletons.Length && skeletonData.Skeletons.Length != 0)
            {
                avatars[i].gameObject.SetActive(true);
                avatars[i].ProcessSkeleton(skeletonData.Skeletons[i]);
            }
            else
            {
                avatars[i].gameObject.SetActive(false);
            }
        }
    }
}
