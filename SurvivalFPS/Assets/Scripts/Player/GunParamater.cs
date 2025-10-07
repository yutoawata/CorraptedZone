using UnityEngine;


[CreateAssetMenu(fileName = "GunParamater", menuName = "ScriptableObject/GunParameters")]
public class GunParamater : ScriptableObject
{
    public GameObject bullet;
    public InputManager input;
    public float shootPower = 100.0f;
    public float fireIngerval = 5.0f;
}
