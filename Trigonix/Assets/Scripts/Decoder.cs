using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoder : MonoBehaviour
{
    [SerializeField] private CodeGenerator _codeGen;

    private int _redDepth, _greenDepth, _blueDepth;
    private int _redMultiplier, _greenMultiplier, _blueMultiplier;


  

    public void StartDecode()
    {
        
        
        Debug.Log("Starting Decoding");
        Transform[] elements = _codeGen.GetElementsTransforms();
        char[] characters = new char[elements.Length];

        for (int i = 0; i < elements.Length; i++)
        {
            characters[i] = DecodeElement(elements[i]);           
        }
        string str = new string(characters);
        Debug.Log(str);


    }

    private char DecodeElement(Transform el)
    {
        SpriteRenderer SR = el.GetComponent<SpriteRenderer>();
        if (SR == null) return '0';
        else
        {
            return DecodeColor(SR.color);
        }
    }

    private char DecodeColor(Color col)
    {
        float red = col.r * (Mathf.Pow(2,_redDepth)-1);
        float green = col.g * (Mathf.Pow(2, _greenDepth) - 1);
        float blue = col.b * (Mathf.Pow(2, _redDepth) - 1);
        float num = red * _redMultiplier + green * _greenMultiplier + blue * _blueMultiplier;
        int numm = (int)num;
        char myChar;
        myChar = (char)numm;
        return myChar;
    }

    public void SetDepths(int r, int g, int b)
    {
        _redDepth = r;
        _greenDepth = g;
        _blueDepth = b;
        _redMultiplier = (int) Mathf.Pow(2, g+b);
        _greenMultiplier = (int) Mathf.Pow(2, b);
        _blueMultiplier = 1;
    }
}
