using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask3 : MaskBase
{
    protected override void Start()
    {
        maskName = MaskName.mask3;
        _thisMaskColor = Color.blue;

    }

    public override void MaskCode(Transform parentToMask)
    {
        Transform[] elements = new Transform[parentToMask.childCount];
        elements[0] = parentToMask.GetChild(0);
        elements[0].GetComponent<SpriteRenderer>().color = _thisMaskColor;
        for (int i = 1; i < elements.Length; i++)
        {
            elements[i] = parentToMask.GetChild(i);
            SpriteRenderer SR = elements[i].GetComponent<SpriteRenderer>();
            if (i % 4 == 0)
            {
                SR.color = Color.white;
            }

        }
    }
}
