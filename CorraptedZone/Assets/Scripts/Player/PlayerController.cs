using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject reloadGun = null;                           //リロードモーション用の銃モデル
    [SerializeField] GameObject shootGun = null;                            //射撃モーション用の銃モデル
    [SerializeField] GameObject muzzleFlash = null;                         //マズルフラッシュ
    [SerializeField] GameObject reticle = null;                             //レティクル
    [SerializeField] Camera mainCamera = null;                              //ゲームの視点となるカメラ
    [SerializeField] LightEmissionController lightEmissionController = null;//プレイヤーの光源制御スクリプト
    [SerializeField] MeshRenderer shootGunRenderer = null;                  //射撃用オブジェクトのRenderer
    [SerializeField] float shootLenge = 20.0f;                              //射程距離
    [SerializeField] float fireIngerval = 1.0f;                             //次の射撃までの間隔
    [SerializeField] float moveSpeed = 20.0f;                               //移動速度
    [SerializeField] float rotateSpeed = 300.0f;                            //振り向き速度
    [SerializeField] float recoveryFuelValue = 40.0f;                       //燃料の回復量

    const int MAX_FIRE_VALUE = 2;   //リロード無しでの射撃回数
    const int MAX_AMMO_VALUE = 100; //所持弾薬数の上限

    Rigidbody rb;                                           //自身にアタッチされているRigidbody
    Animator shootAnim;                                     //射撃用のショットガンオブジェクトのAnimator
    Animator reloadAnim;                                    //リロード用のショットガンオブジェクトのAnimator
    Vector3 ADSPosition = new Vector3(0.0f, 0.393f, 0.25f); //覗き込み時の銃の位置
    Vector3 hipPosition = new Vector3(0.2f, 0.35f, 0.45f);  //腰だめ時の銃の位置
    Vector3 moveDirection = Vector2.zero;                   //移動方向
    Vector2 rotate = Vector2.zero;                          //振り向き角度
    float cameraViewMax = 60.0f;                            //カメラのズームアウト上限値(腰だめ時の値)
    float cameraViewMin = 45.0f;                            //カメラのズームイン下限値(覗き込み時の値)
    float ADSSpeed = 5.0f;                                  //覗き込みと腰だめの切り替え時の銃の移動速度
    float currentSpeed = 0.0f;                              //現在の移動速度
    float timer = 0.0f;                                     //射撃インターバルの計測タイマー
    int remainingAmmoValue = 0;                             //射撃可能数(重心内の弾の数)
    int fuelValue = 0;                                      //所持している燃料アイテムの数
    int ammoValue = 10;                                     //所持弾薬数
    bool isReloading = false;                               //リロードフラグ
    bool isInteract = false;                                //インタラクトフラグ

    public int FuelValue { get => fuelValue; }
    public int RemainingAmmoValue { get => remainingAmmoValue; }
    public int AmmoValue { get => ammoValue; }

    // Start is called before the first frame update
    void Start()
    {
        shootAnim = shootGun.GetComponent<Animator>();
        reloadAnim = reloadGun.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
       
        remainingAmmoValue = MAX_FIRE_VALUE;
        timer = fireIngerval;

        muzzleFlash.SetActive(false);
        reloadGun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < fireIngerval)
        {
            timer += Time.deltaTime;
        }

        if (!isInteract)
        {
            if (Time.timeScale > 0.0f)
            {
                Fire();
                ADS();
                ReLoad();
                Move();
            }
           
        }

        if (isInteract)
        {
            if (!InputManager.IsInputRightButton())
            {
                isInteract = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //アイテム取得処理

        if (other.CompareTag("Gasolene"))
        {
            fuelValue++;
        }

        if (other.CompareTag("Ammo"))
        {
            ammoValue++;
            if(ammoValue >= MAX_AMMO_VALUE)
            {
                ammoValue = MAX_AMMO_VALUE;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //発電機でのインタラクト処理
        if(other.CompareTag("Generator"))
        {
            if (InputManager.IsInputRightButton())
            {
                if (!isInteract)
                {
                    //灯台の燃料回復
                    if (fuelValue > 0)
                    {
                        fuelValue--;
                        lightEmissionController.AddFuel(recoveryFuelValue);
                    }
                    isInteract = true;
                }
            }
        }

        
    }

    //移動・回転処理
    void Move()
    {
        /*
            灯台など衝突時にすり抜けないオブジェクトがあるため、TranslateではなくAddForceを使用
            ただし、移動入力がなければ停止してほしいため、初めに移動方向の逆ベクトルを加算し移動量を打ち消す
        */
        rb.AddForce(-rb.velocity);

        moveDirection = InputManager.InputLeftStickValue();
        rotate = InputManager.InputRightStickValue();

        Quaternion rotation = transform.rotation;

        rotation.x = rotation.z = 0.0f;

        moveDirection = rotation * new Vector3(moveDirection.x, 0.0f, moveDirection.y);
        
        //移動処理
        rb.AddForce(moveDirection * currentSpeed);
        
        //回転処理
        transform.eulerAngles += new Vector3(-rotate.y, rotate.x, 0.0f) * rotateSpeed * Time.deltaTime;
    }

    //覗き込み切り替え処理
    void ADS()
    {
        if (isReloading)
        {
            return;
        }

        /*
            ADS時、レティクルは非表示、銃は輪郭線をなくす
         */
        if (InputManager.IsInputLeftTrigger())
        {
            reticle.SetActive(false);
            shootGunRenderer.material.SetFloat("_OutlineWidth", 0.0f);
            
            //ズームイン
            mainCamera.fieldOfView += (cameraViewMin - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;
            
            //移動処理
            Vector3 direction = ADSPosition - shootGun.transform.localPosition;
            shootGun.transform.localPosition += direction * ADSSpeed * Time.deltaTime;
            currentSpeed = moveSpeed / 2.0f;
        }
        else
        {
            reticle.SetActive(true);
            shootGunRenderer.material.SetFloat("_OutlineWidth", 0.001f);
            
            //ズームアウト
            mainCamera.fieldOfView += (cameraViewMax - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;

            //ショットガン移動処理
            Vector3 direction = hipPosition - shootGun.transform.localPosition;
            shootGun.transform.localPosition += direction * ADSSpeed * Time.deltaTime;
            currentSpeed = moveSpeed;
        }
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, cameraViewMin, cameraViewMax);
    }

    //射撃処理
    void Fire()
    {
        if (!shootAnim.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            muzzleFlash.SetActive(false);
        }

        //銃身内に弾が無ければ射撃不可
        if (remainingAmmoValue <= 0)
        {
            return;
        }

        if (InputManager.IsInputRightTrigger())
        {
            //射撃処理
            if (timer >= fireIngerval)
            {
                muzzleFlash.SetActive(true);

                //着弾判定
                if (Physics.Raycast(mainCamera.transform.position, transform.forward, out RaycastHit target, shootLenge))
                {
                    /*
                        射撃相手にOnRaycastHitという関数を保持させ、着弾時の処理を実装することで命中時のギミックを実装
                     */
                    target.collider.gameObject.SendMessage("OnRaycastHit", target, SendMessageOptions.DontRequireReceiver);
                }
                remainingAmmoValue--;
                timer = 0.0f;
                shootAnim.SetTrigger("IsTrigger");
            }
        }
    }

    //リロード処理
    void ReLoad()
    {
        if (InputManager.IsInputLeftButton())
        {
            //所持弾薬がある かつ 銃身内が満タンでない
            if (AmmoValue > 0 && remainingAmmoValue < MAX_FIRE_VALUE)
            {
                reloadGun.SetActive(true);
                shootGun.SetActive(false);
            }
        }

        //リロードアニメーションが終了していれば
        if (reloadAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //銃身内が満タンでない
            if (remainingAmmoValue < MAX_FIRE_VALUE)
            {
                isReloading = true;

                //リロード処理
                if(ammoValue - MAX_FIRE_VALUE > 0)
                {
                    ammoValue -= MAX_FIRE_VALUE - remainingAmmoValue;
                    remainingAmmoValue = MAX_FIRE_VALUE;
                }
                else
                {
                    remainingAmmoValue = ammoValue;
                    ammoValue = 0;
                }
            }

            isReloading = false;
            reloadGun.SetActive(false);
            shootGun.SetActive(true);
        }
    }
}
