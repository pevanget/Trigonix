using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMyCamera : MonoBehaviour
{
    private Camera _myCam;
    [SerializeField] private Camera _screenshotCamera;
    
    void Start()
    {
        _myCam = GetComponent<Camera>();
    }
   
    public void AdjustCamera(int linesOfTriangle, float sizeElementY)
    {
        linesOfTriangle++; //starts from 0

        if (linesOfTriangle > 9)
        {
            _myCam.orthographicSize = Mathf.Floor((float)((linesOfTriangle) / 5f)) * 3 + 2;
            _screenshotCamera.orthographicSize = _myCam.orthographicSize;
        }
        else
        {
            _myCam.orthographicSize = 5;
            _screenshotCamera.orthographicSize = _myCam.orthographicSize;
        }
        if (linesOfTriangle > 8)
        {
            linesOfTriangle -= 8;
            linesOfTriangle /= 2;
            Vector3 positionToMove = new (0, -linesOfTriangle * sizeElementY, -10);

            transform.position = positionToMove;
            _screenshotCamera.transform.position = positionToMove;
        }
        else
        {
            linesOfTriangle = 8 - linesOfTriangle;
            linesOfTriangle /= 2;
            Vector3 positionToMove = new (0, linesOfTriangle * sizeElementY, -10);

            transform.position = positionToMove;
            _screenshotCamera.transform.position = positionToMove;
        }
    }


    //private void Update()
    //{
    //    if (Input.GetAxis)
    //}
}