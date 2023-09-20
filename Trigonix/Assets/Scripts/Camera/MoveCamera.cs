using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO;
using Unity.IO;

public class MoveCamera : MonoBehaviour
{
    private bool _lockCamera = false;
    [SerializeField] private float _cameraSpeed;
    
    private float x;
    private float y;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9)) _lockCamera = !_lockCamera;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
    }

    private void LateUpdate()
    {
        if (_lockCamera) return;
        transform.Translate(new Vector3(x * _cameraSpeed, y * _cameraSpeed) * Time.deltaTime);
    }
}
