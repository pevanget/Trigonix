using UnityEngine;

public abstract class MaskBase : MonoBehaviour
{
    public enum MaskName { mask1, mask2, mask3, mask4 };
    public MaskName maskName;

    protected Color[] _maskColors = new Color[MaskName.GetNames(typeof(MaskName)).Length];
    protected Color _thisMaskColor;
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
            //SpriteRenderer SR = elements[i].GetComponent<SpriteRenderer>();
            //if (i % 2 == 0)
            //{
            //    SR.color = Color.white;
            //}

        }

        score += ScoringFunctionA(elements);
        return score;
    }

    private int ScoringFunctionA(Transform[] elements)
    {
        int scoreA = 0;
        for (int i = 0; i < elements.Length; i++)
        {

        }




        return scoreA;
    }

    public Color[] GetColors()
    {
        return _maskColors;
    }    


    
}
