using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PimpolloController : MonoBehaviour
{

    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private float distanceToAttack;
    [Tooltip("Velocidad en la que hara el salto hacia el player")]
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float maxTimeChasing;

    [SerializeField]
    private SphereCollider attackCollider;
    [SerializeField]
    private GameObject explosion;
    
    private float attackTranscurse = 0;
    private float attackDuration = 0.15f;
    private bool chasePlayer = false;
    private bool isAttacking = false;
    private NavMeshAgent navMesh;
    private GameObject player;
    [SerializeField]
    private SphereCollider detectionColl;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        if (chasePlayer)
        {

            //Si no esta lo suficiente mente cerca del jugador perseguira al player en caso que este lo sufucientemente cerca este le atacara

            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack && !isAttacking && navMesh.enabled == true)
            {
                navMesh.destination = player.transform.position;

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
        yield return new WaitForSeconds(timeToWait);
        animator.SetTrigger("Chase");
        chasePlayer = true;
        StartCoroutine(WaitToExplode());

    }

    private void AttackPlayer() {

        attackCollider.enabled = true;
        isAttacking = true;
        navMesh.enabled = false;

        if (attackTranscurse < attackDuration)
        {
            attackTranscurse += attackSpeed * Time.deltaTime;
        }
        else
        {
            SelfDestroy();
        }

        transform.position = Vector3.LerpUnclamped(transform.position, player.transform.position, attackTranscurse);
    }

    private void Explosion()
    {
        // Instanciar las particulas de la explosion
        Instantiate(explosion, transform.position, transform.rotation);
    }

    private void SelfDestroy()
    {
        // Instancia las particulas de explosion y se autodestruye
        Explosion();
        Destroy(gameObject);

    }

    private IEnumerator WaitToExplode()
    {
        // Tras empezar a perseguir al jugador si no lo alcanza en el tiempo que tiene este se autidestruira

        yield return new WaitForSeconds(maxTimeChasing);

        SelfDestroy();
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

            SelfDestroy();


        }


    }


}
