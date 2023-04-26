using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFieldHandler : MonoBehaviour
{
    [SerializeField] private string _stringToEdit = "Hello cruel World";

    void OnGUI()
    {
        // Make a text field that modifies stringToEdit.
        _stringToEdit = GUI.TextField(new Rect(10, 10, 200, 50), _stringToEdit, 1556);
    }


    public void PassCodePhrase()
    {

    }

    public string GetStringToEncode()
    {
        return _stringToEdit;
    }

}
