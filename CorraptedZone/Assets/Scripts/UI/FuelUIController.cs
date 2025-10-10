using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUIController : MonoBehaviour
{
    [SerializeField] Text fuelText = null;
    [SerializeField] PlayerController playerController = null;

    int fuelValue = 0;

    private void Update()
    {
        fuelValue = playerController.FuelValue;
        fuelText.text = "”R—¿ : " +  fuelValue.ToString();
    }
}
