using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{

    [SerializeField]
    private GameObject spike;
    [SerializeField]
    private float goDownSpeed;
    private Vector3 starterSpikePos;
    private float moveProgress = 0;
    private bool goingDown = false;
    private bool goingUp = false;
    // Start is called before the first frame update
    void Start()
    {
        starterSpikePos = spike.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGoDown();
        CheckIfGoUp();
    }


    private void CheckIfGoDown() 
    {
        if (goingDown)
        {
            moveProgress += goDownSpeed * Time.deltaTime;

            spike.transform.localPosition = Vector3.Lerp(starterSpikePos,  new Vector3(starterSpikePos.x, 0, starterSpikePos.z) , moveProgress);

            if (moveProgress >= 1)
            {
                moveProgress = 0;
                goingDown = false;
                goingUp = true;
            }
        }
    }

    private void CheckIfGoUp() 
    {
        if (goingUp)
        {
            moveProgress += Time.deltaTime;

            spike.transform.localPosition = Vector3.Lerp(new Vector3(starterSpikePos.x, 0 , starterSpikePos.z), starterSpikePos, moveProgress);
            if (moveProgress >= 1)
            {
                moveProgress = 0;
                goingDown = false;
                goingUp = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Baja
            goingDown = true;
            goingUp = false;
        }
    }

}
