using UnityEngine;

// プレイヤーの“前”に位置させる（ライトの回転は変えない）
public class PlayerLight : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float fixedY = 1.5f;  // ワールドYの固定高さ
    [SerializeField] float forwardDistance = 0.5f; // プレイヤー前方オフセット
    [SerializeField] float sideOffset = 0f;        // 横にずらしたい時に使う（右+ / 左-）
    [SerializeField] bool smooth = false;
    [SerializeField] float followLerp = 12f;

    void LateUpdate()
    {
        // プレイヤーの平面(前と右)を使う
        Vector3 fwd = player.forward; fwd.y = 0f; fwd.Normalize();
        Vector3 right = player.right; right.y = 0f; right.Normalize();

        // Yを固定しプレイヤーの位置+オフセットに合わせる
        Vector3 targetPos = player.position + fwd * forwardDistance + right * sideOffset;
        targetPos.y = fixedY;

        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 1f - Mathf.Exp(-followLerp * Time.deltaTime));
        }
        else
        {
            transform.position = targetPos;
        }
    }
}
