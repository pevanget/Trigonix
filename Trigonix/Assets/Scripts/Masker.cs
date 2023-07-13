using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Masker : MonoBehaviour
{
    //masking presets 0-X
    //number of masks 1-??
    //triangles allocated for masking preset ??
    //positions of masking

    [SerializeField] private Encoder _encoder;
    [SerializeField] private Transform _masksContainer;
    [SerializeField] private TMP_Text _bestMask;
    private List<MaskBase> _masks = new List<MaskBase>();
    //private MaskBase[] _masks = new MaskBase[4];
    private Transform[] _unmaskedElements;
    private Transform[] _maskedElementsToDecode;
    private Transform[] _parentMasked;
    private Vector2Int[] _triangleCoordsElements = new Vector2Int[900];
    private int[] _leftNeighbor = new int[900];
    private int[] _rightNeighbor = new int[900];
    private int[] _upNeighbor = new int[900];
    private int[] _scoresMasks;
    private Transform _parentBestMaskedCode;
    private int _numberOfMasks;
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
        _numberOfMasks = _masks.Count;
        _parentMasked = new Transform[_masks.Count];
        _scoresMasks = new int[_masks.Count];
    }
    public MaskBase GetMask(int j) => _masks[j];
    public int GetNumberOfMasks() => _numberOfMasks;
    public void StartMasking()
    {
        _unmaskedElements = _encoder.GetElementsTransforms();
        Debug.Log(_unmaskedElements.Length);
        ClearOldParentObjects();
        DuplicateCodeForMasking();
        for (int i = 0; i < _masks.Count; i++)
        {
            //mipws 8a paei arga afto?
            _masks[i].MaskCode(_parentMasked[i]);
            _scoresMasks[i] = _masks[i].CountScore(_parentMasked[i]);            
        }
        int _bestMaskIndex = FindBestMask(_scoresMasks);
        _parentBestMaskedCode = _parentMasked[_bestMaskIndex]; 


    }

    private int FindBestMask(int[] scores)
    {
        int maxScore = scores.Max();
        int maxIndex = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (maxScore == scores[i])
            {
                maxIndex = i;
            }
        }
        int debugMaskIndex = maxIndex + 1;
        _bestMask.text = "Best Mask: " + debugMaskIndex;
        //Debug.Log("Best mask is mask "+ debugMaskIndex);

        return maxIndex;
    }

    private void DuplicateCodeForMasking()
    {
        for (int i = 0; i < _masks.Count; i++)
        {
            Vector3 positionToSpawn = new Vector3((-i - 1) * 50, 0, 0);
            _parentMasked[i] = Instantiate(_unmaskedElements[0].parent, positionToSpawn, Quaternion.identity);
            _parentMasked[i].position = positionToSpawn;

        }
    }

    private void ClearOldParentObjects()
    {
        for (int i = 0; i < _numberOfMasks; i++)
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

    public Transform GetBestParentMasked() =>_parentBestMaskedCode;
    


    #region Neighbors
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
            //ABOVE IS SHORTER
            
            //THIS IS CORRECT TOO
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
    #endregion


    //find row = square root of i floored down
    //find number in row = i - row^2



}