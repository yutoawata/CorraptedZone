
using System;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Camera mainCamera;
    [SerializeField] MeshRenderer gun_obj;
    [SerializeField] float shootLenge = 20.0f;
    [SerializeField] float fireIngerval = 1.0f;
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float rotateSpeed = 300.0f;
    [SerializeField] float recoilPower = 5.0f;
    const int MAX_FIRE_VALUE = 2;   //リロード無しでの射撃回数

    Rigidbody rb;
    Material gunMaterial;
    Animator controller;
    Vector3 ADSPosition = new Vector3(0.0f, 0.393f, 0.25f);
    Vector3 hipPosition = new Vector3(0.2f, 0.35f, 0.45f);
    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    float cameraViewMax = 60.0f;
    float cameraViewMin = 45.0f;
    float ADSSpeed = 5.0f;
    float currentSpeed = 0.0f;
    float timer = 0.0f;
    int currentBulleValue = 10;      //所持している弾の総数
    int remainingBulletVaue = 0;    //射撃可能数(重心内の弾の数)
    bool isfirstFrame = true;
    bool isDown = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = gun.GetComponent<Animator>();
        timer = fireIngerval;
        rb = GetComponent<Rigidbody>();
        gunMaterial = gun_obj.GetComponent<MeshRenderer>().material;
        remainingBulletVaue = MAX_FIRE_VALUE;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < fireIngerval)
        {
            timer += Time.deltaTime;
        }

        if (InputManager.IsInputRightTrigger())
        {
            Debug.Log("トリガー");

            if (timer >= fireIngerval && remainingBulletVaue > 0)
            {
                Fire();
                timer = 0.0f;
                controller.SetTrigger("IsTrigger");

                Debug.Log("Fire");
            }
        }

        if (InputManager.IsInputLeftButton())
        {
            ReLoad();
        }

        Move();

        ADS();

        Recoiling();
    }

    void Move()
    {
        rb.AddForce(-rb.velocity);

        if (!isfirstFrame)
        {
            moveDirection = InputManager.InputLeftStickValue();
            rotate = InputManager.InputRightStickValue();
        }
        else
        {
            isfirstFrame = false;
        }

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
            Vector3 direction = ADSPosition - gun.transform.localPosition;
            gun.transform.localPosition += direction * ADSSpeed * Time.deltaTime;
            currentSpeed = moveSpeed / 2.0f;
        }
        else
        {
            gunMaterial.SetFloat("_OutlineWidth", 0.001f);
            //ズームアウト
            mainCamera.fieldOfView += (cameraViewMax - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;

            //ショットガン移動処理
            Vector3 direction = hipPosition - gun.transform.localPosition;
            gun.transform.localPosition += direction * ADSSpeed * Time.deltaTime;
            currentSpeed = moveSpeed;
        }
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, cameraViewMin, cameraViewMax);
    }

    void Fire()
    {
        if(Physics.Raycast(mainCamera.transform.position,transform.forward,out RaycastHit target, shootLenge))
        {
            if (target.collider.gameObject.tag == "Enemy")
            {
                
            }
            target.collider.gameObject.SendMessage("OnRaycastHit", target, SendMessageOptions.DontRequireReceiver);
        }
        remainingBulletVaue--;
    }

    void ReLoad()
    {
        if (remainingBulletVaue != MAX_FIRE_VALUE)
        {
            remainingBulletVaue = MAX_FIRE_VALUE;

            currentBulleValue -= MAX_FIRE_VALUE - remainingBulletVaue;
        }
    }

    void Recoiling()
    {
        if (mainCamera.transform.rotation.x < 5.0f || mainCamera.transform.rotation.x > 1e-4)
        {
            if (!isDown)
            {
                mainCamera.transform.eulerAngles += new Vector3(isDown ? recoilPower : -recoilPower, 0.0f, 0.0f) * Time.deltaTime;
            }
        }
        else
        {
            isDown = true;
        }
    }
}
