using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCameraFollow : MonoBehaviour
{
    public Camera cameraToFollow;

    Vector3 cameraDir;

    void Update()
    {
        if (cameraToFollow != null)
        {
            cameraDir = cameraToFollow.transform.forward;
            cameraDir.y = 0;

            transform.rotation = Quaternion.LookRotation(cameraDir);
        }
    }
}