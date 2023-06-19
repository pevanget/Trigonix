using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaskBase : MonoBehaviour
{
    public enum MaskName { mask1, mask2, mask3, mask4 };
    public MaskName maskName;

    protected virtual void Start()
    {

    }
    public virtual void MaskCode()
    {
        Debug.Log(this);
    }

    protected int CountScore()
    {
        int score = 0;
        return score;
    }
}
