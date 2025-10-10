using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// このスクリプトから爆発を生成する
public class ExplosionManager : MonoBehaviour
{
    [SerializeField] GameObject explosion;


    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void Generate(float x, float y, float z, float mul = 1)
    {
        // 爆発を生成しつつスクリプトを取得
        var explosionScr = Instantiate(explosion, new Vector3(x, y, z), Quaternion.identity).GetComponent<ExplosionLight>();
        Debug.Log("爆発したよ");
        explosionScr.StartExplosion(mul);
    }
}
