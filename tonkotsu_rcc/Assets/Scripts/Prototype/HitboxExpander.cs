using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxExpander : MonoBehaviour
{
    [SerializeField]
    Vector3 targetFinalScale;

    [SerializeField]
    float speed;


    void Update()
    {
        if(gameObject.transform.localScale.x < targetFinalScale.x)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, targetFinalScale, speed);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
