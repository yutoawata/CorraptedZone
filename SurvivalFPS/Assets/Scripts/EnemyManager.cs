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
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        timer += Time.deltaTime;
        float spawnTime = 3.0f;
        // 3秒経過で出現
        if (timer > spawnTime)
        {

            Instantiate(
                enemyObject,
                SpawnPos(),
                Quaternion.identity);

            timer = 0.0f;
        }
    }

    Vector3 SpawnPos()
    {
        // stageの大きさを取得する
        Renderer stageRenderer = stageObject.GetComponent<Renderer>();
        Bounds stageBounds = stageRenderer.bounds;
        // X軸の位置を決める
        float randPosX = Random.Range(stageBounds.min.x, stageBounds.max.x);
        // Z軸の位置を決める
        float randPosZ = Random.Range(stageBounds.min.z, stageBounds.max.z);
        Vector3 spawnPos = new Vector3(randPosX, stageBounds.max.y + spawnOffsetY, randPosZ);

        return spawnPos;
    }
}