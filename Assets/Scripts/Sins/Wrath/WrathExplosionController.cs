using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WrathExplosionController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private bool selfDestroy;
    [SerializeField]
    bool moveHimself;
    [SerializeField]
    float timeToExplode;
    [SerializeField]
    float radius;
    [SerializeField]
    float strenght;
    [SerializeField]
    float upStrenght;
    [SerializeField]
    private float playerImpulse;
    [SerializeField]
    private float bossDamage;

    [Header("VFX")]
    [SerializeField]
    GameObject wrathVFX;
    [SerializeField]
    ParticleSystem explosionWrathVFX;

    private WrathExplosionController explosionController;
    private Rigidbody rigidB;

    void Start()
    {
        explosionController = GetComponent<WrathExplosionController>();
        rigidB = GetComponentInChildren<Rigidbody>();

        if(wrathVFX) wrathVFX.SetActive(false);
    }

    //private void Update()
    //{
    //    while (isInWrath)
    //    {
    //
    //    }
    //}

    public void WrathExplosion()
    {
        //Implosione

        Instantiate(explosionWrathVFX, transform.position, transform.rotation);


        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        if (gameObject.tag == "Activator")
        {
            gameObject.GetComponent<ActivatorsController>().ActivateCurrentArm();
        }

        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "Player" && playerImpulse > 0)
            {
                hit.gameObject.GetComponentInChildren<BelethMovementController>().AddImpulse(transform.forward, playerImpulse);
            }

            if (hit.gameObject.tag == "Boss")
            {
                hit.gameObject.GetComponentInParent<WrathBossStateController>().GetDamage(bossDamage, gameObject);
            }

            if (hit.gameObject.tag == "Breakable" || hit.gameObject.tag == "Muro")
            {
                hit.gameObject.GetComponent<BrokenPiecesController>().Break();
            }


            Rigidbody rb = hit.GetComponent<Rigidbody>();

            

            if (rb != null && rb != rigidB || rb != null && moveHimself)
            {
                rb.AddExplosionForce(strenght, explosionPosition, radius, upStrenght, ForceMode.Impulse);
            }
        }

        if (selfDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            SetNormal();
        }
    }

    private void SetWrath(BelethSinsController _player)
    {
        if (wrathVFX)
        {
            wrathVFX.SetActive(true);
            InWrathAnim();
        }

        _player.AddWrathObject(explosionController);
    }

    private void SetNormal()
    {
        if (wrathVFX) wrathVFX.SetActive(false);
    }

    private void InWrathAnim()
    {
        //Vector3 pos = new Vector3(0, .5f, 0);
        //transform.DOPunchPosition(pos, 1, 6, .1f) ; //.SetLoops(10, LoopType.Restart);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wrath")
        {
            SetWrath(other.gameObject.GetComponentInParent<BelethSinsController>());
        }
    }
}
