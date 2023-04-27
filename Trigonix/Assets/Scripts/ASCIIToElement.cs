using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ASCIIToElement : MonoBehaviour
{
    //32 mexri 126
    public string a = "a";
    public Encoding ascii = Encoding.ASCII;
    private byte[] _encodedBytes;
    private string _decodedBytesASCII;
    SpriteRenderer SR;
    public bool check = false;


    private void Start()
    {
        byte[] encodedBytesASCII = ascii.GetBytes(a);
        //foreach (byte b in encodedBytesASCII)
        //{
        //    Debug.Log(b);
        //}
        SR = GetComponent<SpriteRenderer>();
        Debug.Log(encodedBytesASCII[0]);
        SR.color = PaintElement(encodedBytesASCII[0]);
        Debug.Log(SR.color);
    }

    private void Update()
    {
        if (check)
        {
            check = false;
            byte[] encodedBytesASCII = ascii.GetBytes(a);
            SR.color = PaintElement(encodedBytesASCII[0]);
        }
    }

    public Color PaintElement(byte b)
    {
        
        int colorInt = (int) b;
        Color elementColor = new Color();
        elementColor.r = RedCalculator(colorInt);
        elementColor.g = GreenCalculator(colorInt);
        elementColor.b = BlueCalculator(colorInt);
        elementColor.a = 1f;

        return elementColor;
    }

    private float BlueCalculator(int val)
    {
        Debug.Log(val);
        int mod = val % 4;
        Debug.Log(mod);
        float blueValue = (float) mod / 4f;
        Debug.Log(blueValue);

        return blueValue;
    }

    private float GreenCalculator(int val)
    {
        int mod = val % 32 / 4;
        float greenValue = (float) mod / 8f;
        return greenValue;
    }
    private float RedCalculator(int val)
    {
        int mod = val % 128 / 32;
        float redValue = (float) mod / 4f;
        return redValue;
    }



    private void ColorSeparator()
    {
        //RRRGGGBB
        //Red = mod128 xwris ta alla
        //Green = mod32 xwris ta alla
        //Blue = mod4
    }
}
