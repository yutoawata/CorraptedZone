using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    [SerializeField] GunParamater paramater;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = paramater.fireIngerval;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < paramater.fireIngerval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("”­ŽËOK");
        }

        if (paramater.input != null)
        {


            if (paramater.input.IsInputRightShoulder())
            {
                Debug.Log("ƒgƒŠƒK[");

                if (timer >= paramater.fireIngerval)
                {
                    Fire();
                    timer = 0.0f;

                    Debug.Log("Fire");
                }
            }
        }
    }

    void Fire()
    {
        GameObject obj = null;
        obj = Instantiate(paramater.bullet,transform.position + transform.forward * 2.0f,Quaternion.identity);

        obj.GetComponent<Rigidbody>().AddForce(transform.forward * paramater.shootPower);
    }
}
