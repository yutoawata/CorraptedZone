using UnityEngine;

public class LightHouseController : MonoBehaviour
{
    [SerializeField]private float spotChangeSpan;       //切り替え間隔
    [SerializeField]private Transform[] targetItem;     //アイテムの座標取得に使用
    private int spotNumber;                             //照らすアイテムの番号
    private int spotIndex;                              //アイテム配列の要素数の取得に使用
    private Transform LightHouse;

    // Start is called before the first frame update
    void Start()
    {
        //ターゲットアイテムの要素数の取り出し
        spotIndex = targetItem.Length;
    }

    // Update is called once per frame
    void Update()
    {
        ChengeAngle();
    }

    private void ChengeAngle()
    {
        //表示するスポット＝（経過時間 / 待機時間）％　要素数
        spotNumber = Mathf.FloorToInt(Time.time / spotChangeSpan) % spotIndex;
        //指定されたアイテムにライトの中心を合わせる
        transform.LookAt(targetItem[spotNumber]);
    }

    public Vector3 ReturnEnemyTargetTransform()
    {
        LightHouse = this.transform;                                //トランスフォームの値の取得に使用
        Vector3 enemyTargetAngle = LightHouse.eulerAngles;          //角度の取得に使用
        Vector3 enemyTargetPos = LightHouse.transform.position;     //返す座標の保存に使用

        float offsetRange = Mathf.Tan(enemyTargetAngle.x * enemyTargetPos.y);   //xz軸のベクトルの取得
        enemyTargetPos.x = Mathf.Cos(offsetRange * enemyTargetAngle.y);         //x座標の計算
        enemyTargetPos.z = Mathf.Sin(offsetRange * enemyTargetAngle.y);         //z座標の計算

        return enemyTargetPos;                                      //座標を返す
    }
}
