using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCameraController : MonoBehaviour
{
    public GameObject chickenObject;
    private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        // position of the camera and the position of the player distance
        offset = this.transform.position - chickenObject.transform.position;

            
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // move to where the chicken is but add the offset
        transform.position = chickenObject.transform.position + offset;
    }
}
