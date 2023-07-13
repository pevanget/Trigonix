using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This mask shifts RGB values to the right (RGB -> BRG)
public class Mask4 : MaskBase
{
    protected override void Start()
    {
        maskName = MaskName.mask4;
        _thisMaskColor = Color.white;

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
            //if (i % 2 == 0)
            {
                SR.color = new Color(SR.color.b, SR.color.r, SR.color.g);
            }
            //SR.color = Color.white;
        }
    }

    public override void UnmaskElement(int i, Transform element)
    {
        //if (i % 2 == 0)
        {
            SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
            SR.color = new Color(SR.color.g, SR.color.b, SR.color.r);
            Debug.Log(SR.color);
        }
    }
}
