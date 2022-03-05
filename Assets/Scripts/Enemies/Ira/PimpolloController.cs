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
    private BoxCollider boxColl;

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

            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack && !isAttacking && navMesh.enabled == true)
            {
                //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                navMesh.destination = player.transform.position;

            }
            else
            {
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


        }
    }


   

    IEnumerator PlayerSeen()
    {

        boxColl.enabled = false;
        transform.LookAt(player.transform);
        yield return new WaitForSeconds(timeToWait);
        chasePlayer = true;
    }


    private void Explosion()
    {
        // Instanciar la explosion
        Instantiate(explosion, transform.position, transform.rotation);
        Debug.Log("Exploto");
    }

    private void SelfDestroy()
    {
        
        Explosion();
        Destroy(gameObject);

    }

    private IEnumerator WaitToExplode()
    {


        yield return new WaitForSeconds(maxTimeChasing);

        SelfDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !chasePlayer)
        {
            player = other.gameObject;
            StartCoroutine(PlayerSeen());
        }

        if (other.tag == "Trident" && chasePlayer)
        {
            Debug.Log("Me iso pupa");
            SelfDestroy();


        }


    }


}
