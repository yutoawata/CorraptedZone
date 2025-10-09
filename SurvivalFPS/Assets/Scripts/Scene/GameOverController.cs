using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    bool isReturnTitle;
    bool isRestartGame;



    // Start is called before the first frame update
    void Start()
    {
        isReturnTitle = false;
        isRestartGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.IsInputUpButton())
        {
            isReturnTitle = true;
        }
        if (InputManager.IsInputLeftButton())
        {
            isRestartGame = true;
        }

        if(isReturnTitle)
        {
            SceneManager.LoadScene("TitleScene");
        }
        if (isRestartGame)
        {
            SceneManager.LoadScene("Tamura");
        }
    }
}
