using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselController : MonoBehaviour
{
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private bool isRandom;
    [SerializeField]
    [Range(0, 5)]
    private int coinsIntoVassel;
    [SerializeField]
    private float coinSpawnOffset;
    private BrokenPiecesController brokenPieces;

    private void Awake()
    {
        brokenPieces = GetComponent<BrokenPiecesController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isRandom)
        {
            int randNum = Random.Range(0, 101);
            if (randNum >= 0 && randNum < 30)
            {
                coinsIntoVassel = 0;
            }
            else if (randNum >= 30 && randNum < 55)
            {
                coinsIntoVassel = 1;
            }
            else if (randNum >= 55 && randNum < 75)
            {
                coinsIntoVassel = 2;
            }
            else if (randNum >= 75 && randNum < 90)
            {
                coinsIntoVassel = 3;
            }
            else if (randNum >= 90 && randNum < 97)
            {
                coinsIntoVassel = 4;
            }
            else if (randNum >= 97 && randNum <= 100)
            {
                coinsIntoVassel = 5;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trident" || other.gameObject.tag == "Wrath")
        {
            BreakVassel();
        }
    }


    private void BreakVassel() 
    {
        for (int i = 0; i < coinsIntoVassel; i++)
        {
            switch (i)
            {
                case 0:
                    Instantiate(coin, transform.position, transform.rotation);
                    break;
                case 1:
                    Instantiate(coin, new Vector3(transform.position.x + coinSpawnOffset, transform.position.y, transform.position.z), Quaternion.Euler(0,0,0));
                    break;
                case 2:
                    Instantiate(coin, new Vector3(transform.position.x, transform.position.y, transform.position.z + coinSpawnOffset), Quaternion.Euler(0, 0, 0));
                    break;
                case 3:
                    Instantiate(coin, new Vector3(transform.position.x - coinSpawnOffset, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
                    break;
                case 4:
                    Instantiate(coin, new Vector3(transform.position.x ,transform.position.y, transform.position.z - coinSpawnOffset), Quaternion.Euler(0, 0, 0));
                    break;
                default:
                    break;
            }


        }

        brokenPieces.Break();
    }
}

