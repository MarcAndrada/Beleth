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

    private MeshRenderer meshRenderer;
    private bool gemObtined = false;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }


    private void PlayerGetGem() 
    {
        gemObtined = true;
        specialDoor.canBeTriggered = true;
        meshRenderer.material = whiteMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !gemObtined)
        {
            PlayerGetGem();
        }    
    }


}
