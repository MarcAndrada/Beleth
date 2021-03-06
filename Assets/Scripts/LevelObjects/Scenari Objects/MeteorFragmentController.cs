using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFragmentController : MonoBehaviour
{
    [Header("Explosion Variables")]
    [SerializeField]
    float radius;
    [SerializeField]
    float force;
    [SerializeField]
    private int spawnRate;

    [Header("GameObjects")]
    [SerializeField]
    GameObject hole;
    [SerializeField]
    GameObject brokenObject;
    [SerializeField]
    GameObject wrathRock;
    private WrathBossStateController bossController;

    private void Awake()
    {
        bossController = FindObjectOfType<WrathBossStateController>();
    }

    public void Break(Vector3 rayHit)
    {   
        Instantiate(hole, rayHit, Quaternion.Euler(0,0,0));

        Instantiate(brokenObject, new Vector3(transform.position.x + 2.7f, transform.position.y + .3f, transform.position.z), Quaternion.Euler(0, 0, 0));

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody fragmentrb = hit.GetComponent<Rigidbody>();

            if (fragmentrb != null)
                fragmentrb.AddExplosionForce(force, transform.position, radius);
        }

        int i = 1;
        if (Random.Range(0, spawnRate) == i) 
        {
            GameObject thisMeteor;
            thisMeteor = Instantiate(wrathRock, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

            thisMeteor.GetComponent<ThrowRockController>().SetTarget(bossController.transform);
        } 

        Destroy(gameObject);
    }

    public void BreakOnPlayerHead() 
    {
        Instantiate(brokenObject, new Vector3(transform.position.x + 2.7f, transform.position.y + .3f, transform.position.z), Quaternion.Euler(0, 0, 0));
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody fragmentrb = hit.GetComponent<Rigidbody>();

            if (fragmentrb != null)
                fragmentrb.AddExplosionForce(force, transform.position, radius);
        }

        Destroy(gameObject);

    }
}
