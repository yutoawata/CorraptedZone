using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    GameObject manager = null;
    EnemyManager enemyManager = null;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("EnemyManager");
        enemyManager = manager.GetComponent<EnemyManager>();
        enemyManager.RegistrationEnemyList(gameObject);
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
