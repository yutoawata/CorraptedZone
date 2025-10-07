using UnityEngine;

public class LightHouseController : MonoBehaviour
{
    [SerializeField]private float spotChangeSpan;       //切り替え間隔
    [SerializeField]private float rotationSpeed;
    [SerializeField]private Transform[] targetItem;     //アイテムの座標取得に使用
    [SerializeField]private int spotNumber;             //照らすアイテムの番号
    [SerializeField]private int spotIndex;              //アイテム配列の要素数の取得に使用


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
        // ターゲット方向のベクトルを取得
        Vector3 relativePos = ReturnEnemyTargetTransform() - this.transform.position;
        //回転軸の固定(横にだけ動かすため)
        relativePos.x -= 90;
        relativePos.y -= 90;
        //位置座標から角度に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, rotationSpeed);
    }

    public Vector3 ReturnEnemyTargetTransform()
    {
        Vector3 enemyTargetPos = targetItem[spotNumber].transform.position;     //返す座標の保存に使用
        return enemyTargetPos;                                      //座標を返す
    }
}