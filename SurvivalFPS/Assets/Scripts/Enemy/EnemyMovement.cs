using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IRayCastHit
{
    LightHouseController lightHouseController;
    LightEmissionController playerEmission;
    GameObject player;
    Rigidbody rb;
    [SerializeField]private float MoveSpeed;
    [SerializeField] float damageExplosionTiem = 2;
    [SerializeField] float damageBulletTiem = 2;
    [SerializeField] float playerTargetDis; // ìGÇ™ÉvÉåÉCÉÑÅ[Çí«Ç§ãóó£
    Vector3 targetPos = Vector3.zero;
    Vector3 targetRot = Vector3.zero;
    Vector3 movePos = Vector3.zero;
    float timer;
    bool isExplosionHit;
    bool isBulletHit;
    float currentSpeed;


    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = MoveSpeed;
        player = GameObject.Find("Player");
        playerEmission = player.GetComponent<LightEmissionController>();
        lightHouseController = GameObject.Find("LightHouse").GetComponentInChildren<LightHouseController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentSpeed);
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
        if (isExplosionHit)
        {
            currentSpeed = 0;
            timer += Time.deltaTime;
            if(timer >= damageExplosionTiem)
            {
                currentSpeed = MoveSpeed;
                isExplosionHit = false;
                timer = 0;
            }

        }
        else if (isBulletHit && !isExplosionHit)
        {
            currentSpeed = 0.5f;
            timer += Time.deltaTime;
            if (timer >= damageBulletTiem)
            {
                currentSpeed = MoveSpeed;
                isBulletHit = false;
                timer = 0;
            }
        }
    }

    public void OnRaycastHit(RaycastHit hit)
    {
        isBulletHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            isExplosionHit = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEmission.AddFuel(-300);
        }
    }


}
