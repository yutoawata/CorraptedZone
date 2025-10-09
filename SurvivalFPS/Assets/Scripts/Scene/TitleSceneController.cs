using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(InputManager.IsInputDownButton())
        {
            SceneManager.LoadScene("Tamura");
        }
        if (InputManager.IsInputRightButton())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
