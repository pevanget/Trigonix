using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This mask shuffles RGB values: RGB -> RBG
//test this string

//dfffee

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
            if (i % 2 == 0)
            {
                //SR.color = Color.white - SR.color;
                //Debug.Log(SR.color);

                //SR.color = new Color(1 - SR.color.r, 1 - SR.color.g, 1 - SR.color.b);
                //Debug.Log(SR.color);
                SR.color = new Color(SR.color.r, SR.color.b, SR.color.g);

            }
            //SR.color = Color.white;
        }
    }

    public override void UnmaskElement(int i, Transform element)
    {
        if (i % 2 == 0)
        {
            SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
            //SR.color = new Color(1 - SR.color.r, 1 - SR.color.g, 1 - SR.color.b);
            //Debug.Log(SR.color);
            SR.color = new Color(SR.color.r, SR.color.b, SR.color.g);
        }
    }
}
