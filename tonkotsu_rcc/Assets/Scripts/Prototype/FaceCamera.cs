using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        var p = PlayerHandler.Player.GetComponent<PlayerController>();
        if(p != null)
        {
            var cam = p.Camera;

            if(cam != null)
            {
                transform.LookAt(cam.transform);
            }
        }

    }
}
