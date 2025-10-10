using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    static Gamepad gamepad;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    // Start is called before the first frame update
    static void CheckGamePad()
    {
        gamepad = Gamepad.current;
    }

    public static Vector2 InputRightStickValue() { return gamepad.rightStick.value; }
    public static Vector2 InputLeftStickValue() { return gamepad.leftStick.value; }
    public static bool IsInputUpButton() { return gamepad.buttonNorth.isPressed; }
    public static bool IsInputDownButton() { return gamepad.buttonSouth.isPressed; }
    public static bool IsInputRightButton() { return gamepad.buttonEast.isPressed; }
    public static bool IsInputLeftButton() { return gamepad.buttonWest.isPressed; }
    public static bool IsInputRightTrigger() { return gamepad.rightTrigger.isPressed;}
    public static bool IsInputLeftTrigger() { return gamepad.leftTrigger.isPressed;}
    public static bool IsInputRightShoulder() { return gamepad.rightShoulder.isPressed; }
    public static bool IsInputLeftShoulder() { return gamepad.leftShoulder.isPressed; }
    public static bool IsSelectButton() { return gamepad.selectButton.isPressed;}
}
