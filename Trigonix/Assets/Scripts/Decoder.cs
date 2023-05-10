using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoder : MonoBehaviour
{
    [SerializeField] private CodeGenerator _codeGen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDecode()
    {
        
        
        Debug.Log("Starting Decoding");
        Transform[] elements = _codeGen.GetElementsTransforms();
        char[] characters = new char[elements.Length];
        //Debug.Log(elements.Length);
        for (int i = 0; i < elements.Length; i++)
        {
            characters[i] = DecodeChar(elements[i]);
            //str = str + seed[i].ToString();
            
        }
        string str = new string(characters);
        Debug.Log(str);


    }

    private char DecodeChar(Transform el)
    {
        SpriteRenderer SR = el.GetComponent<SpriteRenderer>();
        if (SR == null) return 'e';
        else
        {
            return DecodeColor(SR.color);
        }
    }

    private char DecodeColor(Color col)
    {
        float red = col.r * 3;
        float green = col.g * 7;
        float blue = col.b * 3;
        float num = red * 32 + green * 4 + blue;
        int numm = (int)num;
        char myChar;
        myChar = (char)numm;
        //myChar = Convert.ToChar(numm);
        return myChar;
    }
}
