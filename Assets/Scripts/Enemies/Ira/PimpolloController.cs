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
    [SerializeField]
    private float attackSpeed;

    private float attackTranscurse = 0;
    private float attackDuration = 0.2f;
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
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack && !isAttacking)
            {
                //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                navMesh.destination = player.transform.position;

            }
            else
            {
                isAttacking = true;
                navMesh.enabled = false;
                if (attackTranscurse < attackDuration)
                {
                    attackTranscurse += attackSpeed * Time.deltaTime;
                }
                else
                {
                    attackTranscurse = 1;
                }

                transform.position = Vector3.Lerp(transform.position, player.transform.position, attackTranscurse);

                if (attackTranscurse >= 1)
                {
                    //TODO Dañar al player

                    Destroy(gameObject);

                }

            }


        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            StartCoroutine(PlayerSeen());
        }
    }


    IEnumerator PlayerSeen() {

        boxColl.enabled = false;
        transform.LookAt(player.transform);
        yield return new WaitForSeconds(timeToWait);
        chasePlayer = true;
    }
}
