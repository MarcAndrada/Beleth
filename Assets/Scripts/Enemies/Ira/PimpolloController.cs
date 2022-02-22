using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PimpolloController : MonoBehaviour
{
    private NavMeshAgent navMesh;

    [SerializeField]
    private Vector3[] PlaceToGo;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();

        navMesh.destination = PlaceToGo[0];

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
