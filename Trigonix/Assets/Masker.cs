using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masker : MonoBehaviour
{
    //masking presets 0-X
    //number of masks 1-??
    //triangles allocated for masking preset ??
    //positions of masking

    [SerializeField] private Encoder _encoder;
    private Transform[] _elements;

    public void StartMasking()
    {
        _elements = _encoder.GetElementsTransforms();
        //Debug.Log(_elements.Length);
    }

}