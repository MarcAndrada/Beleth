using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentController : MonoBehaviour
{
    [SerializeField]
    private GameObject trident;
    [SerializeField]
    private Transform tridentSocket;
    [SerializeField]
    ParticleSystem[] wrathParticles;
    [SerializeField]
    private Transform backSocket;
    [SerializeField]
    private Transform handSocket;
    [SerializeField]
    private Transform attackSocket;
    [SerializeField]
    private Transform wrathAttackSocket;
    
    [SerializeField]
    private float tridentMoveSpeed;
    [SerializeField]
    private float timeToWaitTridentPos;

    private float tridentPlaceState = 0;
    private bool moving = false;
    private bool countingForTridentPos = false;
    private float timeWaitedTridentPos = 0;
    private CapsuleCollider tridentCollider;
    private BelethAnimController belethAnimController;
    // Start is called before the first frame update
    void Start()
    {
        tridentCollider = trident.GetComponent<CapsuleCollider>();
        belethAnimController = GetComponentInParent<BelethAnimController>();
        //wrathParticles.SetActive(false);
    }

    private void Update() {

        if (moving)
        {
            tridentPlaceState += tridentMoveSpeed * Time.deltaTime;
            trident.transform.localPosition = Vector3.Lerp(trident.transform.localPosition, new Vector3(0, 0, 0), tridentPlaceState);
            trident.transform.localRotation = Quaternion.Lerp(trident.transform.localRotation, Quaternion.Euler(0, 0, 0), tridentPlaceState);
            if (tridentPlaceState >= 1)
            {
                moving = false;
                tridentPlaceState = 0;
            }

        }

        TimerTridentPos();

    }

    public void StartTridentAttack() {
        tridentCollider.enabled = true;

        if (trident.tag == "Wrath")
        {
            foreach (var item in wrathParticles)
            {
                item.Play();
            }
        }
    }

    public void EndTridentAttack()
    {
        tridentCollider.enabled = false;
    }

    public void ChangeTridentTag(string _newTag) {
        trident.tag = _newTag;
    }

    public void SetTridentPos(int _currentPos) {
        switch (_currentPos)
        {
            case 0:
                trident.transform.SetParent(backSocket);
                break;
            case 1:
                trident.transform.SetParent(attackSocket);
                break;
            case 2:
                trident.transform.SetParent(wrathAttackSocket);
                break;
            case 3:
                trident.transform.SetParent(handSocket);
                break;
            default:
                break;
        }
        tridentPlaceState = 0;
        moving = true;
    }

    private void TimerTridentPos() {

        if (countingForTridentPos)
        {
            timeWaitedTridentPos += Time.deltaTime;

            if (timeWaitedTridentPos >= timeToWaitTridentPos)
            {
                SetTridentPos(0);
                countingForTridentPos = false;
                timeWaitedTridentPos = 0;
            }
        }
    }

    public void ResetTridentPosTimer() {
        countingForTridentPos = true;
        timeWaitedTridentPos = 0;
    }


}
