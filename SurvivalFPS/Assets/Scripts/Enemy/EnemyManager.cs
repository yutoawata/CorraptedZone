using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyObject = null;
    [SerializeField] GameObject stageObject = null;
    [SerializeField] LayerMask obstaclesLayer = ~0;
    // Y軸のスポーン位置
    [SerializeField] float spawnOffsetY = 0.0f;
    [SerializeField] float spawnradius = 0.0f;

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

    // 敵の出現関数
    void SpawnEnemy()
    {
        timer += Time.deltaTime;
        float spawnTime = 0.5f;
        if (timer > spawnTime)
        {
            // 生成位置を格納
            Vector3 pos = SpawnPos();
            // 現在の位置が生成出来るか判別する
            if (IsSpawn(pos))
            {
                Instantiate(
                    enemyObject,
                    pos,
                    Quaternion.identity);

                timer = 0.0f;
            }
            else
            {
                Debug.Log("生成出来なかった");
            }
        }
    }

    // 近くに障害物がなく生成できるか確認する関数
    bool IsSpawn(Vector3 pos)
    {
        // 現在の位置と障害物の距離を測る
        return !Physics.CheckSphere(pos, spawnradius, obstaclesLayer);
    }

    // 敵の生成位置関数
    Vector3 SpawnPos()
    {
        // stageの大きさを取得する
        Renderer stageRenderer = stageObject.GetComponent<Renderer>();
        Bounds stageBounds = stageRenderer.bounds;
        // X軸の位置を決める
        float randPosX = Random.Range(stageBounds.min.x, stageBounds.max.x);
        // Z軸の位置を決める
        float randPosZ = Random.Range(stageBounds.min.z, stageBounds.max.z);
        // 生成位置
        Vector3 spawnPos = new Vector3(randPosX, stageBounds.max.y + spawnOffsetY, randPosZ);

        return spawnPos;
    }
}