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

        char?[] characters = new char?[elementsMasked.Length];
        string str = null;
        for (int i = 1; i < elementsMasked.Length; i++)
        {
            characters[i - 1] = DecodeElement(elementsMasked[i]);
            str += characters[i - 1];
        }
        //string str = new string(characters);
        _decodedText.text = str;
        //Transform elementsMasked = 
    }

    private int FindMask(Transform elementOfMask)
    {
        int idOfMask = -1;
        SpriteRenderer SR = elementOfMask.GetComponent<SpriteRenderer>();
        Color maskColor = SR.color;
        idOfMask = FindMask(maskColor);

        if (idOfMask < 0) Debug.LogError("no mask was found");


        return idOfMask;
    }

    public int FindMask(Color colorOfThisMask)
    {
        int maskID = -1;
        //Debug.Log(colorOfThisMask);
        for (int i = 0; i < _masker.GetNumberOfMasks(); i++)
        {
            Color colorOfMask = _masker.GetMask(i).GetColorOfMask(i);
            if (colorOfThisMask == colorOfMask)
            {
                maskID = i;
            }
            //Color a = colorOfThisMask - colorOfMask;
            //Debug.Log(colorOfMask);
            //Debug.Log(a);
        }
        if (maskID < 0) Debug.LogError("no mask was found");
        return maskID;
    }


    private void DecodeUnmasked()
    {
        Debug.Log("Starting Unmasked Decoding");
        
        Transform[] elements = _encoder.GetElementsTransforms();
        char?[] characters = new char?[elements.Length];
        string str = null;

        for (int i = 1; i < elements.Length; i++)
        {
            characters[i] = DecodeElement(elements[i]);
            str += characters[i];
        }
        _decodedText.text = str;
    }

    private char? DecodeElement(Transform el)
    {
        SpriteRenderer SR = el.GetComponent<SpriteRenderer>();
        if (SR == null) return '0';
        else
        {
            return DecodeColor(SR.color);
        }
    }

    public char? DecodeColor(Color col)
    {
        //Debug.Log(col);
        float red = col.r * (Mathf.Pow(2, _redDepth) - 1);
        float green = col.g * (Mathf.Pow(2, _greenDepth) - 1);
        float blue = col.b * (Mathf.Pow(2, _blueDepth) - 1);

        float num = red * _redMultiplier + green * _greenMultiplier + blue * _blueMultiplier;
        
        int numm = Mathf.RoundToInt(num);
        
        if (numm == 127) return null;
        
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
