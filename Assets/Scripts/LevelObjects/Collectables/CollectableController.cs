using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField]
    private GameObject breakObject;
    [SerializeField]
    private GameObject firstVfx;
    [SerializeField]
    private GameObject secondVfx;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private bool rootsParticles = true;

    private SphereCollider coll;
    private bool isDone;

    private float timeWaited;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        firstVfx.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isDone = false;
        timeWaited = 0;
    }

    private void Update()
    {
        if (isDone)
        {
            timeWaited += Time.deltaTime;
        }

        if (timeWaited > 4)
        {
            if (rootsParticles)
            {
                Instantiate(secondVfx, transform.position, transform.rotation);

            }
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
