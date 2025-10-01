using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] InputManager input;
    [SerializeField] float fireInterval = 5.0f;

    float timer = 0.0f;
    float isVisibleTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= fireInterval)
        {
            timer += Time.deltaTime;
        }

        if (input != null)
        {
            if (input.IsInputRightShoulder())
            {
                if (timer >= fireInterval) 
                {
                    Fire();
                    timer = 0.0f;
                }
            }
        }

        Delete();
    }

    void Fire()
    {
        Instantiate(bullet,transform.position - transform.up,Quaternion.identity);
    }

    void Delete()
    {
        Destroy(gameObject,5.0f);
    }
}
