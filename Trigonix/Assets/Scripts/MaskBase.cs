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

    protected int CountScore()
    {
        int score = 0;
        return score;
    }

    public Color[] GetColors()
    {
        return _maskColors;
    }    


    
}
