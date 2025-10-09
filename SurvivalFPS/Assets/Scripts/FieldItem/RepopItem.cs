using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RepopItem : MonoBehaviour
{
    [SerializeField] float repopTimer;
    [SerializeField] GameObject child;
    float timer;

    
    void Update()
    {
        if (!child.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= repopTimer)
            {
                child.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            child.SetActive(false);
        }
    }
}
