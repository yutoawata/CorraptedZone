using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]private LightHouseController lightHouseController;
    [SerializeField]private float MoveSpeed;
    Vector3 targetPos = Vector3.zero;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = lightHouseController.ReturnEnemyTargetTransform();
        transform.position = Vector3.MoveTowards(transform.position,targetPos, MoveSpeed * Time.deltaTime);
    }


}
