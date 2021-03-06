using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingrockManager : MonoBehaviour
{
   
    [SerializeField]
    Transform minRange;

    [SerializeField]
    Transform maxRange;

    [SerializeField]
    Vector2 height;

    [SerializeField]
    float timeToSpawn;

    [SerializeField]
    float maxRocks;

    public static float actualRocks;

    [SerializeField]
    GameObject fallingRock;

    void Start()
    {
        actualRocks = 0;
    }

    void Update()
    {
        if (actualRocks < maxRocks)
        {
            StartCoroutine(rockTimer());
        }
    }

    IEnumerator rockTimer()
    {
        actualRocks++;
        
        yield return new WaitForSeconds(timeToSpawn);
        Vector3 pos = posToSpawn();
        Instantiate(fallingRock, pos, Quaternion.identity);

    }

    Vector3 posToSpawn()
    {
        Vector3 _pos;
        _pos = new Vector3(Random.Range(minRange.position.x, maxRange.position.x), Random.Range(height.x,height.y), Random.Range(minRange.position.z, maxRange.position.z));
        return _pos;
    }
}
