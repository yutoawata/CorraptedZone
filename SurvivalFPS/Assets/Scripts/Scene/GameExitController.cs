using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameExitController : MonoBehaviour
{
    [SerializeField]private GameObject exitCanvas;
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
        if(InputManager.IsSelectButton())
        {
            isOpenPause = true;
        }
        if (InputManager.IsInputRightButton())
        {
            isReturnGame = true;
        }
        if (InputManager.IsInputDownButton())
        {
            isExitGame = true;
        }


        if(isOpenPause)
        {
            Time.timeScale = 0.0f;
            exitCanvas.SetActive(true);

            if(isReturnGame)
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
    }

    void ResetBool()
    {
        isOpenPause = false;
        isReturnGame = false;
        isExitGame = false;
    }
}
