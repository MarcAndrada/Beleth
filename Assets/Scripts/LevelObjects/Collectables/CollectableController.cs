using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public int collectableID;
    [SerializeField]
    private GameObject breakObject;

    private SphereCollider coll;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    public void DisableCollectable() 
    {
        coll.enabled = false;
        breakObject.SetActive(false);
    }
}
