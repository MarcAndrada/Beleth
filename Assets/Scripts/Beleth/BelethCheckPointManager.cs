using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethCheckPointManager : MonoBehaviour
{
    [SerializeField]
    private float timeSetNewRespawn;

    [SerializeField]
    private float maxFallDistance;
    [SerializeField]
    private int voidFallDamage;


    private Vector3 lastRespawn;
    private Vector3 lastCheckPoint;

    private bool canSaveRespawnPoint = true;
    private CharacterController charController;
    private BelethHealthController healthController;
    private BelethAudioController audioController;
    private BelethMovementController movementController;
    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        healthController = GetComponent<BelethHealthController>();
        audioController = GetComponentInChildren<BelethAudioController>();
        movementController = GetComponent<BelethMovementController>();

        lastCheckPoint = transform.position;
        lastRespawn = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (canSaveRespawnPoint && charController.isGrounded && !movementController.onPlatform)
        {
            StartCoroutine(WaitForSetNewPoint());
        }

        if (transform.position.y < lastRespawn.y + maxFallDistance)
        {
            healthController.GetDamage(voidFallDamage, false);

            if (healthController.GetHealthPoints() > 0)
            {
                GoLastRespawn();

            }
        }



    }
    

    public void SetNewRespawn(Vector3 _newRecoverZonePos) {
        lastRespawn = _newRecoverZonePos;

    }

    public void GoLastRespawn() {
        charController.enabled = false;
        transform.position = lastRespawn;
        charController.enabled = true;
    }

    public void SetNewCheckPoint(Vector3 _newCheckPointPos, Animation _anim)
    {
        lastCheckPoint = _newCheckPointPos;
        _anim.Play();
        audioController.soundCont.CheckPointSound();
        //Debug.Log("New CheckPoint Setted at " + lastCheckPoint);
    }

    public void GoLastCheckPoint()
    {
        charController.enabled = false;
        transform.position = lastCheckPoint;
        charController.enabled = true;

    }

    public void TPPlayer(Vector3 newPos) 
    {
        charController.enabled = false;
        transform.position = newPos;
        charController.enabled = true;
    }

    private IEnumerator WaitForSetNewPoint() {

        SetNewRespawn(transform.position);
        canSaveRespawnPoint = false;
        yield return new WaitForSeconds(timeSetNewRespawn);
        canSaveRespawnPoint = true;


    }


}
