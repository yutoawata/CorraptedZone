using UnityEngine;

public class LightHouseController : MonoBehaviour
{
    [SerializeField]private float spotChangeSpan;
    [SerializeField] private Transform[] targetItem;
    private int spotNumber;
    private int spotIndex;

    // Start is called before the first frame update
    void Start()
    {
        spotIndex = targetItem.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //表示するスポット＝（経過時間 / 待機時間）％　要素数
        spotNumber = Mathf.FloorToInt(Time.time / spotChangeSpan) % spotIndex;
        Debug.Log(spotNumber);
        transform.LookAt(targetItem[spotNumber]);
    }
}
