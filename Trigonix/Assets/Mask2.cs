using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask2 : MaskBase
{
    protected override void Start()
    {
        maskName = MaskName.mask2;
    }

    public override void MaskCode()
    {
        Debug.Log(this);
    }
}
