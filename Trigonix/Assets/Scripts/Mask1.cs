using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask1 : MaskBase
{
    



    protected override void Start()
    {
        maskName = MaskName.mask1;
    }

    public override void MaskCode()
    {
        Debug.Log(this);
    }
}
