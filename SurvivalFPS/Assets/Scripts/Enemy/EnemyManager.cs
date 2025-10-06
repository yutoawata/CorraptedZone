using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyObject = null;
    [SerializeField] LayerMask obstaclesLayer = ~0;
    [SerializeField] Vector3 spawnRange = Vector3.zero;
    [SerializeField] float spawnRadius = 0.0f;

    List<GameObject> enemis = new List<GameObject>();

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
            if (IsSpawn(pos) &&
                enemyObject != null)
            {
                GameObject enemy = Instantiate(enemyObject, pos, Quaternion.identity);
                AddEnemyList(enemy);

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
        float distance = 2.0f;
        for (int i = 0; i < enemis.Count; i++)
        {
            // nullチェック
            if (enemis[i] != null)
            {
                // 生成位置と敵の現在位置の距離を格納する
                float dist = Vector3.Distance(pos, enemis[i].transform.position);
                // 敵の距離と近い場合はfalseを返す
                if (dist < distance)
                {
                    return false;
                }
            }
        }
        // 現在の位置と障害物の距離を測る
        return !Physics.CheckSphere(pos, spawnRadius, obstaclesLayer);
    }

    // 敵の生成位置関数
    Vector3 SpawnPos()
    {
        // X軸の位置を決める
        float randPosX = Random.Range(-spawnRange.x, spawnRange.x);
        // Z軸の位置を決める
        float randPosZ = Random.Range(-spawnRange.z, spawnRange.z);
        // 生成位置
        Vector3 spawnPos = new Vector3(randPosX, spawnRange.y, randPosZ);

        return spawnPos;
    }

    // Listに追加する用の関数
    void AddEnemyList(GameObject enemy)
    {
        enemis.Add(enemy);
    }

    // Listから消す用の関数
    public void RemoveEnemyList(GameObject enemy)
    {
        enemis.Remove(enemy);
    }
}