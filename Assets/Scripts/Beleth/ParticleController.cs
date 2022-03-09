using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    [SerializeField] ParticleSystem KirbyDust;
    [SerializeField] ParticleSystem MarioDust;
    [SerializeField] Transform LeftAnkle;
    [SerializeField] Transform RightAnkle;
    [SerializeField] Transform Ankles;

    public void ActivateLeftDustFBX()
    {
        Instantiate(KirbyDust, LeftAnkle.position, LeftAnkle.rotation); 
    }
    
    public void ActivateRightDustFBX()
    {
        Instantiate(KirbyDust, RightAnkle.position, RightAnkle.rotation); 
    }
    
    public void ActivateDustFBX()
    {
        Instantiate(MarioDust, Ankles.position, Ankles.rotation); 
    }
}
