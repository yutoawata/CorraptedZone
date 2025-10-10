using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] Text timeText = null;

    float elapsedTime = 0f;
    bool isRunning = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = "ê∂ë∂éûä‘ : " + minutes + ":" + seconds.ToString("00");
    }

    public void StartTimer()
    {
        elapsedTime = 0.0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
