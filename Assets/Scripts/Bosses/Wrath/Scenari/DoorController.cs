using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private int barsBroken = 0;
    private bool firstTimeOpenDoor = true;
    private bool isOpen = false;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            barsBroken++;
        }

        if (barsBroken >= 2 && firstTimeOpenDoor && !CinematicsController._CINEMATICS_CONTROLLER.isPlayingCinematic)
        {
            CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic("WrathBossOpenDoor");
            firstTimeOpenDoor = false;
            OpenDoor();
            isOpen = true;
        }
    }

    public void OpenDoor()
    {
        animator.SetBool("OpenDoor", true);

    }

    public void CloseDoor() 
    {
        animator.SetBool("OpenDoor", false);
    }

    public void BrokenBar() 
    {
        barsBroken++;
    }

    private void OnTriggerEnter(Collider other)
    {


    }

    private void OnTriggerExit(Collider other)
    {


    }

}
