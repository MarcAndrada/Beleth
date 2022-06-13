using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDropController : MonoBehaviour
{
    [SerializeField]
    public GameObject existantStone;

    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private Transform initPos;
    [SerializeField]
    private float dropForce;
    [SerializeField]
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!existantStone)
        {
            existantStone = Instantiate(stonePrefab, initPos.position, Quaternion.identity);
            existantStone.GetComponent<ThrowRockController>().target = target;
            existantStone.GetComponent<Rigidbody>().AddForce(existantStone.transform.forward * dropForce, ForceMode.Impulse);
        }  
    }


}
