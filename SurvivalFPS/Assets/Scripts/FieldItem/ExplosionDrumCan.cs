using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExplosionDrumCan : MonoBehaviour, IRayCastHit
{
    [SerializeField] GameObject child;
    ExplosionManager explosionManager;
    bool isActive = true;
    float timer = 0;

    void Awake()
    {
        explosionManager = GameObject.Find("ExplosionManager").GetComponent<ExplosionManager>();
    }


    [Header("”š”­‚Ì‘å‚«‚³")]
    [SerializeField] float explosionMul = 1;


    public void OnRaycastHit(RaycastHit hit)
    {
        if (isActive)
        {
            explosionManager.Generate(child.transform.position.x, child.transform.position.y, child.transform.position.z, explosionMul);
            child.gameObject.SetActive(false);
            isActive = false;
        }
    }

    void Update()
    {
        if (!isActive)
        {
            timer += Time.deltaTime;
            if (timer >= 10)
            {
                isActive = true;
                child.SetActive(true);
                timer = 0;
            }
        }
    }
}
