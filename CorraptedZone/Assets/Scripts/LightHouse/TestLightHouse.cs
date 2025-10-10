using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLightHouse : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject child;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ==== 1) 親（台座）は水平だけ回す ====
        Vector3 flatTarget = target.position;
        flatTarget.y = transform.position.y; // 高さを無視
        transform.LookAt(flatTarget, Vector3.up);

        // ==== 2) 子（砲身）は上下だけ回す ====
        // 親のローカル空間に変換したターゲット方向
        Vector3 localDir = transform.InverseTransformPoint(target.position);

        // 前方向(Z)に対する仰角を計算
        float pitch = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        // 子はローカルX回転だけ動かす
        child.transform.localRotation = Quaternion.Euler(0, 90, -pitch);
    }
}
