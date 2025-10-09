using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject titleText = null;
    [SerializeField] GameObject gameOverText = null;
    [SerializeField] GameObject clearText = null;

    const float WAIT_TIME = 2.0f;

    bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        titleText.SetActive(true);
        gameOverText.SetActive(false);
        clearText.SetActive(false);
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current == null || isStart)
        {
            return;
        }

        if (InputManager.IsInputDownButton() && !isStart)
        {
            GameStrat();
        }
    }

    void GameStrat()
    {
        // 画面を動かす
        Time.timeScale = 1.0f;
        titleText.SetActive(false);
        isStart = true;
        Debug.Log(Time.timeScale);
    }

    public IEnumerator EndGame(bool isClear)
    {
        //画面を止める
        Time.timeScale = 0.0f;
        if (isClear)
        {
            clearText.SetActive(true);
        }
        else
        {
            gameOverText.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(WAIT_TIME);
        // ロードするシーン
        SceneManager.LoadScene("");
    }
}