using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlyerBulletUI : MonoBehaviour
{
    [SerializeField] PlayerController playerController = null;

    [SerializeField] Text textBullet = null;
    [SerializeField] Text doubleBullet = null;

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        textBullet.text = "écíeêî : " + playerController.AmmoValue.ToString();
        doubleBullet.text = "î≠éÀâ¬î\êî : " + playerController.RemainingAmmoValue.ToString();
    }
}
