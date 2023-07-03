using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask2 : MaskBase
{
    protected override void Start()
    {
        maskName = MaskName.mask2;
        _thisMaskColor = Color.green;

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
            //if (i % 3 == 0)
            //{
            SR.color = Color.white;
            //}

        }
    }
}
