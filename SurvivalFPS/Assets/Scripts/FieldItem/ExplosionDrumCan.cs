using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDrumCan : MonoBehaviour, IRayCastHit
{
    [SerializeField] ExplosionManager explosionManager;


    [Header("”š”­‚Ì‘å‚«‚³")]
    [SerializeField] float explosionMul;


    public void OnRaycastHit(RaycastHit hit)
    {
        explosionManager.Generate(transform.position.x, transform.position.y, transform.position.z, explosionMul);
    }
}
