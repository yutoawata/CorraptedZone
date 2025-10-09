using UnityEngine;

public class LightHouseController : MonoBehaviour
{
    [SerializeField]private float spotChangeSpan;       //切り替え間隔
    [SerializeField]private GameObject child;
    [SerializeField]private EnemyManager enemyManager;
    [SerializeField]private Transform[] targetItem;     //アイテムの座標取得に使用
    private int targetNumber;             //照らすアイテムの番号
    private int spotIndex;              //アイテム配列の要素数の取得に使用
    [SerializeField] private float OFF_SPAWNANGLE = 200.0f;
    [SerializeField] private float ON_SPAWNANGLE = 100.0f;
    float adjY = 83.6f;
    float adjZ = 1.6f;

    // 追加 スポーンを管理する変数
    private bool hasSpawned;


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
        targetNumber = Mathf.FloorToInt(Time.time / spotChangeSpan) % spotIndex;

        // ==== 1) 親（台座）は水平だけ回す ====
        Vector3 flatTarget = ReturnEnemyTargetTransform();
        flatTarget.y = transform.position.y; // 高さを無視
        transform.LookAt(flatTarget, Vector3.up);

        // ==== 2) 子は上下だけ回す ====
        // 親のローカル空間に変換したターゲット方向
        Vector3 localDir = transform.InverseTransformPoint(ReturnEnemyTargetTransform());
        Debug.Log(localDir);

        // 前方向(Z)に対する仰角を計算
        float pitch = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;


        // 子はローカルX回転だけ動かす
        child.transform.localRotation = Quaternion.Euler(0, adjY, -pitch + adjZ);

        //位置座標から角度に変換
        Quaternion rotation = Quaternion.LookRotation(flatTarget, Vector3.right);

        // 追加 現在の角度と目標回転角の差を格納
        float angleDeff = Quaternion.Angle(transform.rotation, rotation);

        Debug.Log(angleDeff);
        // 追加 一定角度内を向くと敵が生成されるようになる マジックナンバーは角度
        if (angleDeff < ON_SPAWNANGLE && !hasSpawned)
        {
            enemyManager.SpawnEnemy();
            hasSpawned = true;
            Debug.Log("通ったよ");
        }

        // 追加 一定角度から離れると生成が出来る状態に戻す
        if (angleDeff > OFF_SPAWNANGLE)
        {
            hasSpawned = false;
        }
    }



    public Vector3 ReturnEnemyTargetTransform()
    {
        Vector3 enemyTargetPos = targetItem[targetNumber].transform.position;     //返す座標の保存に使用
        return enemyTargetPos;                                      //座標を返す
    }
}