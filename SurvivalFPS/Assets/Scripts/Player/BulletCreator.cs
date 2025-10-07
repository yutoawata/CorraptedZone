using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BulletCreator : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float shootPower = 2000.0f;
    [SerializeField] float fireIngerval = 5.0f;

    Animator controller;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
        timer = fireIngerval;

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
            Debug.Log("ƒgƒŠƒK[");

            if (timer >= fireIngerval)
            {
                Fire();
                timer = 0.0f;
                controller.SetTrigger("IsTrigger");

                Debug.Log("Fire");
            }
        }
    }

    void Fire()
    {
        GameObject obj = null;
        obj = Instantiate(bullet,transform.position + -transform.forward, Quaternion.identity);

        obj.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
    }
}
