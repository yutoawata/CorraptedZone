using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Text timeText = null;

    bool isReturnTitle;
    bool isRestartGame;

    TimeController timeController;

    private void Awake()
    {
        timeController = FindAnyObjectByType<TimeController>();
        timeController.StopTimer();
    }

    // Start is called before the first frame update
    void Start()
    {
        float time = timeController.GetElapsedTime();
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timeText.text = "ê∂ë∂éûä‘ : " + minutes + ":" + seconds.ToString("00");

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
