using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyObject = null;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float spawnTime = 3.0f;
        // 3•bŒo‰ß‚ÅoŒ»
        if (timer > spawnTime)
        {
            Instantiate(
                enemyObject,
                transform.position,
                Quaternion.identity);

            timer = 0.0f;
        }
    }
}