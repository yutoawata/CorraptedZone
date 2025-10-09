using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlyerBulletUI : MonoBehaviour
{
    [SerializeField] PlayerController playerController = null;

    Text text = null;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerController.CurrentBulleValue.ToString();
    }
}
