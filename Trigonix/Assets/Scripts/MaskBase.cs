using UnityEngine;

public abstract class MaskBase : MonoBehaviour
{
    public enum MaskName { mask1, mask2, mask3, mask4 };
    public MaskName maskName;
    private float _thresholdSimilarPenalty = 0.5f;
    private float _thresholdDifferentReward = 0.75f;
    protected Color[] _maskColors = new Color[MaskName.GetNames(typeof(MaskName)).Length];
    protected Color _thisMaskColor;
    protected Vector2Int[] _triangleCoordinates = new Vector2Int[900];
    protected int[] _leftNeighbor = new int[900];
    protected int[] _rightNeighbor = new int[900];
    protected int[] _upNeighbor = new int[900];
    //protected int[] _downNeighbor = new int[900];
    //protected Transform _parentTrans;

    //allocate e.g. int maskFirstElements = 3  ===> start for i = 2. and do i =0 - 1 manually
    //ftiakse encoder

    protected virtual void Start()
    {
        //Debug.Log(MaskColors.Length);
    }
    public virtual void MaskCode(Transform parentToMask)
    {
        Debug.Log(this);
    }

    public int CountScore(Transform parentMasked)
    {
        int score = 0;

        Transform[] elements = new Transform[parentMasked.childCount];
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i] = parentMasked.GetChild(i);
        }

        score += ScoringFunctionA(elements);
        //score += ScoringFunctionB(elements);
        return score;
    }

    private int ScoringFunctionA(Transform[] elements)
    {
        int scoreA = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            SpriteRenderer SR = elements[i].GetComponent<SpriteRenderer>();
            if (_leftNeighbor[i] >= 0)
            {
                SpriteRenderer SRLeft = elements[_leftNeighbor[i]].GetComponent<SpriteRenderer>();
                scoreA += CompareColors(SR.color, SRLeft.color);
            }
            if (_rightNeighbor[i]>=0)
            {
                SpriteRenderer SRRight = elements[_rightNeighbor[i]].GetComponent<SpriteRenderer>();
                scoreA += CompareColors(SR.color, SRRight.color);
            }
            if (_upNeighbor[i] >= 0)
            {
                SpriteRenderer SRUp = elements[_upNeighbor[i]].GetComponent<SpriteRenderer>();
                scoreA += CompareColors(SR.color, SRUp.color);
            }
        }
        Debug.Log(maskName + " mask has " + scoreA + " penalty score");
        return scoreA;
    }

    //private int ScoringFunctionB(Transform[] elements) NOT FUNCTIONING                      ///////////////
    //{
    //    int scoreB = 0;
    //    for (int i = 0; i < elements.Length; i++)
    //    {
    //        SpriteRenderer SR = elements[i].GetComponent<SpriteRenderer>();
    //        if (_leftNeighbor[i] >= 0)
    //        {
    //            SpriteRenderer SRLeft = elements[_leftNeighbor[i]].GetComponent<SpriteRenderer>();
    //            scoreA += CompareColors(SR.color, SRLeft.color);
    //        }
    //        if (_rightNeighbor[i] >= 0)
    //        {
    //            SpriteRenderer SRRight = elements[_rightNeighbor[i]].GetComponent<SpriteRenderer>();
    //            scoreA += CompareColors(SR.color, SRRight.color);
    //        }
    //        if (_upNeighbor[i] >= 0)
    //        {
    //            SpriteRenderer SRUp = elements[_upNeighbor[i]].GetComponent<SpriteRenderer>();
    //            scoreA += CompareColors(SR.color, SRUp.color);
    //        }
    //    }
    //    Debug.Log(maskName + " mask has " + scoreA + " penalty score");
    //    return scoreA;

    //}

    public Color[] GetColors()
    {
        return _maskColors;
    }    

    public void SetTriangleCoords(Vector2Int[] triangleCoords)
    {
        _triangleCoordinates = triangleCoords;
    }

    public void SetNeighbors(int[] left, int[] right, int[] up)
    {
        _leftNeighbor = left;
        _rightNeighbor = right;
        _upNeighbor = up;
        //_downNeighbor = down;
    }


    protected int CompareColors (Color a, Color b)
    {
        //Debug.Log(a);
        //Debug.Log(b);
        int valueToReturn = 0;
        Color c = a - b;
        float valueToComp = c.r * c.r + c.g * c.g + c.b * c.b;
        valueToComp = Mathf.Sqrt(valueToComp);
        if (valueToComp < _thresholdSimilarPenalty) valueToReturn -= 5;
        if (valueToComp > _thresholdDifferentReward) valueToReturn += 1;
        //else valueToReturn++;

        return valueToReturn;
    }


    
}
