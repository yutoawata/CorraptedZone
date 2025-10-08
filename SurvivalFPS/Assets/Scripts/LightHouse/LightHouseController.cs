using UnityEngine;

public class LightHouseController : MonoBehaviour
{
    [SerializeField]private float spotChangeSpan;       //切り替え間隔
    [SerializeField]private GameObject child;
    [SerializeField]private GameObject enemyObject;
    [SerializeField]private Transform[] targetItem;     //アイテムの座標取得に使用
    private int targetNumber;             //照らすアイテムの番号
    private int spotIndex;              //アイテム配列の要素数の取得に使用
    private const float OFF_SPAWNANGLE = 10.0f;
    private const float ON_SPAWNANGLE = 5.0f;
    private EnemyManager enemyManager;

    // 追加 スポーンを管理する変数
    private bool hasSpawned;

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
        targetNumber = Mathf.FloorToInt(Time.time / spotChangeSpan) % spotIndex;

        // ==== 1) 親（台座）は水平だけ回す ====
        Vector3 flatTarget = ReturnEnemyTargetTransform();
        flatTarget.y = transform.position.y; // 高さを無視
        transform.LookAt(flatTarget, Vector3.up);

        // ==== 2) 子（砲身）は上下だけ回す ====
        // 親のローカル空間に変換したターゲット方向
        Vector3 localDir = transform.InverseTransformPoint(ReturnEnemyTargetTransform());
        Debug.Log(localDir);

        // 前方向(Z)に対する仰角を計算
        float pitch = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;


        // 子はローカルX回転だけ動かす
        child.transform.localRotation = Quaternion.Euler(0, 90, -pitch);

        //位置座標から角度に変換
        Quaternion rotation = Quaternion.LookRotation(flatTarget, Vector3.right);

        // 追加 現在の角度と目標回転角の差を格納
        float angleDeff = Quaternion.Angle(transform.rotation, rotation);

        // 追加 一定角度内を向くと敵が生成されるようになる マジックナンバーは角度
        if (angleDeff < ON_SPAWNANGLE && !hasSpawned)
        {
            enemyManager.SpawnEnemy();
            hasSpawned = true;
            //Debug.Log(hasSpawned);
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