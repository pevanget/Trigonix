using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMyCamera : MonoBehaviour
{
    private Camera _myCam;
    
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
        }
        else
        {
            _myCam.orthographicSize = 5;
        }
        if (linesOfTriangle > 8)
        {
            linesOfTriangle -= 8;
            linesOfTriangle /= 2;
            Vector3 positionToMove = new (0, -linesOfTriangle * sizeElementY, -10);

            transform.position = positionToMove;
        }
        else
        {
            linesOfTriangle = 8 - linesOfTriangle;
            linesOfTriangle /= 2;
            Vector3 positionToMove = new (0, linesOfTriangle * sizeElementY, -10);

            transform.position = positionToMove;
        }
    }
}
