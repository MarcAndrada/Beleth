using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public int collectableID;
    [SerializeField]
    private GameObject breakObject;
    [SerializeField]
    private GameObject firstVfx;
    [SerializeField]
    private GameObject secondVfx;
    [SerializeField]
    private AudioSource audioSource;

    private SphereCollider coll;
    private bool isDone;

    private float t;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        firstVfx.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isDone = false;
        t = 0;
    }

    private void Update()
    {
        if (isDone)
        {
            t += Time.deltaTime;
        }

        if (t > 4)
        {
            Instantiate(secondVfx, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void DisableCollectable() 
    {
        SoundManager._SOUND_MANAGER.FavorGet(audioSource);
        firstVfx.SetActive(true);
        coll.enabled = false;
        breakObject.SetActive(false);
        isDone = true;
    }
}
