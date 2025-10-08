using UnityEngine;

public class LightHouseRotation : MonoBehaviour
{
    [SerializeField] LightHouseController lightHouseController;
    [SerializeField] GameObject child;
    [SerializeField] Transform[] target;
    [SerializeField]private float spotChangeSpan;       //êÿÇËë÷Ç¶ä‘äu
    [SerializeField]private float rotationSpeed;
    private Vector3 targerPos;


    void Update()
    {
        ChengeAngle();
        Debug.Log(transform.up + "è„ÇæÇÊ");
    }

    private void ChengeAngle()
    {
        targerPos = lightHouseController.ReturnEnemyTargetTransform();
        Vector3 curremtPos = targerPos;
        Vector3 adj = Vector3.zero;
        transform.LookAt(targerPos,new Vector3(0, -1, 0));

    }
}
