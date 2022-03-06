using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentController : MonoBehaviour
{
    [SerializeField]
    private GameObject trident;
    [SerializeField]
    private ParticleSystem wrathParticles;

    private CapsuleCollider tridentCollider;
    // Start is called before the first frame update
    void Start()
    {
        tridentCollider = trident.GetComponent<CapsuleCollider>();
    }

    public void StartTridentAttack() {
        tridentCollider.enabled = true;
        if (trident.tag == "Wrath")
        {
            Instantiate(wrathParticles, trident.transform.position, Quaternion.Euler(-transform.forward));
        }
    }

    public void EndTridentAttack()
    {
        tridentCollider.enabled = false;
    }

    public void ChangeTridentTag(string _newTag) {
        trident.tag = _newTag;
    }

}
