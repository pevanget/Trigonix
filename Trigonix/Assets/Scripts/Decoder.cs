using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Decoder : MonoBehaviour
{
    [SerializeField] private Encoder _encoder;
    [SerializeField] private Masker _masker;
    [SerializeField] private TMP_InputField _decodedText;
    //[SerializeField] private TMP_Text _decodedMessageUsingMask;
    [SerializeField] private Toggle _maskingToggle;

    private int _redDepth, _greenDepth, _blueDepth;
    private int _redMultiplier, _greenMultiplier, _blueMultiplier;




    public void StartDecode()
    {
        if (_maskingToggle.isOn) DecodeMasked();
        else DecodeUnmasked();
    }

    private void DecodeMasked()
    {
        Transform parentMasked = _masker.GetBestParentMasked();
        Transform[] elementsMasked = new Transform[parentMasked.childCount];

        Debug.Log("hello");


        for (int i = 0; i < elementsMasked.Length; i++)
        {
            elementsMasked[i] = parentMasked.GetChild(i);
        }
        int maskID = FindMask(elementsMasked[0]);
        for (int i = 1; i < elementsMasked.Length; i++)
        {
            SpriteRenderer SR = elementsMasked[i].GetComponent<SpriteRenderer>();
            _masker.GetMask(maskID).UnmaskElement(i, elementsMasked[i]);
        }

        char[] characters = new char[elementsMasked.Length];

        for (int i = 1; i < elementsMasked.Length; i++)
        {
            characters[i - 1] = DecodeElement(elementsMasked[i]);
            //Debug.Log("helo");
        }
        string str = new string(characters);
        _decodedText.text = str;
        //Transform elementsMasked = 
    }

    private int FindMask(Transform elementOfMask)
    {
        int idOfMask = -1;
        SpriteRenderer SR = elementOfMask.GetComponent<SpriteRenderer>();
        Color maskColor = SR.color;
        idOfMask = FindMask(maskColor);
        

        return idOfMask;
    }

    public int FindMask(Color colorOfMask)
    {
        int maskID = -1;
        for (int i = 0; i < _masker.GetNumberOfMasks(); i++)
        {
            if (colorOfMask == _masker.GetMask(i).GetColorOfMask(i))
            {
                maskID = i;
            }
        }
        return maskID;
    }


    private void DecodeUnmasked()
    {
        Debug.Log("Starting Unmasked Decoding");
        Transform[] elements = _encoder.GetElementsTransforms();
        char[] characters = new char[elements.Length];

        for (int i = 0; i < elements.Length; i++)
        {
            characters[i] = DecodeElement(elements[i]);
        }
        string str = new string(characters);
        _decodedText.text = str;
        //Debug.Log(str); //show in ui!
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

    public char DecodeColor(Color col)
    {
        float red = col.r * (Mathf.Pow(2, _redDepth) - 1);
        float green = col.g * (Mathf.Pow(2, _greenDepth) - 1);
        float blue = col.b * (Mathf.Pow(2, _blueDepth) - 1);
        float num = red * _redMultiplier + green * _greenMultiplier + blue * _blueMultiplier;
        //Debug.Log(num);
        //int numm = (int)num;
        int numm = Mathf.RoundToInt(num);
        //Debug.Log(numm);
        char myChar;
        myChar = (char)numm;
        return myChar;
    }

    public void SetDepths(int r, int g, int b)
    {
        _redDepth = r;
        _greenDepth = g;
        _blueDepth = b;
        _redMultiplier = (int)Mathf.Pow(2, g + b);
        _greenMultiplier = (int)Mathf.Pow(2, b);
        _blueMultiplier = 1;
    }
}
