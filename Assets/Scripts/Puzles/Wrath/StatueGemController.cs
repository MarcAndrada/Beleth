using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueGemController : MonoBehaviour
{
    [SerializeField]
    private DoorController bossDoor;
    [SerializeField]
    private WrathExplosionController specialDoor;
    [SerializeField]
    private Material whiteMat;
    [SerializeField]
    private string cinematicName;
    private MeshRenderer meshRenderer;
    private bool gemObtined = false;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }


    private void PlayerGetGem() 
    {
        gemObtined = true;
        specialDoor.canBeTriggered = true;
        meshRenderer.material = whiteMat;
        //Hacer cinematica
        CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(cinematicName);
        bossDoor.BrokenBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !gemObtined)
        {
            PlayerGetGem();
        }    
    }


}
