using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TESTASCII : MonoBehaviour
{
    //https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding.ascii?view=net-7.0
    public Encoding ascii = Encoding.ASCII;
    public string StringToEncode = "Oh les patates! I love Les Patates! ð";

    private byte[] _encodedBytes;
    private string _decodedBytesASCII;
    public bool CheckValid = false;
    public int MaxSizeForString = 10;


    // Start is called before the first frame update
    void Start()
    {
        _encodedBytes = ascii.GetBytes(StringToEncode);
        Debug.Log(_encodedBytes);
        foreach (byte b in _encodedBytes)
        {
            //Debug.Log(b + " ");
        }
        _decodedBytesASCII = ascii.GetString(_encodedBytes);
        //Debug.Log("ASCII text: " + _decodedBytesASCII);
        //StringContainsOnlyASCII(StringToEncode);
        CheckStringValid(StringToEncode);

    }

    // Update is called once per frame
    void Update()
    {

        if (CheckValid)
        {
            CheckValid = false;
            CheckStringValid(StringToEncode);
        }
    }

    private bool CheckStringValid(string stringToCheck)
    {
        bool IsNotNull = CheckNull(stringToCheck);
        bool IsASCII = CheckStringContainsOnlyASCII(stringToCheck);
        bool IsNotTooBig = CheckSize(stringToCheck);
        bool isValid = (IsASCII && IsNotTooBig && IsNotNull);

        if (isValid) Debug.Log("Valid string");
        else Debug.LogWarning("Invalid string");
        return isValid;
    }

    private bool CheckNull(string stringToCheck)
    {
        bool notNull = stringToCheck != null;
        if (notNull) Debug.Log("String is not null");
        else Debug.LogWarning("String is null!");
        return notNull;
    }
    private bool CheckSize(string stringToCheck)
    {
        bool isNotTooBig = (stringToCheck.Length <= MaxSizeForString);
        bool isNotEmpty = (stringToCheck.Length > 0);
        bool isValidSize = (isNotEmpty && isNotTooBig);
        if (isNotTooBig && isNotEmpty) Debug.Log("String is valid size");
        else if (!isNotTooBig) Debug.LogWarning("String is too big!");
        else if (!isNotEmpty) Debug.LogWarning("String is empty!");

        return (isValidSize);
    }


    public bool CheckStringContainsOnlyASCII(string stringToCheck)
    {
        byte[] encodedBytesASCII = ascii.GetBytes(stringToCheck);
        string decodedBytesASCII = ascii.GetString(encodedBytesASCII);
        bool isASCII = (stringToCheck == decodedBytesASCII);
        if (isASCII) Debug.Log("This is an ASCII only string");
        else Debug.LogWarning("This is not an ASCII only string");
        return (isASCII);
    }
}
