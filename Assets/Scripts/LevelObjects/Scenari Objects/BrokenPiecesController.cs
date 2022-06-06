using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiecesController : MonoBehaviour
{
    [SerializeField]
    GameObject BrokePiece;
    [SerializeField]
    public GameObject existingBrokenPiece;

    [SerializeField] float radius;
    [SerializeField] float force;

    bool can;

    public void Break()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody fragmentrb = hit.GetComponentInChildren<Rigidbody>();

            if (fragmentrb != null)
                fragmentrb.AddExplosionForce(force, transform.position, radius);
        }

        if (!can)
        {
            if (existingBrokenPiece != null)
            {
                existingBrokenPiece.SetActive(true);
            }
            else
            {
                Instantiate(BrokePiece);
            }            
            can = true;
            Destroy(gameObject);
        }

    }
}
