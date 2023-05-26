using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMyCamera : MonoBehaviour
{
    private Camera _myCam;
    //private AdjustMyCamera _adjustMyCamera;


    // Start is called before the first frame update
    void Start()
    {
        _myCam = GetComponent<Camera>();
        //_adjustMyCamera = GetComponent()
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void AdjustCamera(Vector3 centerOfCamera)
    //{
    //    transform.position = centerOfCamera;
    //}
    public void AdjustCamera(int linesOfTriangle, float sizeElementY)
    {
        linesOfTriangle++; //starts from 0
        //_adjustMyCamera.AdjustCamera();
        if (linesOfTriangle > 9)
        {
            _myCam.orthographicSize = Mathf.Floor((float)((float)(linesOfTriangle) / 5f)) * 3 + 2;
        }
        else
        {
            _myCam.orthographicSize = 5;
        }
        if (linesOfTriangle > 8)
        {
            linesOfTriangle -= 8;
            linesOfTriangle /= 2;
            Vector3 positionToMove = new Vector3(0, -linesOfTriangle * sizeElementY, -10);

            transform.position = positionToMove;
        }
        else
        {
            linesOfTriangle = 8 - linesOfTriangle;
            linesOfTriangle /= 2;
            Vector3 positionToMove = new Vector3(0, linesOfTriangle * sizeElementY, -10);

            transform.position = positionToMove;
        }
    }
}
