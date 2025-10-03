using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float shootPower = 100.0f;
    [SerializeField] float fireIngerval = 5.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = fireIngerval;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < fireIngerval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("”­ŽËOK");
        }

        if (InputManager.IsInputRightTrigger())
        {
            Debug.Log("ƒgƒŠƒK[");

            if (timer >= fireIngerval)
            {
                Fire();
                timer = 0.0f;

                Debug.Log("Fire");
            }
        }
    }

    void Fire()
    {
        GameObject obj = null;
        obj = Instantiate(bullet,transform.position + transform.forward * 2.0f,Quaternion.identity);

        obj.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
    }
}
