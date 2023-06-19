using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask4 : MaskBase
{
    protected override void Start()
    {
        maskName = MaskName.mask4;
    }

    public override void MaskCode()
    {
        Debug.Log(this);
    }
}
