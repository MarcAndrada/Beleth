using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GuideStarterPosController : MonoBehaviour
{

    [SerializeField]
    private GameObject ghost;
    [SerializeField]
    private Text followMeText;
    [SerializeField]
    private float fadeInSpeed;
    [SerializeField]
    private float fadeOutSpeed;

    private AudioSource audioSource;

    [HideInInspector]
    public GameObject player;
    private GuideAIController guideAIController;
    private bool showingText = false;
    private bool fadingIn;

    private void Awake()
    {
        guideAIController = ghost.GetComponent<GuideAIController>();
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (showingText)
        {
            if (fadingIn)
            {
                followMeText.color = new Color(followMeText.color.r, followMeText.color.g, followMeText.color.b, followMeText.color.a + (fadeInSpeed * Time.deltaTime));
                if (followMeText.color.a >= 1)
                {
                    fadingIn = false;
                }
            }
            else
            {
                followMeText.color = new Color(followMeText.color.r, followMeText.color.g, followMeText.color.b, followMeText.color.a - (fadeOutSpeed * Time.deltaTime));
                if (followMeText.color.a <= 0)
                {
                    showingText = false;
                }
            }
        }
    }

    private void StartGuidePlayer() 
    {
        SoundManager._SOUND_MANAGER.PhantomApears(audioSource);
        ghost.SetActive(true);
        if (!guideAIController.guidingPlayer)
        {
            showingText = true;
            fadingIn = true;
        }
        guideAIController.StartGuide();
        
    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player == null)
            {
                player = other.gameObject;
            }

            StartGuidePlayer();
        }
    }


}
