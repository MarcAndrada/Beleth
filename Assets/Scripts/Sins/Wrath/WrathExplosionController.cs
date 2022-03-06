using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathExplosionController : MonoBehaviour
{
    [SerializeField]
    bool wrathActive;
    [SerializeField]
    float timeToExplode;
    [SerializeField]
    float radius;
    [SerializeField]
    float strenght;
    [SerializeField]
    float upStrenght;

    private Material normalMaterial;
    [SerializeField]
    private Material wrathMaterial;

    private WrathExplosionController explosionController;
    private BlindEnemieController blindEnemie;
    private MeshRenderer mesh;
    private Rigidbody rigidB;

    void Start()
    {
        blindEnemie = GetComponent<BlindEnemieController>();
        mesh = GetComponentInChildren<MeshRenderer>();
        explosionController = GetComponent<WrathExplosionController>();
        rigidB = GetComponentInChildren<Rigidbody>();
        normalMaterial = mesh.material;
        wrathActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("g"))
        //{
        //    if (!wrathActive)
        //    {

        //        wrathActive= true;
        //    }
        //    else
        //    {
        //        WrathExplosion();
        //        wrathActive = false;
        //    }
        //}
    }


    public void WrathExplosion()
    {
        //Implosione

        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        foreach(Collider hit in colliders)
        {

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb != rigidB)
            {
                rb.AddExplosionForce(strenght, explosionPosition, radius, upStrenght, ForceMode.Impulse);
            }
        }

        
        SetNormal();

    }



    private void SetWrath(BelethSinsController _player)
    {

        mesh.material = wrathMaterial;
        _player.AddWrathObject(explosionController);
    }

    private void SetNormal()
    {

        mesh.material = normalMaterial;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wrath")
        {
            SetWrath(other.gameObject.GetComponentInParent<BelethSinsController>());
        }


    }
}
