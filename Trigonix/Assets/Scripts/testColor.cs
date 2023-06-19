using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testColor : MonoBehaviour
{
    Camera cam;
    public Vector3 myVector3 = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(myVector3);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

    }
}
