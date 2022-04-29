using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethCheckPointManager : MonoBehaviour
{
    [SerializeField]
    private float timeSetNewRespawn;

    [SerializeField]
    private Transform[] respawnPointCheckers;

   

    private float timeWaitedToNewRespawn = 0;
    private bool canSaveRespawnPoint = true;
    private Vector3 lastRespawn;
    private Vector3 lastCheckPoint;
    private BelethMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<BelethMovementController>();
    }

    // Start is called before the first frame update
    void Start()
    {

        lastCheckPoint = transform.position;
        lastRespawn = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canSaveRespawnPoint)
        {
            timeWaitedToNewRespawn += Time.deltaTime;

            if (timeWaitedToNewRespawn >= timeSetNewRespawn)
            {
                canSaveRespawnPoint = true;
                timeWaitedToNewRespawn = 0;
            }
        }

        if (canSaveRespawnPoint && !movementController.onPlatform && movementController.groundedPlayer)
        {
            SetNewRespawn(transform.position);
        }

    }
    
    public void SetNewRespawn(Vector3 _newRecoverZonePos) {

        bool canSave = true;

        foreach (Transform item in respawnPointCheckers)
        {
            if (!Physics.Raycast(new Ray(item.position, -transform.up), movementController.maxFloorCheckDistance, movementController.walkableLayers))
            {
                canSave = false;
                break;
            }
        }

        if (canSave)
        {
            lastRespawn = _newRecoverZonePos;
            canSaveRespawnPoint = false;
        }
        

    }

    public void GoLastRespawn() {
        transform.position = lastRespawn;
    }

    public void SetNewCheckPoint(Vector3 _newCheckPointPos, Animation _anim)
    {
        lastCheckPoint = _newCheckPointPos;
        _anim.Play();
        SoundManager._SOUND_MANAGER.CheckPointSound();
        //Debug.Log("New CheckPoint Setted at " + lastCheckPoint);
    }

    public void GoLastCheckPoint()
    {
       
        transform.position = lastCheckPoint;
       

    }

}
