using UnityEngine;
using System.Collections.Generic;

public class EnemyLegController : MonoBehaviour
{
    [SerializeField] Transform[] segments; // 足のボーン（順番に入れる）
    [SerializeField] float waveSpeed = 2f; // 波の進む速さ
    [SerializeField] float waveAmplitude = 20f; // 振れ幅（角度）
    [SerializeField] float waveFrequency = 2f;  // 波の間隔

    void Update()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            float angle = Mathf.Sin(Time.time * waveSpeed + i * waveFrequency) * waveAmplitude;
            segments[i].localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
