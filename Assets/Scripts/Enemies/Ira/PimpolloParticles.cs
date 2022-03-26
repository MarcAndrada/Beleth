using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpolloParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem AppersVFX;
    [SerializeField]
    private Transform AppearsSocket;

    [SerializeField]
    private ParticleSystem killedVFX;

    public void LastExplosion()
    {
        Instantiate(killedVFX, transform.position, transform.rotation);
    }

    public void Appears()
    {
        Instantiate(AppersVFX, AppearsSocket.position, AppearsSocket.rotation); 
    }


}
