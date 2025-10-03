using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Camera mainCamera;
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float rotateSpeed = 300.0f;

    Rigidbody rb;
    Material gunMaterial;
    Vector3 ADSPosition = new Vector3(0.0f, 0.662f, 0.2f);
    Vector3 hipPosition = new Vector3(0.2f, 0.55f, 0.4f);
    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    float cameraViewMax = 60.0f;
    float cameraViewMin = 45.0f;
    float ADSSpeed = 5.0f;
    float currentSpeed = 0.0f;
    bool isfirstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunMaterial = gun.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        ADS();
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

        transform.rotation = Quaternion.Euler(new Vector3(-rotate.y, rotate.x, 0.0f) * rotateSpeed * Time.deltaTime);

        //transform.eulerAngles += new Vector3(-rotate.y, rotate.x, 0.0f) * rotateSpeed * Time.deltaTime;
    }

    void ADS()
    {
        if (InputManager.IsInputLeftTrigger())
        {
            Debug.Log("ADS");

            gunMaterial.SetFloat("_OutlineWidth", 0.0f);
            //ズームイン
            mainCamera.fieldOfView += (cameraViewMin - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;
            //移動処理
            Vector3 direction = ADSPosition - gun.transform.localPosition;
            gun.transform.localPosition += direction * Time.deltaTime * ADSSpeed;
            currentSpeed = moveSpeed / 2.0f;
        }
        else
        {
            gunMaterial.SetFloat("_OutlineWidth", 0.001f);
            //ズームアウト
            mainCamera.fieldOfView += (cameraViewMax - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;

            //ショットガン移動処理
            Vector3 direction = hipPosition - gun.transform.localPosition;
            gun.transform.localPosition += direction * Time.deltaTime * ADSSpeed;
            currentSpeed = moveSpeed;
        }
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, cameraViewMin, cameraViewMax);
    }
}
