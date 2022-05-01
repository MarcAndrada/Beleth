using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpiController : MonoBehaviour
{
    public enum SerpiStates {IDLE, ATTACKING, TRAPPED, DEAD };

    public SerpiStates currentSerpiState = SerpiStates.IDLE;

    [SerializeField]
    private CapsuleCollider[] capsuleColl;
    [SerializeField]
    private SphereCollider[] sphereColl;
    [SerializeField]
    private BoxCollider attackRangeColl;
    [SerializeField]
    private string[] serpiCinematicNames;
    [SerializeField]
    private DoorController bossDoor;
    private Animator animator;
    private bool[] activators = new bool[2];

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        CheckTotalChains();
    }

    private void CheckTotalChains() 
    {
        if (activators[0] && activators[1])
        {
            //Hacer cinematica de serpi atrapada

            TrapSerpi();
            CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(serpiCinematicNames[3]);
            activators[0] = false;
            activators[1] = false;

        }
        else
        {
            if (activators[0])
            {
                //Hacer cinematica viendo como en la izquierda pasa algo
                CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(serpiCinematicNames[1]);
                activators[0] = false;
            }
            else if (activators[1])
            {
                //Hacer cinematica viendo como en la derecha pasa algo
                CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(serpiCinematicNames[2]);
                activators[1] = false;
            }
            else
            {
                activators[0] = false;
                activators[1] = false;
            }
            
        }
    }
    public void ActivateChain(int _chainID) 
    {
        activators[_chainID] = true;
    }
    public void TrapSerpi() 
    {
        if (currentSerpiState != SerpiStates.TRAPPED)
        {
            animator.SetBool("Trapped", true);
            currentSerpiState = SerpiStates.TRAPPED;
        }
    }
    public void UnTrapSerpi() 
    {
        if (currentSerpiState == SerpiStates.TRAPPED)
        {
            animator.SetBool("Trapped", false);
            currentSerpiState = SerpiStates.IDLE;
        }
    }

    private void SerpiStartAttack() 
    {
        currentSerpiState = SerpiStates.ATTACKING;
        animator.SetTrigger("Attack");
        foreach (var item in capsuleColl)
        {
            item.isTrigger = true;
        }

        foreach (var item in sphereColl)
        {
            item.isTrigger = true;
        }
    }
    public void SerpiStopAttack() 
    {
        currentSerpiState = SerpiStates.IDLE;
        animator.ResetTrigger("Attack");
        foreach (var item in capsuleColl)
        {
            item.isTrigger = false;
        }

        foreach (var item in sphereColl)
        {
            item.isTrigger = false;
        }
    }
    public void EnableSerpiAttackTrigger()
    {
        attackRangeColl.enabled = true;
        
    }
    public void DisableSerpiAttackTrigger()
    {
        attackRangeColl.enabled = true;

    }
    public void SerpiDead() 
    {
        if (currentSerpiState != SerpiStates.DEAD)
        {
            animator.SetTrigger("Dead");
            CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(serpiCinematicNames[0]);
            currentSerpiState = SerpiStates.DEAD;
            bossDoor.BrokenBar();
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && currentSerpiState == SerpiStates.IDLE)
        {
            SerpiStartAttack();
            attackRangeColl.enabled = false;
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && currentSerpiState == SerpiStates.ATTACKING)
        {
            other.gameObject.GetComponent<BelethHealthController>().GetDamage(1);
            other.gameObject.GetComponent<BelethMovementController>().AddImpulse(-transform.right, 20);
        }
    }




}
