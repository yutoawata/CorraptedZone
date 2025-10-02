using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputManager input;
    [SerializeField] Rigidbody rb;

    Vector3 moveDirection = Vector2.zero;
    Vector2 rotate = Vector2.zero;
    [SerializeField]float moveSpeed = 20.0f;
    [SerializeField]float rotateSpeed = 60.0f;
    bool isfirstFrame = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        rb.AddForce(-rb.velocity);

        if (!isfirstFrame)
        {
            moveDirection = new Vector2(input.InputLeftStickValue().x, input.InputLeftStickValue().y);
            rotate = input.InputRightStickValue();
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
}
