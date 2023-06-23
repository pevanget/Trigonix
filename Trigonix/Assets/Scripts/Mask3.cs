using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask3 : MaskBase
{
    protected override void Start()
    {
        maskName = MaskName.mask3;
    }

    public override void MaskCode(Transform parentToMask)
    {
        Debug.Log(this);
    }
}
