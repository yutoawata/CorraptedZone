using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
    }

    public Vector2 InputRightStickValue() { return gamepad.rightStick.value; }
    public Vector2 InputLeftStickValue() { return gamepad.leftStick.value; }
    public bool IsInputUpButton() { return gamepad.buttonNorth.isPressed; }
    public bool IsInputDownButton() { return gamepad.buttonSouth.isPressed; }
    public bool IsInputRightButton() { return gamepad.buttonEast.isPressed; }
    public bool IsInputLeftButton() { return gamepad.buttonWest.isPressed; }
    public bool IsInputRightTrigger() { return gamepad.rightTrigger.isPressed;}
    public bool IsInputLeftTrigger() { return gamepad.leftTrigger.isPressed;}
    public bool IsInputRightShoulder() { return gamepad.rightShoulder.isPressed; }
    public bool IsInputLeftShoulder() { return gamepad.leftShoulder.isPressed; }
}
