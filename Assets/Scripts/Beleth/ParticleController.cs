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

    [Header("Fall")]
    [SerializeField] ParticleSystem FallDust;

    [Header("Attack")]
    [SerializeField] ParticleSystem AttackVFX;
    [SerializeField] Transform HandSocket;

    [Header("Damage")]
    [SerializeField] ParticleSystem DamageVFX;
    [SerializeField] Transform CapSocket;

    [Header("Gidle")]
    [SerializeField] ParticleSystem SweatVFX;


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

    public void FallDustFbx()
    {
        Instantiate(FallDust, Ankles.position, Ankles.rotation);
    }

    public void ActivateAttack()
    {
        Instantiate(AttackVFX, HandSocket.position, HandSocket.rotation);

    }

    //IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(0.4f);
    //    DesactivateAttack();
    //}
    //
    //private void DesactivateAttack()
    //{
    //    AttackVFX.SetActive(false);
    //}

    public void DamageDustFbx()
    {
        Instantiate(DamageVFX, CapSocket.position, CapSocket.rotation);
    }

    public void SweatFbx()
    {
        Instantiate(SweatVFX, CapSocket.position, CapSocket.rotation);
    }
}
