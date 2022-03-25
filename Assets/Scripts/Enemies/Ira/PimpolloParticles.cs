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
    private ParticleSystem suicideVFX;

    public void LastExplosion()
    {
        Instantiate(suicideVFX, transform.position, transform.rotation);
    }

    public void Appears()
    {
        Instantiate(AppersVFX, AppearsSocket.position, AppearsSocket.rotation); 
    }


}
