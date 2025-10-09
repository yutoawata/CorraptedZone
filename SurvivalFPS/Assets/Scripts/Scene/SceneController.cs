using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject exitCanvas;
    [SerializeField] private LightEmissionController lightEmissionController;
    bool isOpenPause;
    bool isReturnGame;
    bool isExitGame;



    // Start is called before the first frame update
    void Start()
    {
        exitCanvas.SetActive(false);
        ResetBool();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (InputManager.IsSelectButton())
            {
                isOpenPause = true;
            }
            if (InputManager.IsInputRightButton() && isOpenPause)
            {
                isExitGame = true;
            }
            if (InputManager.IsInputDownButton() && isOpenPause)
            {
                isReturnGame = true;
            }
        }


        if (isOpenPause)
        {
            Time.timeScale = 0.0f;
            exitCanvas.SetActive(true);

            if (isReturnGame)
            {
                Time.timeScale = 1.0f;
                isOpenPause = false;
                isReturnGame = false;
                exitCanvas.SetActive(false);
                Debug.Log("ゲーム続行");
            }

            if (isExitGame)
            {
                Debug.Log("ゲーム終了");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                Application.Quit();//ゲームプレイ終了
#endif
            }
        }

        if (lightEmissionController.Fuel >= 0.0f)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    void ResetBool()
    {
        isOpenPause = false;
        isReturnGame = false;
        isExitGame = false;
    }
}