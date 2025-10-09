using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject reloadGun = null;   //リロードモーション用の銃モデル
    [SerializeField] GameObject shootGun = null;    //射撃モーション用の銃モデル
    [SerializeField] GameObject muzzleFlash = null; //
    [SerializeField] Camera mainCamera = null;      //
    [SerializeField] MeshRenderer gun_obj = null;   //
    [SerializeField] float shootLenge = 20.0f;
    [SerializeField] float fireIngerval = 1.0f;
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float rotateSpeed = 300.0f;
    [SerializeField] float recoilPower = 5.0f;


    const int MAX_FIRE_VALUE = 2;   //リロード無しでの射撃回数

    Rigidbody rb;
    Material gunMaterial;
    Animator ShootAnim;
    Animator reloadAnim;
    AnimatorStateInfo stateInfo;
    Vector3 ADSPosition = new Vector3(0.0f, 0.393f, 0.25f);
    Vector3 hipPosition = new Vector3(0.2f, 0.35f, 0.45f);
    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    float cameraViewMax = 60.0f;
    float cameraViewMin = 45.0f;
    float ADSSpeed = 5.0f;
    float currentSpeed = 0.0f;
    float recoilTimer = 0.0f;
    float timer = 0.0f;
    int currentBulleValue = 10;      //所持している弾の総数
    int remainingBulletVaue = 0;    //射撃可能数(重心内の弾の数)
    int fuelValue = 0;
    bool isfirstFrame = true;
    bool isReloading = false;
    bool isRecoiling = false;
    bool isDown = false;



    public int CurrentBulleValue { get => currentBulleValue; }

    // Start is called before the first frame update
    void Start()
    {
        
        ShootAnim = shootGun.GetComponent<Animator>();
        reloadAnim = reloadGun.GetComponent<Animator>();
        gunMaterial = gun_obj.GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
       
        remainingBulletVaue = MAX_FIRE_VALUE;
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


        Fire();
        

        if (!isRecoiling) 
        {
            ADS();
        }

        ReLoad();
        Move();

        if (isRecoiling)
        {
            //往復分の時間を代入
            recoilTimer = 2.0f;
            Recoiling();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Generator"))
        {
            if(fuelValue > 0)
            {
                if (InputManager.IsInputRightButton())
                {
                    fuelValue--;
                }
            }
            
        }
    }

    void Move()
    {
        rb.AddForce(-rb.velocity);

        moveDirection = InputManager.InputLeftStickValue();
        rotate = InputManager.InputRightStickValue();

        Quaternion rotation = transform.rotation;

        rotation.x = rotation.z = 0.0f;

        moveDirection = rotation * new Vector3(moveDirection.x, 0.0f, moveDirection.y);

        rb.AddForce(moveDirection * currentSpeed);

        transform.eulerAngles += new Vector3(-rotate.y, rotate.x, 0.0f) * rotateSpeed * Time.deltaTime;
    }

    void ADS()
    {
        if (InputManager.IsInputLeftTrigger())
        {
            gunMaterial.SetFloat("_OutlineWidth", 0.0f);
            //ズームイン
            mainCamera.fieldOfView += (cameraViewMin - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;
            //移動処理
            Vector3 direction = ADSPosition - shootGun.transform.localPosition;
            shootGun.transform.localPosition += direction * ADSSpeed * Time.deltaTime;
            currentSpeed = moveSpeed / 2.0f;
        }
        else
        {
            gunMaterial.SetFloat("_OutlineWidth", 0.001f);
            //ズームアウト
            mainCamera.fieldOfView += (cameraViewMax - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;

            //ショットガン移動処理
            Vector3 direction = hipPosition - shootGun.transform.localPosition;
            shootGun.transform.localPosition += direction * ADSSpeed * Time.deltaTime;
            currentSpeed = moveSpeed;
        }
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, cameraViewMin, cameraViewMax);
    }

    void Fire()
    {
        if (InputManager.IsInputRightTrigger())
        {
            if (timer >= fireIngerval && remainingBulletVaue > 0)
            {
                muzzleFlash.SetActive(true);
                if (Physics.Raycast(mainCamera.transform.position, transform.forward, out RaycastHit target, shootLenge))
                {
                    target.collider.gameObject.SendMessage("OnRaycastHit", target, SendMessageOptions.DontRequireReceiver);
                }
                isRecoiling = true;
                remainingBulletVaue--;
                timer = 0.0f;
                ShootAnim.SetTrigger("IsTrigger");
            }
        }

        if()
    }

    void ReLoad()
    {
        if (InputManager.IsInputLeftButton())
        {
            reloadGun.SetActive(true);
            shootGun.SetActive(false);
        }

        Debug.Log(reloadAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log(stateInfo.IsName("Reload"));

        if (reloadAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("re");
            
            if (remainingBulletVaue != MAX_FIRE_VALUE)
            {
                isReloading = true;
                remainingBulletVaue = MAX_FIRE_VALUE;
                currentBulleValue -= MAX_FIRE_VALUE - remainingBulletVaue;
            }

            isReloading = false;
            reloadGun.SetActive(false);
            shootGun.SetActive(true);
        }
    }

    void Recoiling()
    {
        if (recoilTimer <= 0.0f)
        {
            isRecoiling = false;
            return;
        }
        else
        {
            recoilTimer -= Time.deltaTime;
        }

        if (!(mainCamera.transform.localEulerAngles.x >= 355.0f || mainCamera.transform.localEulerAngles.x == 0.0f))
        {
            isDown = !isDown;
        }

        mainCamera.transform.eulerAngles 
            -= new Vector3(isDown ? -recoilPower : recoilPower, 0.0f, 0.0f) * Time.deltaTime;
    }
}
