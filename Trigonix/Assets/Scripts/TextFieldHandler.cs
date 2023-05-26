using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFieldHandler : MonoBehaviour
{
    private string _stringToEdit;
    
    [SerializeField] private TMPro.TMP_InputField _textToEncode;

   

    public string GetStringToEncode()
    {
        _stringToEdit = _textToEncode.text.ToString();
        return _stringToEdit;
    }

}
