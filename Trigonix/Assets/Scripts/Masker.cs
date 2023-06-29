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
    private Transform[] _parentMasked;
    private Vector2Int[] _triangleCoordsElements = new Vector2Int[900];
    private int[] _leftNeighbor = new int[900];
    private int[] _rightNeighbor = new int[900];
    private int[] _upNeighbor = new int[900];
    //private int[] _downNeighbor = new int[900];

    private void Start()
    {
        for (int i = 0; i < 900; i++)
        {
            _triangleCoordsElements[i] = ToTriangleCoordinates(i);

        }

        for (int i = 0; i < 900; i++)
        {
            FindNeighbors(i);
        }

        int childCounter = 0;
        foreach (Transform child in _masksContainer)
        {
            _masks.Add(child.GetComponent<MaskBase>());
            _masks[childCounter].SetTriangleCoords(_triangleCoordsElements);
            _masks[childCounter].SetNeighbors(_leftNeighbor, _rightNeighbor, _upNeighbor);
            childCounter++;
        }
        _parentMasked = new Transform[_masks.Count];
    }

    private void FindNeighbors(int n)
    {


        _leftNeighbor[n] = FindLeftNeighbor(n);
        _rightNeighbor[n] = FindRightNeighbor(n);
        _upNeighbor[n] = FindUpNeighbor(n);
    }

    private int FindLeftNeighbor(int n)
    {

        if (_triangleCoordsElements[n].y == 0)
        {
            //Debug.Log("no left");
            return -1; //does not exist
        }
        else
        {
            return n - 1;
        }
    }

    private int FindRightNeighbor(int n)
    {
        float numToCheck = n + 1;
        bool lastInLine = Mathf.Sqrt(n + 1) % 1 == 0;

        if (lastInLine)
        {
            //Debug.Log("no right");
            return -1; //does not exist
        }
        else
        {
            return n + 1;
        }
    }

    private int FindUpNeighbor(int n)
    {
        bool noUpneighbor = !((_leftNeighbor[n] >= 0) && (_rightNeighbor[n] >= 0));

        bool a = _leftNeighbor[n] >= 0;
        //if (!a) Debug.Log(_leftNeighbor[n] >= 0);
        if (noUpneighbor)
        {
            //Debug.Log(n);
            return -1; //does not exist
        }
        else
        {
            int valueToReturn;
            valueToReturn = n - 2 * _triangleCoordsElements[n].x;
            //THIS IS CORRECT
            //ABOVE IS SHORTER
            //Vector2Int posInTriangle = new Vector2Int(_triangleCoordsElements[n].x - 1, _triangleCoordsElements[n].y - 1);
            //valueToReturn = posInTriangle.x * posInTriangle.x + posInTriangle.y;
            return valueToReturn;
        }
    }

    //private int FindDownNeighbor(int n)
    //{NOT CORRECT!!!!
    //    bool noDownneighbor = !((_leftNeighbor[n] >= 0) && (_rightNeighbor[n] >= 0));

    //    bool a = _leftNeighbor[n] >= 0;
    //    //if (!a) Debug.Log(_leftNeighbor[n] >= 0);
    //    if (noDownneighbor)
    //    {
    //        Debug.Log(n);
    //        return -1; //does not exist
    //    }
    //    else
    //    {
    //        int valueToReturn;
    //        Vector2Int posInTriangle = new Vector2Int(_triangleCoordsElements[n].x - 1, _triangleCoordsElements[n].y - 1);
    //        valueToReturn = posInTriangle.x * posInTriangle.x + posInTriangle.y;
    //        return valueToReturn;
    //    }
    //}


    public void StartMasking()
    {
        _elements = _encoder.GetElementsTransforms();
        ClearOldParentObjects();
        DuplicateCodeForMasking();
        for (int i = 0; i < _masks.Count; i++)
        {
            //mipws 8a paei arga afto?
            _masks[i].MaskCode(_parentMasked[i]);
            _masks[i].CountScore(_parentMasked[i]);
        }
    }

    private void DuplicateCodeForMasking()
    {
        for (int i = 0; i < _masks.Count; i++)
        {
            Vector3 positionToSpawn = new Vector3((-i - 1) * 50, 0, 0);
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