using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyObject = null;
    [SerializeField] GameObject stageObject = null;
    [SerializeField] float spawnOffsetY = 0.0f;

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
            // stage‚Ì‘å‚«‚³‚ðŽæ“¾‚·‚é
            Renderer stageRenderer = stageObject.GetComponent<Renderer>();
            Bounds stageBounds = stageRenderer.bounds;
            float randPosX = Random.Range(stageBounds.min.x, stageBounds.max.x);
            float randPosZ = Random.Range(stageBounds.min.z, stageBounds.max.z);
            Vector3 spawnPos = new Vector3(randPosX, stageBounds.max.y + spawnOffsetY, randPosZ);

            Instantiate(
                enemyObject,
                spawnPos,
                Quaternion.identity);

            timer = 0.0f;
        }
    }
}