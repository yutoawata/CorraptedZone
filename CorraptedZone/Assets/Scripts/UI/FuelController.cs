using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    [SerializeField] LightEmissionController emissionController;
    Slider fuel;
    float maxFuel;
    float currentFuel;
    void Start()
    {
        fuel = GetComponent<Slider>();
        fuel.value = 1;
        maxFuel = emissionController.Fuel;
        currentFuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        currentFuel = emissionController.Fuel;
        fuel.value = currentFuel / maxFuel;
    }
}
