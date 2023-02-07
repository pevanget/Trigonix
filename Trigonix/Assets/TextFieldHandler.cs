using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFieldHandler : MonoBehaviour
{
    public string stringToEdit = "Hello cruel World";

    void OnGUI()
    {
        // Make a text field that modifies stringToEdit.
        stringToEdit = GUI.TextField(new Rect(10, 10, 200, 50), stringToEdit, 1556);
    }


    public void PassCodePhrase()
    {

    }
}
