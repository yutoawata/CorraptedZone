using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IRayCastHit
{
    [SerializeField]private float MoveSpeed;
    [SerializeField] float damageExplosionTiem = 2;
    [SerializeField] float damageBulletTiem = 2;
    [SerializeField] float playerTargetDis; // 敵がプレイヤーを追う距離
    ExplosionManager explosionManager;
    LightHouseController lightHouseController;
    LightEmissionController playerEmission;
    GameObject player;
    Rigidbody rb;
    Vector3 targetPos = Vector3.zero;
    Vector3 targetRot = Vector3.zero;
    Vector3 movePos = Vector3.zero;
    float timer;
    float yPos;
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
        explosionManager = GameObject.Find("ExplosionManager").GetComponent<ExplosionManager>();
        rb = GetComponent<Rigidbody>();
        yPos = transform.position.y; // 敵が浮かないようにするため
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dis = player.transform.position - transform.position;
        Vector3 lightDis = lightHouseController.ReturnEnemyTargetTransform() - transform.position; // ライト方向に向かせるために必要
        if (dis.sqrMagnitude <= playerTargetDis * playerTargetDis)
        {
           targetPos = player.transform.position;
           targetRot = player.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dis);
            transform.rotation = targetRotation;
        }
        else
        {
            targetPos = lightHouseController.ReturnEnemyTargetTransform();
            targetRot = lightHouseController.ReturnEnemyTargetTransform();
            Quaternion targetRotation = Quaternion.LookRotation(lightDis);
            transform.rotation = targetRotation;
        }
        movePos = targetPos - transform.position;
        movePos.Normalize();
        rb.AddForce(movePos * currentSpeed, ForceMode.Acceleration);



        // 爆発を受けたら動きが止まる
        if (isExplosionHit)
        {
            currentSpeed = 0;
            timer += Time.fixedDeltaTime;
            if(timer >= damageExplosionTiem)
            {
                currentSpeed = MoveSpeed;
                isExplosionHit = false;
                timer = 0;
            }

        }
        else if (isBulletHit && !isExplosionHit)
        {

            rb.velocity *= 0.7f;
            timer += Time.fixedDeltaTime;
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
        explosionManager.Generate(hit.point.x, hit.point.y, hit.point.z, 0.3f);
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
