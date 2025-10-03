using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerParameters paramaters;

    Rigidbody rb;
    Vector3 ADSPosition = new Vector3(0.0f, 0.6f, 0.3f);
    Vector3 hipPosition = new Vector3(0.2f, 0.55f, 0.4f);
    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    float cameraViewMax = 60.0f;
    float cameraViewMin = 45.0f;
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
            moveDirection = new Vector2(paramaters.input.InputLeftStickValue().x, paramaters.input.InputLeftStickValue().y);
            rotate = paramaters.input.InputRightStickValue();
        }
        else
        {
            isfirstFrame = false;
        }

        Quaternion rotation = transform.rotation;

        rotation.x = rotation.z = 0.0f;

        moveDirection = rotation * new Vector3(moveDirection.x, 0.0f, moveDirection.y);

        rb.AddForce(moveDirection * paramaters.moveSpeed);

        transform.eulerAngles += new Vector3(-rotate.y, rotate.x, 0.0f) * paramaters.rotateSpeed * Time.deltaTime;
    }

    void ADS()
    {
        if (paramaters.input.IsInputLeftTrigger())
        {
            paramaters.mainCamera.fieldOfView += (cameraViewMin - paramaters.mainCamera.fieldOfView) * Time.deltaTime * 5.0f;

            Vector3 direction = ADSPosition - paramaters.gun.transform.position;

            paramaters.gun.transform.position += direction * Time.deltaTime * 5.0f;
        }
        else
        {
            paramaters.mainCamera.fieldOfView += (cameraViewMax - paramaters.mainCamera.fieldOfView) * Time.deltaTime * 5.0f;


            Vector3 direction = hipPosition - paramaters.gun.transform.position;

            paramaters.gun.transform.position += direction * Time.deltaTime * 5.0f;
        }

        paramaters.mainCamera.fieldOfView = Mathf.Clamp(paramaters.mainCamera.fieldOfView, cameraViewMin, cameraViewMax);
    }
}
