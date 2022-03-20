using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathExplosionController : MonoBehaviour
{
    [SerializeField]
    bool moveHimself;
    [SerializeField]
    float timeToExplode;
    [SerializeField]
    float radius;
    [SerializeField]
    float strenght;
    [SerializeField]
    float upStrenght;
    [SerializeField]
    private float playerImpulse;

    private Material normalMaterial;
    [SerializeField]
    private Material wrathMaterial;

    private WrathExplosionController explosionController;
    private MeshRenderer mesh;
    private Rigidbody rigidB;

    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        explosionController = GetComponent<WrathExplosionController>();
        rigidB = GetComponentInChildren<Rigidbody>();
        normalMaterial = mesh.material;
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
            if (hit.gameObject.tag == "Player" && playerImpulse > 0)
            {
                hit.gameObject.GetComponentInChildren<BelethMovementController>().AddImpulse(playerImpulse);
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if(hit.gameObject.tag == "Breakable")
            {
                hit.gameObject.GetComponent<BrokenWallController>().Break();

            }

            if (rb != null && rb != rigidB || rb != null && moveHimself)
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
