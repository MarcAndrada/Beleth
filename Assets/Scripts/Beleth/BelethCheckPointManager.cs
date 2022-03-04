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

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        healthController = GetComponent<BelethHealthController>();
        lastCheckPoint = transform.position;
        lastRespawn = transform.position;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GoLastCheckPoint();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSaveRespawnPoint && charController.isGrounded)
        {
            StartCoroutine(WaitForSetNewPoint());
        }

        if (transform.position.y < maxFallDistance)
        {
            healthController.Damaged(voidFallDamage);

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

    public void SetNewCheckPoint(Vector3 _newCheckPointPos) {
        lastCheckPoint = _newCheckPointPos;
        Debug.Log("New CheckPoint Setted at " + lastCheckPoint);
        
    }

    public void GoLastCheckPoint()
    {
        transform.position = lastCheckPoint;


    }


    private IEnumerator WaitForSetNewPoint() {

        SetNewRespawn(transform.position);
        canSaveRespawnPoint = false;
        yield return new WaitForSeconds(timeSetNewRespawn);
        canSaveRespawnPoint = true;


    }


}
