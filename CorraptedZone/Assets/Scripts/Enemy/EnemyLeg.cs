using System.Collections.Generic;
using UnityEngine;

public class EnemyLeg
{
    List<Transform> bones;      //ボーンのTransform配列
    List<Vector3> positions;    //ボーンの座標のコピー配列
    List<float> lengths;        //ボーン間の距離の配列
    GameObject target;          //先端の設置座標
    Vector3 tipPosition;        //先端の座標
    int maxIteration = 0;       //関節数
    float stride = 1.5f;        //歩幅

    //変数初期化処理
    public void initialized(List<Transform> bones_, GameObject target_)
    {
        bones = new List<Transform>();
        bones.AddRange(bones_);
        positions = new List<Vector3>();
        lengths = new List<float>();
        target = target_;
        tipPosition = target_.transform.position;

        // ボーンの長さ
        lengths.Clear();
        for (int i = 0; i < bones.Count - 1; i++)
        {
            lengths.Add(Vector3.Distance(bones[i].transform.position, bones[i + 1].transform.position));
        }

        // ボーンの位置
        positions.Clear();
        for (int i = 0; i < bones.Count; i++)
        {
            positions.Add(bones[i].transform.position);
        }
        maxIteration = bones.Count - 2;
    }

    //歩行挙動処理
    public void ControlleBone()
    {
        // 現在のボーン位置をコピーしてくる
        for (int i = 0; i < bones.Count; i++)
        {
            positions[i] = bones[i].transform.position;
        }

        // FABRIKでボーン位置を推定
        Vector3 basePosition = positions[0];
        float prevDistance = 0.0f;

        if (Vector3.Distance(positions[positions.Count - 1], tipPosition) >= stride)
        {
            tipPosition = target.transform.position;
        }

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

            //先端から付け根にかけての関節処理
            positions[positions.Count - 1] = tipPosition;
            for (int i = positions.Count - 1; i >= 1; i--)
            {
                positions[i - 1] = GetPosition(positions[i], positions[i - 1], lengths[i - 1], i);
            }

            // 付け根から先端にかけた関節処理
            positions[0] = basePosition;
            for (int i = 0; i <= positions.Count - 2; i++)
            {
                positions[i + 1] = GetPosition(positions[i], positions[i + 1], lengths[i], i);
            }
        }

        // 推定したボーン位置から回転角を計算
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector3 origin = bones[i].transform.position;
            Vector3 current = bones[i + 1].transform.position;
            Vector3 target = positions[i + 1];
            Quaternion delta = GetDeltaRotation(origin, current, target);
            bones[i].transform.rotation = delta * bones[i].transform.rotation;
        }
    }

    Vector3 GetPosition(Vector3 current_, Vector3 next_, float length_, int num_)
    {
        Vector3 direction = (current_ - next_).normalized;
        Vector3 setPosition = current_ - direction * length_;
        setPosition.y += Mathf.Sin((Mathf.PI / positions.Count) * num_) / maxIteration;

        return setPosition;
    }

    //回転角の差を算出
    Quaternion GetDeltaRotation(Vector3 origin, Vector3 current, Vector3 target)
    {
        Vector3 beforeDirection = (current - origin).normalized;
        Vector3 afterDirection = (target - origin).normalized;
        return Quaternion.FromToRotation(beforeDirection, afterDirection);
    }
}
