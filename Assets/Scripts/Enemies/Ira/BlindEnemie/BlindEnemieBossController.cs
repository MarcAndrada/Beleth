using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindEnemieBossController : MonoBehaviour
{
    [SerializeField]
    private float maxDistanceToReach;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float cdDuration;
    [SerializeField]
    private GameObject testMeshScale;
    [SerializeField]
    private float maxScale;

    private Vector3 starterPos;
    private Vector3 pointToReach;
    private bool going = true;

    private bool canAttack = true;
    private float attackProcess = 1;
    private bool isAttacking = false;

    private NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        starterPos = transform.position;
        pointToReach = transform.position + transform.forward * maxDistanceToReach;

        navAgent.SetDestination(pointToReach);
    }

    // Update is called once per frame
    void Update()
    {
        CheckRachedPoint();

        CheckAttack();
    }

    private void CheckRachedPoint() 
    {
        if (navAgent.velocity == Vector3.zero && !navAgent.hasPath)
        {
            if (going)
            {
                navAgent.SetDestination(starterPos);
                going = false;
            }
            else
            {
                //Destruir
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void CheckAttack()
    {

        if (isAttacking)
        {
            
            if (canAttack)
            {
                
                if (attackProcess < maxScale)
                {
                    attackProcess += attackSpeed * Time.deltaTime;
                }
                else
                {
                    attackProcess = maxScale;
                    canAttack = false;
                }


            }
            else
            {
                if (attackProcess > 1)
                {
                    attackProcess -= cdDuration * Time.deltaTime;
                }
                else
                {
                    attackProcess = 1;
                    canAttack = true;
                    isAttacking = false;
                }
            }

            testMeshScale.transform.localScale = new Vector3(attackProcess, attackProcess, attackProcess);
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isAttacking)
        {
            isAttacking = true;
            other.gameObject.GetComponent<BelethHealthController>().GetDamage(1);

        }
    }

}
