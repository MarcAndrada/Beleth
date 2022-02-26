using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BlindEnemieController : MonoBehaviour
{
    [SerializeField]
    private Transform[] placesToGo;
    [Tooltip("Si esta activado cuando llegue al ultimo punto volvera a empezar desde el 1 si no ira marcha atras \n Ej: El ultimo punto es el 8, en vez de ir al 1 va al 7 despues al 6 ...")]
    [SerializeField]
    private bool restartWhenEnd;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float cdDuration;
    [SerializeField]
    private GameObject testMeshScale;


    private int index = 0;
    private NavMeshAgent agent;
    private bool ascending = true;
    private bool canAttack = true;
    private float attackProcess = 1;
    private GameObject player;
    private bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveNextPlace();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.hasPath && !isAttacking)
        {
            SetNextPlace();
            MoveNextPlace();
        }
        else
        {
            CheckAttack();
        }

    }

    private void MoveNextPlace() {

        agent.SetDestination(placesToGo[index].position);

    }

    private void SetNextPlace() {
        if (restartWhenEnd)
        {
            index++;
            if (index > placesToGo.Length - 1)
            {
                index = 0;
            }
        }
        else
        {
            if (ascending)
            {
                index++;
                if (index == placesToGo.Length - 1)
                {
                    ascending = false;
                }
            }
            else
            {
                index--;

                if (index == 0)
                {
                    ascending = true;
                }
            }
        }
    }

    private void CheckAttack() {
       
        if (isAttacking)
        {
            agent.SetDestination(transform.position);
            if (canAttack)
            {
                agent.destination = transform.position;
                if (attackProcess < 5)
                {
                    attackProcess += attackSpeed * Time.deltaTime;
                }
                else
                {
                    attackProcess = 5;
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
                    MoveNextPlace();
                }
            }

            testMeshScale.transform.localScale = new Vector3(attackProcess, attackProcess, attackProcess);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            isAttacking = true;
        }
    }
}
