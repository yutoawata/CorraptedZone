using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    LightHouseController lightHouseController;
    GameObject player;
    Rigidbody rb;
    [SerializeField]private float MoveSpeed;
    [SerializeField] float damageTiem = 2;
    [SerializeField] float playerTargetDis; // ìGÇ™ÉvÉåÉCÉÑÅ[Çí«Ç§ãóó£
    Vector3 targetPos = Vector3.zero;
    Vector3 targetRot = Vector3.zero;
    Vector3 movePos = Vector3.zero;
    float timer;
    bool isHit;
    float currentSpeed;


    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = MoveSpeed;
        player = GameObject.Find("Player");
        lightHouseController = GameObject.Find("LightHouse").GetComponentInChildren<LightHouseController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(transform.position, player.transform.position);
        if (dis <= playerTargetDis)
        {
           targetPos = player.transform.position;
           targetRot = player.transform.position;
        }
        else
        {
            targetPos = lightHouseController.ReturnEnemyTargetTransform();
            targetRot = lightHouseController.ReturnEnemyTargetTransform();
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetRot);
        transform.rotation = targetRotation;
        movePos = targetPos - transform.position;
        movePos.Normalize();
        rb.AddForce(movePos * currentSpeed);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);



        // îöî≠ÇéÛÇØÇΩÇÁìÆÇ´Ç™é~Ç‹ÇÈ
        if (isHit)
        {
            currentSpeed = 0;
            timer += Time.deltaTime;
            if(timer >= damageTiem)
            {
                currentSpeed = MoveSpeed;
                isHit = false;
                timer = 0;
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            isHit = true;
            Debug.Log("ìñÇΩÇ¡ÇΩÇÊ");
        }
    }


}
