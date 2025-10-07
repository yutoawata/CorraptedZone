using UnityEngine;

public class LightHouseController : MonoBehaviour
{
    [SerializeField]private float spotChangeSpan;       //切り替え間隔
    [SerializeField]private float rotationSpeed;
    [SerializeField]private Transform[] targetItem;     //アイテムの座標取得に使用
    [SerializeField]private int spotNumber;             //照らすアイテムの番号
    [SerializeField]private int spotIndex;              //アイテム配列の要素数の取得に使用
    [SerializeField]private GameObject enemyObject;

    private EnemyManager enemyManager;

    // 追加 スポーンを管理する変数
    bool hasSpawned;

    private void Awake()
    {
        // 追加
        enemyManager = enemyObject.GetComponent<EnemyManager>();
    }

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
        // 追加 ターゲット方向の回転情報を格納
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        //回転軸の固定(横にだけ動かすため)
        relativePos.y = 0;
        //位置座標から角度に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, rotationSpeed);

        // 追加 現在の角度と目標回転角の差を格納
        float angleDeff = Quaternion.Angle(transform.rotation, targetRotation);

        // 追加 一定角度内を向くと敵が生成されるようになる マジックナンバーは角度
        if (angleDeff < 5.0f && !hasSpawned)
        {
            enemyManager.SpawnEnemy();
            hasSpawned = true;
            Debug.Log(hasSpawned);
        }

        // 追加 一定角度から離れると生成が出来る状態に戻す
        if (angleDeff > 10.0f)
        {
            hasSpawned = false;
        }
    }

    public Vector3 ReturnEnemyTargetTransform()
    {
        Vector3 enemyTargetPos = targetItem[spotNumber].transform.position;     //返す座標の保存に使用
        return enemyTargetPos;                                      //座標を返す
    }
}