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
    private BelethHealthController healthController;
    private BelethAudioController audioController;
    private BelethMovementController movementController;
    // Start is called before the first frame update
    void Start()
    {
        healthController = GetComponent<BelethHealthController>();
        audioController = GetComponentInChildren<BelethAudioController>();
        movementController = GetComponent<BelethMovementController>();

        lastCheckPoint = transform.position;
        lastRespawn = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (canSaveRespawnPoint && !movementController.onPlatform && movementController.groundedPlayer)
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
        transform.position = lastRespawn;

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
       
        transform.position = lastCheckPoint;
       

    }

    public void TPPlayer(Vector3 newPos) 
    {
        
        transform.position = newPos;
        
    }

    private IEnumerator WaitForSetNewPoint() {

        SetNewRespawn(transform.position);
        canSaveRespawnPoint = false;
        yield return new WaitForSeconds(timeSetNewRespawn);
        canSaveRespawnPoint = true;


    }


}
