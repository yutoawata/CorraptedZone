using UnityEngine;

public class ExplosionLight : MonoBehaviour
{
    [Header("爆発本体用")]
    [SerializeField] ExplosionSeach seach; // 爆発が終わったかどうかを検知する

    [Header("爆発ライト")]
    [SerializeField] float amplitude = 8.0f;     // 最大強度
    [SerializeField] float lightDuration = 0.25f; // 光る総時間（秒
    [SerializeField] Light pointLight;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] ParticleSystem mainExplosion; //メイン(炎)
    [SerializeField] ParticleSystem subExplosion; //　サブ(煙)

    bool isLight;         // ライト点灯中フラグ
    bool isEnd;
    float startTime;      // この発光の開始Time

    public bool IsEnd { get => isEnd; set => isEnd = value; }

    void Awake()
    {
        if (pointLight) pointLight.intensity = 0f;
    }

    void Update()
    {
        SettingLight();
        if(seach.IsEnd) Destroy(this.gameObject);
    }

    public void StartExplosion(float sizeMul = 1)
    {
        mainExplosion.transform.localScale *= sizeMul;
        subExplosion.transform.localScale *= sizeMul;
        pointLight.range *= sizeMul;
        sphereCollider.radius *= sizeMul;
        if (mainExplosion) mainExplosion.Play();

        
        isLight = true;
        startTime = Time.time;    // この時点を基準にローカルタイマー開始
    }

    void SettingLight()
    {
        if (!pointLight) return;

        if (isLight)
        {
            // 経過0..duration を 0..1 に正規化
            float t = (Time.time - startTime) / lightDuration;
            if (t >= 1f)
            {
                // 1回分の発光が終了
                isLight = false;
                pointLight.intensity = 0f;
            }
            else
            {
                // 半サイン波：0→π（常に非負）で“ピカッ→消える”
                float halfSine = Mathf.Sin(Mathf.PI * t); // 0→1→0
                pointLight.intensity = amplitude * halfSine;
            }
        }
        else
        {
            pointLight.intensity = 0f;
        }
    }
}
