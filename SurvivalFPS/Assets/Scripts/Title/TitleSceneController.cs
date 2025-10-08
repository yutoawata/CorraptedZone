using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] Image fadeImage = null;

    float fadeAlpha = 0.0f;

    bool isFadeIn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current == null)
        {
            return;
        }
        if (InputManager.IsInputDownButton())
        {
            isFadeIn = true;
        }
        if (isFadeIn)
        {
            FadeIn();
        }
    }

    void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);

        fadeAlpha += Time.deltaTime;

        if (fadeAlpha >= 2.0f)
        {
            fadeAlpha = 2.0f;
            SceneManager.LoadScene("");
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeAlpha);
    }
}
