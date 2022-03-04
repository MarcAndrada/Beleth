using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentController : MonoBehaviour
{
    [SerializeField]
    private GameObject trident;


    private CapsuleCollider tridentCollider;
    // Start is called before the first frame update
    void Start()
    {
        tridentCollider = trident.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTridentAttack() {
        tridentCollider.enabled = true;
    }

    public void EndTridentAttack()
    {
        tridentCollider.enabled = false;
    }

}
