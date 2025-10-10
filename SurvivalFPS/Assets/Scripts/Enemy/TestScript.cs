using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    EnemyManager enemyManager = null;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindAnyObjectByType<EnemyManager>();
        enemyManager.GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        //float destroyTime = 3.0f;
        //if (timer > destroyTime)
        //{
        //    enemyManager.RemoveEnemyList(gameObject);
        //    Destroy(gameObject);
        //}
    }
}
