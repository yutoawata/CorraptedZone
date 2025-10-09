using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReGenerateFuel : MonoBehaviour
{
    [SerializeField] LightEmissionController emissionController;
    [SerializeField] float regeneValue;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            emissionController.SetRegenerateing(true); // ‰ñ•œ’†‚É‚·‚é
            emissionController.AddFuel(regeneValue); // ‰ñ•œ‚³‚¹‚é
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            emissionController.SetRegenerateing(false);
        }
    }
}
