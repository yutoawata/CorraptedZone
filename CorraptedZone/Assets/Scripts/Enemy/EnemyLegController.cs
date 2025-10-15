using System.Collections.Generic;
using UnityEngine;


public class EnemyLegController : MonoBehaviour
{
    [SerializeField] List<Transform> legObjectList;     //ボーンの親オブジェクト配列
    [SerializeField] List<GameObject> targetPositions; //ボーンの座標配列

    List<EnemyLeg> legList = new List<EnemyLeg>();      //足一本単位のボーンのクラス配列

    void Awake()
    {
        for (int i = 0; i < legObjectList.Count; i++)
        {
            legList.Add(new EnemyLeg());
            legList[i].initialized(GetChildren(legObjectList[i], false), targetPositions[i]);
        }
    }

    void Update()
    {

        for (int i = 0; i < legList.Count; i++)
        {
            legList[i].ControlleBone();
        }
    }

    //ボーンのTransformを取得
    List<Transform> GetChildren(Transform parent_, bool include_parent)
    {
        List<Transform> list = new List<Transform>();
        //trueを指定しないと非アクティブなオブジェクトを取得できない
        list.AddRange(parent_.GetComponentsInChildren<Transform>(true));

        //Cyllinderを削除
        list.Remove(list[list.Count - 1]);

        //親オブジェクトを保存した状態で返す
        if (include_parent)
        {
            return list;
        }

        //リストから親オブジェクトを削除
        list.Remove(parent_);


        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].name);
        }

        return list;
    }
}
