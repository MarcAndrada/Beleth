using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GuideAIController : MonoBehaviour
{
    [SerializeField]
    private Transform[] placesToGo;
    [SerializeField]
    private GuideStarterPosController startingZone;
    [SerializeField]
    private float maxDistanceFromPlayer;
    [SerializeField]
    private float resetDistanceFromPlayer;
    [SerializeField]
    private bool resetTime;
    [SerializeField]
    private float timeToWait;

    private bool canMove = true;
    [HideInInspector]
    public bool guidingPlayer = false;
    private int index = 0;
    private bool farFromPlayer = false;
    private float timeWaited;
    

    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    // Start is called before the first frame update
    void Start()
    {
        StartGuide();

    }

    // Update is called once per frame
    void Update()
    {

        CheckDistanceFromPlayer();
        CheckIfReset();

        if (!agent.hasPath && canMove)
        {
            SetNextPlace();
            MoveNextPlace();
        }


    }

    private void CheckDistanceFromPlayer() 
    {
        if (Vector3.Distance(startingZone.player.transform.position, transform.position) > maxDistanceFromPlayer)
        {
            agent.isStopped = true;
            canMove = false;
            farFromPlayer = true;
        }
        else if (agent.isStopped)
        {
            agent.isStopped = false;
            canMove = true;
            farFromPlayer = false;

        }



        if (Vector3.Distance(startingZone.player.transform.position, transform.position) > resetDistanceFromPlayer)
        {
            StartCoroutine(RestartPos());
            farFromPlayer = false;
        }

    }
    private void CheckIfReset() 
    {
        if (farFromPlayer && resetTime)
        {
            timeWaited += Time.deltaTime;

            if (timeWaited >= timeToWait)
            {
                StartCoroutine(RestartPos());
                timeWaited = 0;
                farFromPlayer = false;
            }
        }
        else
        {
            timeWaited = 0;
        }


    }

    private void SetNextPlace()
    {

        index++;
        if (index > placesToGo.Length - 1)
        {
            
            StartCoroutine(RestartPos());
        }

    }
    private void MoveNextPlace()
    {
        if (!agent.isStopped)
        {
            agent.SetDestination(placesToGo[index].position);
        }
    }


    IEnumerator RestartPos() 
    {
        //Hacer que ahora se pueda interactuar con el objeto que queremos y reiniciar el guia
        canMove = false;
        transform.position = placesToGo[0].position;
        index = 0;
        guidingPlayer = false;
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);

    }


    public void StartGuide() 
    {
        if (!canMove)
        {
            canMove = true;
            MoveNextPlace();
            guidingPlayer = true;
        }
        
    }

}
