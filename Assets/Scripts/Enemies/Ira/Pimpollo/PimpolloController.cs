using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PimpolloController : MonoBehaviour
{

    [Header("Hiding")]
    [SerializeField]
    private bool hide;
    [SerializeField]
    private GameObject meshPimpollo;
    [SerializeField]
    private SphereCollider unHideCollider;


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

    [Header("Damaged")]
    [SerializeField]
    private float knockBackForce = 6;
    [SerializeField]
    private float knockUpForce = 0.5f;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystem exclamationVfx;
    [SerializeField]
    private Transform exclamationSocket;
    [SerializeField]
    private ParticleSystem dieExplosionVfx;


    [Header("Sound")]
    



    private float attackTranscurse = 0;
    private bool chasePlayer = false;
    private bool isAttacking = false;
    private bool destroying = false;
    private bool hitted = false;

    private NavMeshAgent navMesh;
    private GameObject player;
    private Animator animator;
    private Rigidbody rb;
    private AudioSource audioSource;


    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        if (hide)
        {
            animator.SetBool("Hide", true);
        }
        else
        {
            unHideCollider.enabled = false;
        }
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


    private void AttackPlayer() {
        if (!hitted)
        {

        
            transform.LookAt(player.transform, Vector3.up);
            attackCollider.enabled = true;
            isAttacking = true;
            navMesh.enabled = false;
            if (attackTranscurse <= 0)
            {
                SoundManager._SOUND_MANAGER.PimpolloJumpSound(audioSource);

            }

            if (attackTranscurse < maxAttackPercent)
            {
                attackTranscurse += attackSpeed * Time.deltaTime;
            }
            else
            {
                if (!destroying)
                {
                    SelfDestroy();

                    SoundManager._SOUND_MANAGER.WrathBossEnemieCircleSound(audioSource);
                    Destroy(gameObject);
                }
            }

            transform.position = Vector3.LerpUnclamped(transform.position, player.transform.position, attackTranscurse);
        }
    }
    private void SelfDestroy()
    {
        // Instancia las particulas de explosion y se autodestruye
        Instantiate(dieExplosionVfx, transform.position, transform.rotation);
        destroying = true;
    }
    private void Damaged()
    {
        // Si es golpeado por el tridente del player el enemigo hara la animacion de recibir da?o y se le a?adria fuerza atras
        animator.SetBool("getHit", true);
        navMesh.enabled = false;
        hitted = true;
        //Hacer que vaya parriba tambien
        rb.isKinematic = false;
        rb.AddForce(transform.position - player.transform.position * knockBackForce + transform.up * knockUpForce, ForceMode.Impulse);
        StartCoroutine(WaitToDeathSound(0.8f));
        StartCoroutine(WaitToDestroy(1.1f));
    }


    private IEnumerator PlayerSeen()
    {
        //Desactiva la colision que detecta si el player esta en el area y tras esperar un tiempo empieza a perseguir al player

        detectionColl.enabled = false;
        transform.LookAt(player.transform);
        Instantiate(exclamationVfx, exclamationSocket.position, exclamationSocket.rotation);
        animator.SetTrigger("Chase");
        yield return new WaitForSeconds(timeToWait);
        chasePlayer = true;
        StartCoroutine(WaitToExplode());
        SoundManager._SOUND_MANAGER.PimpolloChaseSound(audioSource);
    }

    private IEnumerator WaitToExplode()
    {
        // Tras empezar a perseguir al jugador si no lo alcanza en el tiempo que tiene este se autidestruira
        yield return new WaitForSeconds(maxTimeChasing);
        SelfDestroy();
        StartCoroutine(WaitToDestroy(0));
    }

    private IEnumerator WaitToDeathSound(float _timeToWait) 
    {
        yield return new WaitForSeconds(_timeToWait);
        SoundManager._SOUND_MANAGER.PimpolloDeathSound(audioSource);
    }
    private IEnumerator WaitToDestroy(float _timeToWait) 
    {
        yield return new WaitForSeconds(_timeToWait);
        Destroy(gameObject);
    }
    private IEnumerator UnHide()
    {
        unHideCollider.enabled = false;
        animator.SetBool("Hide", false);
        yield return new WaitForSeconds(0.2f);
        hide = false;
        SoundManager._SOUND_MANAGER.PimpolloUnHideSound(audioSource);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && hide)
        {
            StartCoroutine(UnHide());

        }

        if (other.tag == "Player" && !chasePlayer && !hide)
        {
            //En caso de que el player entre en la zona de interaccion empezara a perseguirle
            player = other.gameObject;
            StartCoroutine(PlayerSeen());
        }

        if (other.tag == "Trident" && chasePlayer || other.tag == "Wrath" && chasePlayer)
        {
            Damaged();
        }
    }
}
