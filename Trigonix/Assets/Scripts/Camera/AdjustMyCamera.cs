using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMyCamera : MonoBehaviour
{
    public Camera _myCam;
    

    // Start is called before the first frame update
    void Start()
    {
        _myCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustCamera(Vector3 centerOfCamera)
    {
        transform.position = centerOfCamera;
    }
}
