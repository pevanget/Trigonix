using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CheckString : MonoBehaviour
{
    //https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding.ascii?view=net-7.0

    private Encoding _ascii = Encoding.ASCII;
    private int _maxSizeForString = 10;
    private bool _debugsOn = false;
    

    public bool CheckStringValid(string stringToCheck)
    {
        bool IsNotNull = CheckNull(stringToCheck);
        bool IsASCII = CheckStringContainsOnlyASCII(stringToCheck);
        bool IsNotTooBig = CheckSize(stringToCheck);
        bool isValid = (IsASCII && IsNotTooBig && IsNotNull);
        if (!_debugsOn) return isValid;

        //debugs
        if (isValid) Debug.Log("Valid string");
        else Debug.LogWarning("Invalid string");
        return isValid;
    }

    private bool CheckNull(string stringToCheck)
    {
        bool notNull = stringToCheck != null;
        if (!_debugsOn) return notNull;

        //debugs
        if (notNull) Debug.Log("String is not null");
        else Debug.LogWarning("String is null!");
        return notNull;
    }
    private bool CheckSize(string stringToCheck)
    {
        bool isNotTooBig = (stringToCheck.Length <= _maxSizeForString);
        bool isNotEmpty = (stringToCheck.Length > 0);
        bool isValidSize = (isNotEmpty && isNotTooBig);
        if (!_debugsOn) return isValidSize;

        //debugs
        if (isNotTooBig && isNotEmpty) Debug.Log("String is valid size");
        else if (!isNotTooBig) Debug.LogWarning("String is too big!");
        else if (!isNotEmpty) Debug.LogWarning("String is empty!");
        else Debug.Log("what's going on");
        return isValidSize;
    }


    private bool CheckStringContainsOnlyASCII(string stringToCheck)
    {
        byte[] encodedBytesASCII = _ascii.GetBytes(stringToCheck);
        string decodedBytesASCII = _ascii.GetString(encodedBytesASCII);
        bool isASCII = (stringToCheck == decodedBytesASCII);
        if (!_debugsOn) return (isASCII);

        //debugs
        if (isASCII) Debug.Log("This is an ASCII only string");
        else Debug.LogWarning("This is not an ASCII only string");
        return (isASCII);
    }

    public void SetMaxSizeForString(int maxSize) => _maxSizeForString = maxSize;
    public void SetDebuggers(bool val) => _debugsOn = val;
}
