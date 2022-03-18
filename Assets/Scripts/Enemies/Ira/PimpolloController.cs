using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PimpolloController : MonoBehaviour
{
    [SerializeField]
    private float knockBackForce = 6;
    [SerializeField]
    private float knockUpForce = 0.5f;

    [Header("Movement")]
    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private SphereCollider detectionColl;
    [SerializeField]
    private float maxTimeChasing;
    
    [Header("Attack")]
    [SerializeField]
    private float distanceToAttack;
    [Tooltip("Velocidad en la que hara el salto hacia el player")]
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float maxAttackPercent;
    [SerializeField]
    private SphereCollider attackCollider;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystem exclamationVfx;
    [SerializeField]
    private Transform exclamationSocket;
    [SerializeField]
    private ParticleSystem dieExplosionVfx;
   
    private float attackTranscurse = 0;
    private bool chasePlayer = false;
    private bool isAttacking = false;
    private bool destroying = false;

    private NavMeshAgent navMesh;
    private GameObject player;
   
    private Animator animator;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (chasePlayer)
        {
            //Si no esta lo suficiente mente cerca del jugador perseguira al player en caso que este lo sufucientemente cerca este le atacara
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack && !isAttacking)
            {
                if (navMesh.enabled == true)
                {
                    navMesh.destination = player.transform.position;
                }
            }
            else
            {
                AttackPlayer();
            }
        }
    }

    IEnumerator PlayerSeen()
    {
        //Desactiva la colision que detecta si el player esta en el area y tras esperar un tiempo empieza a perseguir al player

        detectionColl.enabled = false;
        transform.LookAt(player.transform);
        Instantiate(exclamationVfx, exclamationSocket.position, exclamationSocket.rotation);
        yield return new WaitForSeconds(timeToWait);
        animator.SetTrigger("Chase");
        chasePlayer = true;
        StartCoroutine(WaitToExplode());
    }

    private void AttackPlayer() {

        transform.LookAt(player.transform, Vector3.up);
        attackCollider.enabled = true;
        isAttacking = true;
        navMesh.enabled = false;

        if (attackTranscurse < maxAttackPercent)
        {
            attackTranscurse += attackSpeed * Time.deltaTime;
        }
        else
        {
            if (!destroying)
            {
                SelfDestroy();
                Destroy(gameObject);
            }
        }

        transform.position = Vector3.LerpUnclamped(transform.position, player.transform.position, attackTranscurse);
    }

    private void SelfDestroy()
    {
        // Instancia las particulas de explosion y se autodestruye
        Instantiate(dieExplosionVfx, transform.position, transform.rotation);
        destroying = true;
    }

    private IEnumerator WaitToExplode()
    {
        // Tras empezar a perseguir al jugador si no lo alcanza en el tiempo que tiene este se autidestruira
        yield return new WaitForSeconds(maxTimeChasing);
        SelfDestroy();
        StartCoroutine(WaitToDestroy(0));
    }

    private IEnumerator WaitToDestroy(float _timeToWait) 
    {
        yield return new WaitForSeconds(_timeToWait);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !chasePlayer)
        {
            //En caso de que el player entre en la zona de interaccion empezara a perseguirle
            player = other.gameObject;
            StartCoroutine(PlayerSeen());
        }

        if (other.tag == "Trident" && chasePlayer)
        {
            // Si es golpeado por el tridente del player el enemigo se autodestruira

            
            animator.SetBool("getHit", true);
            navMesh.enabled = false;
            //Hacer que vaya parriba tambien
            rb.isKinematic = false;
            rb.AddForce(player.transform.forward * knockBackForce + transform.up * knockUpForce, ForceMode.Impulse);

            StartCoroutine(WaitToDestroy(1.1f));

        }
    }
}
