using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject reloadGun = null;   //リロードモーション用の銃モデル
    [SerializeField] GameObject shootGun = null;    //射撃モーション用の銃モデル
    [SerializeField] GameObject muzzleFlash = null; //
    [SerializeField] GameObject reticle = null;
    [SerializeField] Camera mainCamera = null;      //
    [SerializeField] LightEmissionController lightEmissionController = null;
    [SerializeField] MeshRenderer gun_obj = null;   //
    [SerializeField] float shootLenge = 20.0f;
    [SerializeField] float fireIngerval = 1.0f;
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float rotateSpeed = 300.0f;
    [SerializeField] float recoilPower = 5.0f;
    [SerializeField] float interactTime = 2.0f;
    [SerializeField] float recoveryFuelValue = 40.0f;

    const int MAX_FIRE_VALUE = 2;   //リロード無しでの射撃回数
    const int MAX_AMMO_VALUE = 100;

    Rigidbody rb;
    Material gunMaterial;
    Animator shootAnim;
    Animator reloadAnim;
    Vector3 ADSPosition = new Vector3(0.0f, 0.393f, 0.25f);
    Vector3 hipPosition = new Vector3(0.2f, 0.35f, 0.45f);
    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    float cameraViewMax = 60.0f;
    float cameraViewMin = 45.0f;
    float ADSSpeed = 5.0f;
    float currentSpeed = 0.0f;
    float timer = 0.0f;
    int remainingAmmoValue = 0;    //射撃可能数(重心内の弾の数)
    int fuelValue = 0;
    int ammoValue = 10;
    bool isReloading = false;
    bool isInteract = false;

    public int FuelValue { get => fuelValue; }
    public int RemainingAmmoValue { get => remainingAmmoValue; }
    public int AmmoValue { get => ammoValue; }

    // Start is called before the first frame update
    void Start()
    {
        
        shootAnim = shootGun.GetComponent<Animator>();
        reloadAnim = reloadGun.GetComponent<Animator>();
        gunMaterial = gun_obj.GetComponent<MeshRenderer>().material;
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
        if(other.CompareTag("Generator"))
        {

            if (InputManager.IsInputRightButton())
            {
                if (!isInteract)
                {
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
        if (isReloading)
        {
            return;
        }

        if (InputManager.IsInputLeftTrigger())
        {
            //reticle.SetActive(false);
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
            //reticle.SetActive(true);
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
        if (!shootAnim.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            muzzleFlash.SetActive(false);
        }

        if (remainingAmmoValue <= 0)
        {
            return;
        }

        if (InputManager.IsInputRightTrigger())
        {
            if (timer >= fireIngerval)
            {
                muzzleFlash.SetActive(true);
                if (Physics.Raycast(mainCamera.transform.position, transform.forward, out RaycastHit target, shootLenge))
                {
                    target.collider.gameObject.SendMessage("OnRaycastHit", target, SendMessageOptions.DontRequireReceiver);
                }
                remainingAmmoValue--;
                timer = 0.0f;
                shootAnim.SetTrigger("IsTrigger");
            }
        }
    }

    void ReLoad()
    {
        if (InputManager.IsInputLeftButton())
        {
            reloadGun.SetActive(true);
            shootGun.SetActive(false);
        }

        if (reloadAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {   
            if (remainingAmmoValue != MAX_FIRE_VALUE)
            {
                isReloading = true;

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
