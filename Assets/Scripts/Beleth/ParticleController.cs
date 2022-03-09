using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] ParticleSystem WalkDust;
    [SerializeField] Transform LeftAnkle;
    [SerializeField] Transform RightAnkle;
    [SerializeField] Transform Ankles;

    [Header("Run")]
    [SerializeField] ParticleSystem KirbyDust;
    [SerializeField] ParticleSystem MarioDust;

    [Header("Jump")]
    [SerializeField] ParticleSystem ExteriorJumpDust;
    [SerializeField] ParticleSystem InteriorJumpDust;
    [SerializeField] ParticleSystem JumpTrailDust;
    [SerializeField] Transform Socket;

    public void WalkLeftDustFbx()
    {
        //Instantiate(WalkDust, LeftAnkle.position, LeftAnkle.rotation);
    }

    public void WalkRightDustFbx()
    {
        //Instantiate(WalkDust, RightAnkle.position, RightAnkle.rotation);
    }

    public void RunLeftDustFbx()
    {
        Instantiate(KirbyDust, LeftAnkle.position, LeftAnkle.rotation); 
    }
    
    public void RunRightDustFbx()
    {
        Instantiate(KirbyDust, RightAnkle.position, RightAnkle.rotation); 
    }
    
    public void RunDustFbx()
    {
        Instantiate(MarioDust, Ankles.position, Ankles.rotation); 
    }

    public void JumpDustFbx()
    {
        Instantiate(ExteriorJumpDust, Ankles.position, Ankles.rotation);
    }
    
    public void DobleJumpDustFbx()
    {
        Instantiate(InteriorJumpDust, new Vector3(Ankles.position.x, Ankles.position.y + 1f, Ankles.position.z), Ankles.rotation);
    }

    public void JumpTrailDustFbx()
    {

    }
}
