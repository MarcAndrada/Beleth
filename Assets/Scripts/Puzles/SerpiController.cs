using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpiController : MonoBehaviour
{
    [SerializeField]
    private string serpiDeadCinematic;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SerpiDead();

        }
    }


    public void SerpiDead() 
    {
        animator.SetTrigger("Dead");
        CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(serpiDeadCinematic);
    }

}
