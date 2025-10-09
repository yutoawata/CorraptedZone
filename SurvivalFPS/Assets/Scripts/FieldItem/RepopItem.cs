using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RepopItem : MonoBehaviour
{
    [SerializeField] float repopTimer;
    [SerializeField] GameObject child;
    float timer;
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (!child.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= repopTimer)
            {
                child.SetActive(true);
                // boxCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            child.SetActive(false);
            Debug.Log("’Ê‚Á‚½‚æ");
            // boxCollider.enabled = false;
        }
    }
}
