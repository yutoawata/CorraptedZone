using UnityEngine;
using System.Collections.Generic;

public class EnemyLegController : MonoBehaviour
{
    [SerializeField] Transform[] bones;      // 足のボーン（根本から先端まで）
    [SerializeField] float walkSpeed = 2f;   // 歩行リズム
    [SerializeField] float walkAmplitude = 30f; // 前後に振る角度
    [SerializeField] float waveLength = 0.5f;   // 節ごとの位相ずれ

    [SerializeField] float randomAmplitude = 10f; // ランダム揺らぎの強さ
    [SerializeField] float randomSpeed = 1.5f;    // 揺らぎの速さ

    SkinnedMeshRenderer renderer;
    float walkAngle = 0.0f;
    List<Transform> basePositionList = new List<Transform>();
    int num = 0;
    private void Start()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();

        if(renderer != null)
        {
            for (int i = 0; i < renderer.bones.Length; i++)
            {
                bones = renderer.bones;
            }
        }

        for (int i = 0; i < bones.Length; i++)
        {
            //basePositionList.Add(bones[i]);
        }
    }


    void Update()
    {
        float time = Time.time;


        for (int i = 0; i < bones.Length; i++)
        {
            // ---- 歩行の基本サイン波 ----
            float walkAngle = Mathf.Sin(time * walkSpeed + i * waveLength) * walkAmplitude;

            // ---- ランダムな揺らぎ（PerlinNoise使用）----
            float noise = (Mathf.PerlinNoise(i * 0.3f, time * randomSpeed) - 0.5f) * 2f;
            float randomAngle = noise * randomAmplitude;

            // ---- 合成して回転 ----
           // bones[i].localRotation = Quaternion.Euler(walkAngle + randomAngle, 0, 0);
            bones[i].localPosition = new Vector3(1, Mathf.Sin(walkAngle + randomAngle), 0.0f);
        }
        if (num == bones.Length)
        {
            num = 0;
        }
    }
}
