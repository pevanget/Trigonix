using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFieldHandler : MonoBehaviour
{
    private string _stringToEdit;
    [SerializeField] private TMPro.TMP_InputField _textToEncode;

    //void OnGUI()
    //{
    //    // Make a text field that modifies stringToEdit.
    //    _stringToEdit = GUI.TextField(new Rect(10, 10, 200, 50), _stringToEdit, 1556);
    //}


    //public void PassCodePhrase()
    //{

    //}

    private void Start()
    {
        //_textToEncode = GetComponentInChildren<TMPro.TextMeshPro>();
        //_textToEncode = GetComponent<TMPro.TextMeshPro>();
    }

    public string GetStringToEncode()
    {
        _stringToEdit = _textToEncode.text.ToString();
        return _stringToEdit;
    }

}
