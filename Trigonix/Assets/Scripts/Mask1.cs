using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask1 : MaskBase
{
    



    protected override void Start()
    {
        maskName = MaskName.mask1;
    }

    public override void MaskCode(Transform parentToMask)
    {
        //base.MaskCode(parentToMask);
        Debug.Log(this);
        int elementCounter = 1;
        foreach (Transform element in parentToMask)
        {
            SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
            if (elementCounter % 2 == 0)
            {
                SR.color = Color.white;
            }
            elementCounter++;
        }
    }
}
