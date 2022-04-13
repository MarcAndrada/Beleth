using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideStarterPosController : MonoBehaviour
{

    [SerializeField]
    private GameObject ghost;

    [HideInInspector]
    public GameObject player;
    private GuideAIController guideAIController;

    private void Awake()
    {
        guideAIController = ghost.GetComponent<GuideAIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player == null)
            {
                player = other.gameObject;
            }

            ghost.SetActive(true);
            guideAIController.StartGuide();
        }
    }


}
