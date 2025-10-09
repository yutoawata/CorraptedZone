using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    public void EndGame(bool isClear)
    {
        if (isClear)
        {
            // ロードするシーン
            SceneManager.LoadScene("");
        }
        else
        {
            // ロードするシーン
            SceneManager.LoadScene("");
        }
    }
}