using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform mainCameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, 
            mainCameraTransform.rotation * Vector3.up);
    }
}
