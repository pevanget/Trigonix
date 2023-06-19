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
    [SerializeField] private Transform _masksContainer;
    private List<MaskBase> _masks = new List<MaskBase>();
    //private MaskBase[] _masks = new MaskBase[4];
    private Transform[] _elements;
    private Transform[] _parentMasked = new Transform[3];
    private Vector2Int[] _triangleCoordsElements = new Vector2Int[900];

    private void Start()
    {
        for (int i = 0; i < 900; i++)
        {
            _triangleCoordsElements[i] = ToTriangleCoordinates(i);
        }
        int childCounter = 0;
        foreach (Transform child in _masksContainer)
        {
            _masks.Add(child.GetComponent<MaskBase>());
            childCounter++;
        }


        //Debug.Log(_triangleCoordsElements[72]);
        //Debug.Log(_triangleCoordsElements[772]);
    }

    public void StartMasking()
    {
        _elements = _encoder.GetElementsTransforms();
        ClearOldParentObjects();
        DuplicateCodeForMasking();
        for (int i = 0; i < _masks.Count; i++)
        {
            _masks[i].MaskCode();
        }

        //Debug.Log(_elements.Length);
    }

    private void DuplicateCodeForMasking()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 positionToSpawn = new Vector3((-i - 1) * 50, 0, 0);
            //Debug.Log(i);
            //Debug.Log(_parentMasked.Length);
            //Debug.Log(_parentMasked[i].position);
            _parentMasked[i] = Instantiate(_elements[0].parent, positionToSpawn, Quaternion.identity);
            _parentMasked[i].position = positionToSpawn;

        }
    }

    private void ClearOldParentObjects()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_parentMasked[i] != null)
            {
                Destroy(_parentMasked[i].gameObject);
            }
        }
    }

    private void Mask1()
    {

    }
    private void Mask2()
    {

    }
    private void Mask3()
    {

    }
    private void Mask4()
    {

    }

    private Vector2Int ToTriangleCoordinates(int i)
    {
        int row = Mathf.FloorToInt(Mathf.Sqrt(i));
        int numberInRow = i - (int)Mathf.Pow(row, 2);
        Vector2Int triangleCoordinates = new(row, numberInRow);
        return triangleCoordinates;
    }


    //find row = square root of i floored down
    //find number in row = i - row^2
}