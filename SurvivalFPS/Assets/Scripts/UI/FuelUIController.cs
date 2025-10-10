using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUIController : MonoBehaviour
{
    [SerializeField] Text fuelText = null;
    [SerializeField] PlayerController playerController = null;

    public void UpdateFuelUI()
    {
        int fuelValue = playerController.FuelValue;
        fuelText.text = "”R—¿ : " + fuelValue;
    }
}
