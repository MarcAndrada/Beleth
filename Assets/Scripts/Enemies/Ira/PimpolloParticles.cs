using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpolloParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem secondExplosion;

    public void LastExplosion()
    {
        Instantiate(secondExplosion, transform.position, transform.rotation);
    }
}
