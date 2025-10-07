using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSeach : MonoBehaviour
{
    bool isEnd; // 爆発が終了したかをサーチする

    public bool IsEnd { get => isEnd;}

    private void OnParticleSystemStopped()
    {
        isEnd = true;
    }
}
