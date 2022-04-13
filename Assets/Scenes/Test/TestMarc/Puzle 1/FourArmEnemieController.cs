using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourArmEnemieController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] arms;
    [SerializeField]
    private Animator swordAnimator;
    [SerializeField]
    private float animDuration;

    private Animator[] armAnimators;
    // Start is called before the first frame update
    void Start()
    {
        armAnimators = new Animator[arms.Length];

        for (int i = 0; i < arms.Length; i++)
        {
            armAnimators[i] = arms[i].GetComponent<Animator>();
        } 


    }

    // Update is called once per frame
    void Update()
    {
        int upArms = 0;
        for (int i = 0; i < armAnimators.Length; i++)
        {
            if (armAnimators[i].GetBool("Activated")) {
                upArms++;
                
            }
            

        }

        if (upArms == armAnimators.Length)
        {
            //Tirar la espada
            swordAnimator.SetTrigger("ArmsTogether");
        }
    }

    public float ActivateArm(int _armIndex) 
    {
        armAnimators[_armIndex].SetBool("Activated", true);
        return animDuration;
    }

    public void DesactivateArm(int _armIndex)
    {
        armAnimators[_armIndex].SetBool("Activated", false);
    }

}
