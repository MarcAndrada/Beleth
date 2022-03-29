using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathExplosionController : MonoBehaviour
{
    [Header("Variables")]
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

    [Header("VFX")]
    [SerializeField]
    GameObject wrathVFX;

    //[SerializeField]
    //private Material wrathMaterial;

    private WrathExplosionController explosionController;
    //private MeshRenderer mesh;
    //private SkinnedMeshRenderer skinnedMesh;
    private Rigidbody rigidB;

    void Start()
    {
        //mesh = GetComponentInChildren<MeshRenderer>();
        //if (mesh != null)
        //{
        //    normalMaterial = mesh.material;
        //
        //}
        //else
        //{
        //    skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        //    normalMaterial = skinnedMesh.material;
        //}
       
        explosionController = GetComponent<WrathExplosionController>();
        rigidB = GetComponentInChildren<Rigidbody>();

        if(wrathVFX) wrathVFX.SetActive(false);
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

            if (hit.gameObject.tag == "Boss")
            {
                hit.gameObject.GetComponentInParent<WrathBossStateController>().GetDamage(15f, gameObject);
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
        //if (mesh != null)
        //{
        //    mesh.material = wrathMaterial;
        //}
        //else
        //{
        //    skinnedMesh.material = wrathMaterial;
        //
        //}

        if (wrathVFX) wrathVFX.SetActive(true);

        _player.AddWrathObject(explosionController);
    }

    private void SetNormal()
    {
        //if (mesh != null)
        //{
        //    mesh.material = normalMaterial;
        //}
        //else
        //{
        //    skinnedMesh.material = normalMaterial;
        //}

        if (wrathVFX) wrathVFX.SetActive(false);
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
