using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public bool GetInput(KeyCode code)
    {
        return Input.GetKeyDown(code);
    }

    public bool GetMouseInput(int code)
    {
        return Input.GetMouseButtonDown(code);
    }
}
