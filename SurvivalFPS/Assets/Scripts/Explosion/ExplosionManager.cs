using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// このスクリプトから爆発を生成する
public class ExplosionManager : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject testCube;


    [Header("テスト用")]
    [SerializeField] float amplitude = 1.0f;   // A 振幅
    [SerializeField] float frequency = 1.0f;   // f 周波数 [Hz]
    [SerializeField] float offsetY = 0.5f;     // y0 平行移動
    [SerializeField] float moveSpeed = 0.0f;

    float size = 1;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        float y = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time) + offsetY; // 縦
        float x = amplitude * Mathf.Cos(2 * Mathf.PI * frequency * Time.time); //横
        float z = moveSpeed * Time.time;
        testCube.transform.position = new Vector3(x, y, z);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate(testCube.transform.position.x, testCube.transform.position.y, testCube.transform.position.z, size);
            size++;
        }
    }

    public void Generate(float x, float y, float z, float mul = 1)
    {
        // 爆発を生成しつつスクリプトを取得
        var explosionScr = Instantiate(explosion, new Vector3(x, y, z), Quaternion.identity).GetComponent<ExplosionLight>();
        Debug.Log("爆発したよ");
        explosionScr.StartExplosion(mul);
    }
}
