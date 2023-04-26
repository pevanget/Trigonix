using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UItoCodeHandler : MonoBehaviour
{
    private TextFieldHandler _textFieldHandler;
    // Start is called before the first frame update
    void Start()
    {
        _textFieldHandler = FindObjectOfType<TextFieldHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
