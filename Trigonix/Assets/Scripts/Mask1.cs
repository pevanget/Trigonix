using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This mask switches R and B values

public class Mask1 : MaskBase
{
    protected override void Start()
    {
        //base.Start();
        maskName = MaskName.mask1;
        _thisMaskColor = Color.red;

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
                SR.color = new Color(SR.color.b, SR.color.g, SR.color.r);
            }
            //SR.color = Color.white;

        }
    }

    public override void UnmaskElement(int i, Transform element)
    {
        if (i % 2 == 0)
        {
            SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
            SR.color = new Color(SR.color.b, SR.color.g, SR.color.r);
        }
    }

    public override void UnmaskElement(int i, Color color)
    {
        color = new Color(color.b, color.g, color.r);
        //Debug.Log(color);
    }
}

//algorithm
//SCORING: check adjacent metro color
//if i%2==0 + kati else - kati
//script nearest neighbors o deksia o aristera kai o panw H katw
//QOL = scroll camera?

//    		    0,0
//		    1,0	1,1	1,2
//	    2,0	2,1	2,2	2,3	2,4
//  3,0	3,1	3,2	3,3	3,4	3,5	3,6
// if
