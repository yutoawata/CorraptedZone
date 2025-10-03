using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Camera mainCamera;
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float rotateSpeed = 60.0f;
    Rigidbody rb;
    Vector3 ADSPosition = new Vector3(0.0f, 0.662f, 0.2f);
    Vector3 hipPosition = new Vector3(0.2f, 0.55f, 0.4f);
    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    float cameraViewMax = 60.0f;
    float cameraViewMin = 45.0f;
    float ADSSpeed = 5.0f;
    bool isfirstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        rb.AddForce(moveDirection * moveSpeed);

        transform.eulerAngles += new Vector3(-rotate.y, rotate.x, 0.0f) * rotateSpeed * Time.deltaTime;
    }

    void ADS()
    {
        if (InputManager.IsInputLeftTrigger())
        {
            Debug.Log("ADS");

            mainCamera.fieldOfView += (cameraViewMin - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;

            Vector3 direction = ADSPosition - gun.transform.localPosition;

            gun.transform.localPosition += direction * Time.deltaTime * ADSSpeed;
        }
        else
        {
            mainCamera.fieldOfView += (cameraViewMax - mainCamera.fieldOfView) * Time.deltaTime * 5.0f;


            Vector3 direction = hipPosition - gun.transform.localPosition;

            gun.transform.localPosition += direction * Time.deltaTime * ADSSpeed;
        }

        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, cameraViewMin, cameraViewMax);
    }
}
