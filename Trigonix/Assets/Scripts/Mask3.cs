using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This mask shifts RGB values to the left (RGB -> GBR)

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
            if (i % 2 == 0)
            {
                SR.color = new Color(SR.color.g, SR.color.b, SR.color.r);
            }
            //SR.color = Color.white;
        }
    }

    public override void UnmaskElement(int i, Transform element)
    {
        if (i % 2 == 0)
        {
            SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
            SR.color = new Color(SR.color.b, SR.color.r, SR.color.g);
            //Debug.Log(SR.color);
        }
    }

    public override void UnmaskElement(int i, Color color)
    {
        color = new Color(color.b, color.r, color.g);
        //Debug.Log(color);
    }
}
