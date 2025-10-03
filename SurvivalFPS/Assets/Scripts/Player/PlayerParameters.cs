using UnityEngine;


[CreateAssetMenu(fileName = "PlayerParamater", menuName = "ScriptableObject/PlayerParameters")]
public class PlayerParameters : ScriptableObject
{
    public GameObject gun;
    public Camera mainCamera;
    public InputManager input;
    public float moveSpeed = 20.0f;
    public float rotateSpeed = 60.0f;
}
