using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ASCIIToElement : MonoBehaviour
{
    [Header("Blue")]
    [SerializeField] private int _blueMod = 4; //this should be 0-1
    [SerializeField] private int _blueColorDepth = 2;
    [SerializeField] private int _blueShiftPosition = 0;
    [Header("Green")]
    [SerializeField] private int _greenMod = 32; //this should be 2-4
    [SerializeField] private int _greenColorDepth = 3;
    [SerializeField] private int _greenShiftPosition = 2;
    [Header("Red")]
    [SerializeField] private int _redMod = 128; //this should be 5-6
    [SerializeField] private int _redColorDepth = 2;
    [SerializeField] private int _redShiftPosition = 5;

    [Header("Misc")]
    public bool DebugColorValues = false;
    public Encoding ascii = Encoding.ASCII;
    [SerializeField] private Decoder _decoder;

    private void Start()
    {
        if (_decoder != null)
        {
            _decoder.SetDepths(_redColorDepth, _greenColorDepth, _blueColorDepth);
        }
        else
        {
            Debug.LogWarning("No decoder");
        }
    }

    public Color PaintElement(byte b)
    {

        int colorInt = (int)b;
        Color elementColor = new Color();
        elementColor.r = RedCalculator(colorInt);
        elementColor.g = GreenCalculator(colorInt);
        elementColor.b = BlueCalculator(colorInt);
        elementColor.a = 1f;

        return elementColor;
    }

    private float BlueCalculator(int val)
    {
        int _blueDivToShift = (int)Mathf.Pow(2, _blueShiftPosition);
        int mod = val % _blueMod / _blueDivToShift;
        float bluePresets = Mathf.Pow(2, _blueColorDepth);
        float blueValue = (float)mod / (bluePresets - 1);
        if (DebugColorValues)
        {
            Debug.Log(val);
            Debug.Log(mod);
            Debug.Log(blueValue);
        }

        return blueValue;
    }

    private float GreenCalculator(int val)
    {
        int _greenDivToShift = (int)Mathf.Pow(2, _greenShiftPosition);
        int mod = val % _greenMod / _greenDivToShift;
        float greenPresets = Mathf.Pow(2, _greenColorDepth);
        float greenValue = (float)mod / (greenPresets - 1);
        if (DebugColorValues)
        {
            Debug.Log(val);
            Debug.Log(mod);
            Debug.Log(greenValue);
        }
        return greenValue;
    }
    private float RedCalculator(int val)
    {
        int _redDivToShift = (int)Mathf.Pow(2, _redShiftPosition);

        int mod = val % _redMod / _redDivToShift;
        float redPresets = Mathf.Pow(2, _redColorDepth);
        float redValue = (float)mod / (redPresets - 1);
        if (DebugColorValues)
        {
            Debug.Log(val);
            Debug.Log(mod);
            Debug.Log(redValue);
        }

        return redValue;
    }



    private void ColorSeparator() //might be helpful
    {
        //RRRGGGBB
        //Red = mod128 xwris ta alla
        //Green = mod32 xwris ta alla
        //Blue = mod4
    }
}
