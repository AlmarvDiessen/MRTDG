using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoof : MonoBehaviour
{
    [SerializeField] private ParticleSystem system; 
    public GameObject Explode()
    {
        // CAUSES WEIRD LAG
        system.Emit(100);
        return gameObject;
    }
}
