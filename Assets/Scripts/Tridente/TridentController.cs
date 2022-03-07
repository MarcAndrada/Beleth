using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentController : MonoBehaviour
{
    [SerializeField]
    private GameObject trident;
    [SerializeField]
    private ParticleSystem wrathParticles;
    [SerializeField]
    private Transform handSocket;
    [SerializeField]
    private Transform backSocket;

    private CapsuleCollider tridentCollider;
    // Start is called before the first frame update
    void Start()
    {
        tridentCollider = trident.GetComponent<CapsuleCollider>();
    }

    private void Update() {
    
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

    public void SetTridentOnHand() {
        trident.transform.SetParent(handSocket);
    }

    public void SetTridentOnBack() {
        trident.transform.SetParent(handSocket);

    }


}
