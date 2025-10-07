using UnityEngine;

public class LightHouseRotation : MonoBehaviour
{
    [SerializeField] LightHouseController lightHouseController;
    [SerializeField]private float spotChangeSpan;       //切り替え間隔
    [SerializeField]private float rotationSpeed;


    void Update()
    {
        ChengeAngle();
    }

    private void ChengeAngle()
    {
        // ターゲット方向のベクトルを取得
        Vector3 relativePos = lightHouseController.ReturnEnemyTargetTransform() - this.transform.position;
        //回転軸の固定(縦にだけ動かすため)
        relativePos.x = 0;
        relativePos.z = 0;
        //位置座標から角度に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        //オブジェクトの角度のずれの修正
        rotation.y = rotation.x;
        rotation.x = 0;
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, rotationSpeed);
    }
}
