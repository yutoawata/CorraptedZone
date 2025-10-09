using UnityEngine;

public class LightEmissionController : MonoBehaviour
{
    [Header("プレイヤーのライトかどうか判別する")]
    [SerializeField] bool isPlayer = false;

    [Header("灯台の回復範囲の判定")]
    [SerializeField] SphereCollider sphereCollider = null;
    [SerializeField] SphereCollider sphereBarrier = null;
    [SerializeField] SphereCollider sphereBarrier2 = null;

    [Header("燃料設定")]
    [SerializeField] float fuelMax = 100f;         // 最大燃料
    [SerializeField] float startFuel = 100f;       // 開始時の燃料
    [SerializeField] float minutesToEmpty = 5f;    // 何分で空にするか（自動で毎秒の消費量を計算）

    [Header("ライト参照")]
    [SerializeField] Light lightingPoint;
    [SerializeField] Light lightingSpot;

    [Header("満タン/空の時のパラメータ")]
    [SerializeField] float lightHouseIntensityAtFullP = 6f;
    [SerializeField] float lightHouseIntensityAtEmptyP = 0f;
    [SerializeField] float lightHouseIntensityAtFullS = 6f;
    [SerializeField] float lightHouseIntensityAtEmptyS = 0f;
    [SerializeField] float lightHouseTrrigerRadiusFull = 11.69f;
    [SerializeField] float lightHouseTrrigerRadiusEmppty = 0.1f; // 灯台周囲を照らす明かり(プレイヤーの燃料回復範囲)の判定も小さくする
    [SerializeField] float lightHouseBarrierRadiusFull = 0.1f; 
    [SerializeField] float lightHouseBarrierRadiusEmppty = 0.1f; 
    [SerializeField] float lightHouseBarrier2RadiusFull = 0.1f; 
    [SerializeField] float lightHouseBarrier2RadiusEmpty = 0.1f;
    [SerializeField] float lightHouseSpotAngleAtFull = 80f;
    [SerializeField] float lightHouseSpotAngleAtEmpty = 1f;
    [SerializeField] float lightHousePointRangeAtFull = 17.7f;
    [SerializeField] float lightHousePointRangeAtEmpty = 1f;
    [SerializeField] float playerIntensityAtFull = 0.96f;
    [SerializeField] float playerIntensityAtEmpty = 0f;
    [SerializeField] float playerPointAngleAtFull = 17.7f;
    [SerializeField] float playerPointAngleAtEmpty = 1f;

    [Header("減り方の調整（0..1 を 0..1 に変形）")]
    // S字は後半で急に減衰する。線形はゆっくり減衰する
    [SerializeField] AnimationCurve response = AnimationCurve.Linear(0, 0, 1, 1);

    float fuel;            // 現在燃料
    float burnPerSecond;   // 1秒あたりの燃料消費量
    bool isRegenerateing = false; // 回復中かどうか

    public float Fuel { get => fuel; }
    public bool IsRegenerateing { get => isRegenerateing; }

    void Awake()
    {
        fuel = Mathf.Clamp(startFuel, 0f, fuelMax);

        float durationSec = Mathf.Max(0.001f, minutesToEmpty * 60f);
        burnPerSecond = fuelMax / durationSec;

        ApplyLightFromFuel(); // 初期反映
    }

    void Update()
    {
        if (!isRegenerateing)
        {
            // 燃料を消費
            fuel = Mathf.Max(0f, fuel - burnPerSecond * Time.deltaTime);

            ApplyLightFromFuel();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetFuelRatio(200);
        }
    }

    void ApplyLightFromFuel()
    {
        if (!lightingPoint) return;

        // 0..1 の燃料割合
        float frac = (fuelMax <= 0f) ? 0f : fuel / fuelMax;

        // 減り具合をcurveで調整する
        float t = Mathf.Clamp01(response.Evaluate(frac));

        // 補間して反映
        if (!isPlayer)
        {
            lightingPoint.intensity = Mathf.Lerp(lightHouseIntensityAtEmptyP, lightHouseIntensityAtFullP, t);
            lightingSpot.intensity = Mathf.Lerp(lightHouseIntensityAtEmptyS, lightHouseIntensityAtFullS, t);
            lightingPoint.range = Mathf.Lerp(lightHousePointRangeAtEmpty, lightHousePointRangeAtFull, t);
            lightingSpot.spotAngle = Mathf.Lerp(lightHouseSpotAngleAtEmpty, lightHouseSpotAngleAtFull, t);
            sphereCollider.radius = Mathf.Lerp(lightHouseTrrigerRadiusEmppty, lightHouseTrrigerRadiusFull, t);
            sphereBarrier.radius = Mathf.Lerp(lightHouseBarrierRadiusEmppty, lightHouseBarrierRadiusFull, t);
            sphereBarrier2.radius = Mathf.Lerp(lightHouseBarrier2RadiusEmpty, lightHouseBarrier2RadiusFull, t);
        }
        else
        {
            lightingPoint.intensity = Mathf.Lerp(playerIntensityAtEmpty, playerIntensityAtFull, t);
            lightingPoint.range = Mathf.Lerp(playerPointAngleAtEmpty, playerPointAngleAtFull, t);
        }
    }

    // 外部から補給はこれを呼ぶ
    public void AddFuel(float amount)
    {
        fuel = Mathf.Clamp(fuel + amount, 0f, fuelMax);
        ApplyLightFromFuel();
    }

    // 外部から回復中かどうかのフラグを出す
    public void SetRegenerateing(bool tf)
    {
        isRegenerateing = tf;
    }

    // デバッグ用に現在の燃料を直接セットしたい場合
    public void SetFuelRatio(float ratio01)
    {
        fuel = Mathf.Clamp01(ratio01) * fuelMax;
        ApplyLightFromFuel();
    }

    // エディタで値を弄った時に下限・上限を整える
    void OnValidate()
    {
        fuelMax = Mathf.Max(0.0001f, fuelMax);
        minutesToEmpty = Mathf.Max(0.0001f, minutesToEmpty);
        lightHouseSpotAngleAtEmpty = Mathf.Max(1f, lightHouseSpotAngleAtEmpty);
        lightHousePointRangeAtEmpty = Mathf.Max(1f, lightHousePointRangeAtEmpty);
    }
}
