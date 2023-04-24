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
    public bool Check = false;


    // Start is called before the first frame update
    void Start()
    {
        _encodedBytes = ascii.GetBytes(StringToEncode);
        Debug.Log(_encodedBytes);
        foreach (byte b in _encodedBytes)
        {
            Debug.Log(b + " ");
        }
        _decodedBytesASCII = ascii.GetString(_encodedBytes);
        Debug.Log("ASCII text: " + _decodedBytesASCII);
        StringContainsOnlyASCII(StringToEncode);

    }

    // Update is called once per frame
    void Update()
    {
        if (Check)
        {
            Check = false;
            StringContainsOnlyASCII(StringToEncode);
        }
    }

    [ContextMenu("StringContainsOnlyASCII")]
    public bool StringContainsOnlyASCII(string stringToCheck)
    {
        byte[] encodedBytesASCII = ascii.GetBytes(stringToCheck);
        string decodedBytesASCII = ascii.GetString(encodedBytesASCII);
        bool isASCII = (stringToCheck == decodedBytesASCII);
        if (isASCII) Debug.Log("This is an ASCII only string");
        else Debug.Log("This is not an ASCII only string");
        return (isASCII);
    }
}
