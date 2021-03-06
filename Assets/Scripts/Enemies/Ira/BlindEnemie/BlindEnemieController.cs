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
    [SerializeField]
    private float maxScale;

    private NavMeshAgent agent;
    private int index = 0;
    private bool ascending = true;
    private bool canAttack = true;
    private float attackProcess = 1;
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
        if (!agent.hasPath)
        {
            SetNextPlace();
            MoveNextPlace();
        }
        

            CheckAttack();
        
    }

    private void MoveNextPlace() 
    {
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
                    MoveNextPlace();
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
