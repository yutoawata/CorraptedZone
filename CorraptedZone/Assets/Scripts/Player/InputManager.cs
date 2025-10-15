using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    static Gamepad gamepad;//接続されているゲームパッド

    //シーンロード時に処理が実行されるように設定
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
   
    //ゲームパッド判定処理
    static void CheckGamePad()
    {
        gamepad = Gamepad.current;
    }

    //右スティックの入力量
    public static Vector2 InputRightStickValue() { return gamepad.rightStick.value; }
    
    //左スティックの入力量
    public static Vector2 InputLeftStickValue() { return gamepad.leftStick.value; }
    
    //右四つボタンの上ボタン入力フラグ
    public static bool IsInputUpButton() { return gamepad.buttonNorth.isPressed; }
    
    //右四つボタンの下ボタン入力フラグ
    public static bool IsInputDownButton() { return gamepad.buttonSouth.isPressed; }
    
    //右四つボタンの右ボタン入力フラグ
    public static bool IsInputRightButton() { return gamepad.buttonEast.isPressed; }
    
    //右四つボタンの左ボタン入力フラグ
    public static bool IsInputLeftButton() { return gamepad.buttonWest.isPressed; }
    
    //右のトリガーボタン(RZ)入力フラグ
    public static bool IsInputRightTrigger() { return gamepad.rightTrigger.isPressed;}
    
    //左のトリガーボタン(LZ)入力フラグ
    public static bool IsInputLeftTrigger() { return gamepad.leftTrigger.isPressed;}
    
    //右のショルダーボタン(R)入力フラグ
    public static bool IsInputRightShoulder() { return gamepad.rightShoulder.isPressed; }
    
    //左のショルダーボタン(L)入力フラグ
    public static bool IsInputLeftShoulder() { return gamepad.leftShoulder.isPressed; }
   
    //スタートボタン入力フラグ
    public static bool IsSelectButton() { return gamepad.selectButton.isPressed;}
}
