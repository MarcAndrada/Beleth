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
    private SphereCollider attackCollider;
    [SerializeField]
    private float maxTimeChasing;
    
    private float attackTranscurse = 0;
    private float attackDuration = 0.2f;
    private bool chasePlayer = false;
    private bool isAttacking = false;
    private NavMeshAgent navMesh;
    private GameObject player;
    private BoxCollider boxColl;
    private Vector3 directionAttack;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        boxColl = GetComponent<BoxCollider>();
    }


    // Update is called once per frame
    void Update()
    {
        if (chasePlayer)
        {

            StartCoroutine(WaitToExplode());

            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack && !isAttacking)
            {
                //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                navMesh.destination = player.transform.position;

            }
            else
            {
                if (directionAttack.x == 0)
                {
                    directionAttack = transform.position;
                }
                isAttacking = true;
                navMesh.enabled = false;
                if (attackTranscurse < attackDuration)
                {
                    attackTranscurse += attackSpeed * Time.deltaTime;
                }
                else
                {
                    attackTranscurse = 0.5f;
                }

                transform.position = Vector3.Lerp(transform.position, player.transform.position, attackTranscurse);

                if (attackTranscurse >= 0.1f)
                {
                    StartCoroutine(SelfDestroy());

                }

            }


        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !chasePlayer)
        {
            player = other.gameObject;
            StartCoroutine(PlayerSeen());
        }

        if (other.tag == "Player" && chasePlayer)
        {
            player.GetComponent<BelethHealthController>().Damaged(1);
        }

    }

    IEnumerator PlayerSeen() {

        boxColl.enabled = false;
        attackCollider.enabled = true;
        transform.LookAt(player.transform);
        yield return new WaitForSeconds(timeToWait);
        chasePlayer = true;
    }


    private void Explosion() {
        // Hacer efectos de explosion

    }

    private IEnumerator SelfDestroy() {


        Explosion();

        yield return new WaitForSeconds(0.1f);
        
        Destroy(gameObject);


    }

    private IEnumerator WaitToExplode() {


        yield return new WaitForSeconds(maxTimeChasing);

        StartCoroutine(SelfDestroy());
    }

    
}
