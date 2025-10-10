using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyObject = null;
    [SerializeField] GameObject lightObject = null;
    [SerializeField] GameObject spawnPosition = null;
    [SerializeField] LayerMask obstaclesLayer = ~0;
    // ¶¬”ÍˆÍ‚ğŒˆ‚ß‚é•Ï”
    [SerializeField] Vector3 spawnRange = Vector3.zero;
    // “G‚Ì¶¬ŠÔŠu‚Ì•Ï”
    [SerializeField] float spawnRadius = 0.0f;
    [SerializeField] int spawnCount = 0;

    List<GameObject> enemis = new List<GameObject>();
    LightHouseController lightHouseController = null;

    private void Awake()
    {
        lightHouseController = lightObject.GetComponent<LightHouseController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // “G‚ÌoŒ»ŠÖ”
    public void SpawnEnemy()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // ¶¬ˆÊ’u‚ğŠi”[
            Vector3 pos = lightHouseController.ReturnEnemyTargetTransform() + SpawnPos();
            // Œ»İ‚ÌˆÊ’u‚ª¶¬o—ˆ‚é‚©”»•Ê‚·‚é
            if (IsSpawn(pos) &&
                enemyObject != null)
            {
                GameObject enemy = Instantiate(enemyObject, pos, Quaternion.identity);
                AddEnemyList(enemy);
            }
        }
    }

    // ‹ß‚­‚ÉáŠQ•¨‚ª‚È‚­¶¬‚Å‚«‚é‚©Šm”F‚·‚éŠÖ”
    bool IsSpawn(Vector3 pos)
    {
        float distance = 2.0f;
        for (int i = 0; i < enemis.Count; i++)
        {
            // nullƒ`ƒFƒbƒN
            if (enemis[i] != null)
            {
                // ¶¬ˆÊ’u‚Æ“G‚ÌŒ»İˆÊ’u‚Ì‹——£‚ğŠi”[‚·‚é
                float dist = Vector3.Distance(pos, enemis[i].transform.position);
                // “G‚Ì‹——£‚Æ‹ß‚¢ê‡‚Ífalse‚ğ•Ô‚·
                if (dist < distance)
                {
                    return false;
                }
            }
        }
        // Œ»İ‚ÌˆÊ’u‚ÆáŠQ•¨‚Ì‹——£‚ğ‘ª‚é
        return !Physics.CheckSphere(pos, spawnRadius, obstaclesLayer);
    }

    // “G‚Ì¶¬ˆÊ’uŠÖ”
    Vector3 SpawnPos()
    {
        // X²‚ÌˆÊ’u‚ğŒˆ‚ß‚é
        float randPosX = Random.Range(-spawnRange.x, spawnRange.x);
        // Z²‚ÌˆÊ’u‚ğŒˆ‚ß‚é
        float randPosZ = Random.Range(-spawnRange.z, spawnRange.z);
        // ¶¬ˆÊ’u
        Vector3 spawnPos = new Vector3(randPosX, spawnRange.y, randPosZ);

        return spawnPos;
    }

    // List‚É’Ç‰Á‚·‚é—p‚ÌŠÖ”
    void AddEnemyList(GameObject enemy)
    {
        enemis.Add(enemy);
    }

    // List‚©‚çÁ‚·—p‚ÌŠÖ”
    public void RemoveEnemyList(GameObject enemy)
    {
        enemis.Remove(enemy);
    }

    private void OnDrawGizmos()
    {
        // Fw’è
        Gizmos.color = Color.red;
        Vector3 center = Vector3.zero;
        if (spawnPosition != null)
        {
            center = spawnPosition.transform.position;
        }
        // •`‰æ‚Ì‘å‚«‚³‚ğ2”{‚É‚·‚é
        Vector3 size = new Vector3(spawnRange.x * 2, spawnRange.y, spawnRange.z * 2);
        Gizmos.DrawWireCube(center, size);
    }
}