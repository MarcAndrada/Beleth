using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WrathExplosionController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    public bool canBeTriggered = true;
    [SerializeField]
    private bool selfDestroy;
    [SerializeField]
    bool moveHimself;
    [SerializeField]
    float timeToExplode;
    [SerializeField]
    private Transform explosionPos;
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
    [HideInInspector]
    public bool isOnWrath = false;

    [Header("VFX")]
    [SerializeField]
    GameObject wrathVFX;
    [SerializeField]
    GameObject explosionWrathVFX;
    [SerializeField] Transform SocketVFX;

    [SerializeField]
    public string objectType;
    private WrathExplosionController explosionController;
    private Rigidbody rigidB;

    void Start()
    {
        explosionController = GetComponent<WrathExplosionController>();
        rigidB = GetComponentInChildren<Rigidbody>();

        if(wrathVFX) wrathVFX.SetActive(false);
    }


    public void WrathExplosion()
    {
        //Implosione

        Instantiate(explosionWrathVFX, explosionPos.position , Quaternion.identity);


        if (gameObject.tag == "Activator")
        {
            GetComponent<ActivatorController>().ActivatorUsed();
        }

        if (gameObject.tag == "ActivatorTutorial")
        {
            GetComponent<ActivatorTutoController>().ActivatorUsed();  
        }

        if (objectType == "RockImpulsor")
        {
            GetComponent<RockImpulsorController>().ActivateImpulsor();
        }

        Collider[] colliders = Physics.OverlapSphere(explosionPos.position , radius);

       
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

            if (hit.gameObject.tag == "Movable")
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) 
                {
                    rb.AddExplosionForce(strenght, explosionPos.position, radius, upStrenght, ForceMode.Impulse);
                }
            }

            if (hit.gameObject.tag == "StoneWall") 
            {
                hit.GetComponent<StoneWallController>().ShakeWall();
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

    public void CheckOnWrath(BelethSinsController _player) 
    {
        if (objectType != "")
        {
            _player.AddWrathObject(explosionController);
        }
        else
        {
            Debug.LogError(gameObject + " No tiene un tipo asignado");
        }
    }

    public void SetWrath()
    {
        if (wrathVFX)
        {
            wrathVFX.SetActive(true);
            isOnWrath = true;
        }


    }

    private void SetNormal()
    {
        if (wrathVFX) wrathVFX.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(explosionPos.position, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wrath")
        {
            CheckOnWrath(other.gameObject.GetComponentInParent<BelethSinsController>());
        }
    }
}
