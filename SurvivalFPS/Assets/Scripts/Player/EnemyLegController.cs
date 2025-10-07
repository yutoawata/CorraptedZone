using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyLegController : MonoBehaviour
{

    [SerializeField] List<Transform> bones = new List<Transform>(); //ボーンの座標配列
    [SerializeField] Transform targetPosition;                      //先端が向かう先の座標
    [SerializeField] Transform body;                                //体の座標

    struct Leg
    {
        List<Transform> bones;
        List<Vector3> positions;
        List<float> lengths;
        Vector3 tipPositon;
        Leg(List<Transform> bones_)
        {
            bones = new List<Transform>();
            bones.AddRange(bones_);
            positions = new List<Vector3>();
            lengths = new List<float>();
            tipPositon = new Vector3();
        }
       
    }


    List<float> lengths = new List<float>();        //ボーン間の距離
    List<Vector3> positions = new List<Vector3>();  //ボーンの座標配列(コピー)
    Vector3 tipPosition = Vector3.zero;             //付け根の座標
    int maxIteration = 0;                           //関節数


    void Awake()
    {
        InitializeBones();
        Debug.Log(targetPosition.name);
    }

    void Update()
    {
        if (bones.Count <= 0 )
        {
            InitializeBones();
            Debug.Log("配列エラー");
            Debug.Log(gameObject.name);
        }

        if((targetPosition.position - tipPosition).magnitude >= 1.5f)
        {
            tipPosition = targetPosition.position; //移動先の座標
        }
        

        ControlleBone();
    }

    List<Transform> GetChildren(Transform parent_, bool include_parent)
    {
        List<Transform> list= new List<Transform>();
        //trueを指定しないと非アクティブなオブジェクトを取得できない
        list.AddRange(parent_.GetComponentsInChildren<Transform>(true));

        //親オブジェクトを保存した状態で返す
        if (include_parent)
        {
            return list;
        }

        //リストから親オブジェクトを削除
        list.Remove(parent_);

        return list;
    }

    void ControlleBone()
    {
        // 現在のボーン位置をコピーしてくる
        for (int i = 0; i < bones.Count; i++)
        {
            positions[i] = bones[i].position;
        }

        // FABRIKでボーン位置を推定
        Vector3 basePosition = positions[0];
        float prevDistance = 0.0f;

        //関節ボーンの処理
        for (int iter = 0; iter < maxIteration; iter++)
        {
            // 収束チェック
            float distance = Vector3.Distance(positions[positions.Count - 1], tipPosition);
            float change = Mathf.Abs(distance - prevDistance);
            prevDistance = distance;

            //非常に小さい値なら処理を抜ける
            if (distance < 1e-6 || change < 1e-8)
            {
                break;
            }

            // Backward
            positions[positions.Count - 1] = tipPosition;

            //先端から付け根にかけての関節処理
            for (int i = positions.Count - 1; i >= 1; i--)
            {
                Vector3 direction = (positions[i] - positions[i - 1]).normalized;
                Vector3 setPosition = positions[i] - direction * lengths[i - 1];
                setPosition.y += Mathf.Sin((Mathf.PI / positions.Count) * i) / positions.Count;

                positions[i - 1] = setPosition;
            }

            float allLength = 0;
            for(int i = 0; i < lengths.Count; i++)
            {
                allLength += lengths[i];
            }

            // 付け根から先端にかけた関節処理
            positions[0] = basePosition;
            for (int i = 0; i <= positions.Count - 2; i++)
            {
                Vector3 direction = (positions[i + 1] - positions[i]).normalized;
                Vector3 setPosition = positions[i] + direction * lengths[i];
                setPosition.y += Mathf.Sin((Mathf.PI / positions.Count) * i) / positions.Count;

                positions[i + 1] = setPosition;
            }
        }

        // 推定したボーン位置から回転角を計算
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector3 origin = bones[i].position;
            Vector3 current = bones[i + 1].position;
            Vector3 target = positions[i + 1];
            Quaternion delta = GetDeltaRotation(origin, current, target);
            bones[i].rotation = delta * bones[i].rotation;
        }
    }

    //必要情報の初期化
    void InitializeBones()
    {
        tipPosition = targetPosition.position;
        //関節数を先端と付け根を抜いた数にする
        maxIteration = bones.Count - 2;

        // ボーンの長さ
        lengths.Clear();
        for (int i = 0; i < bones.Count - 1; i++)
        {
            lengths.Add(Vector3.Distance(bones[i].position, bones[i + 1].position));
        }

        // ボーンの位置
        positions.Clear();
        for (int i = 0; i < bones.Count; i++)
        {
            positions.Add(bones[i].position);
        }

        if (bones.Count == 0)
        {
            Debug.Log(gameObject.name);
        }
    }

    //回転角の差を算出
    Quaternion GetDeltaRotation(Vector3 origin, Vector3 current, Vector3 target)
    {
        Vector3 beforeDirection = (current - origin).normalized;
        Vector3 afterDirection = (target - origin).normalized;
        return Quaternion.FromToRotation(beforeDirection, afterDirection);
    }
}
