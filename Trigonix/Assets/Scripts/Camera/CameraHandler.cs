using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    [SerializeField] Camera _mainCamera;
    [SerializeField] Camera _secondaryCamera;
    [SerializeField] GameObject _mainCameraUI;
    [SerializeField] GameObject _secondaryCameraUI;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchCameras()
    {
        _mainCamera.enabled = !_mainCamera.enabled;
        _mainCameraUI.SetActive(!_mainCameraUI.activeSelf);
        _secondaryCamera.enabled = !_secondaryCamera.enabled;
        _secondaryCameraUI.SetActive(!_secondaryCameraUI.activeSelf);
    }
}
