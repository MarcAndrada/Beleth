using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethHealthController : MonoBehaviour
{

    [Header("Health")]
    [SerializeField]
    private int maxHealthPoints;
    [SerializeField]
    public int healthPoints;
    
    [SerializeField]
    [Tooltip("Tiempo de invulnerabilidad despues de que te peguen")]
    private float timeToWaitInmortal;
    [SerializeField]
    [Tooltip("Tiempo en el que tarda en reaparecer")]
    private float timeToWaitDead;
    [SerializeField]
    [Tooltip("Tiempo en el que tarda en devolver el movimiento tras ser golpeado")]
    private float timeToWaitDamaged;

    private bool canBeDamaged = true;
    private bool isAlive = true;
    private bool isDamaged = false;
    private float timeWaitedImortal = 0;
    private float timeWaitedDead = 0;
    private float timeWaitedDamaged = 0;
    private BelethUIController uiController;
    private BelethAnimController animController;
    private BelethCheckPointManager checkPointManager;
    private BelethMovementController movementController;
    private WrathBossActivator activator;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<BelethUIController>();
        animController = GetComponent<BelethAnimController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        movementController = GetComponent<BelethMovementController>();
        activator = FindObjectOfType<WrathBossActivator>();
        healthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GetDamage(1);
        }

        if (!canBeDamaged && isAlive)
        {
            timeWaitedImortal += Time.deltaTime;
            if (timeWaitedImortal >= timeToWaitInmortal)
            {
                canBeDamaged = true;
                timeWaitedImortal = 0;
            }
        }

        if (isDamaged && isAlive)
        {
            timeWaitedDamaged += Time.deltaTime;
            if (timeWaitedDamaged >= timeToWaitDamaged)
            {
                movementController.SetCanMove(true);
                isDamaged = false;
                timeWaitedDamaged = 0;

            }

        }
        
        if (!isAlive)
        {
            timeWaitedDead += Time.deltaTime;

            if (timeWaitedDead >= timeToWaitDead)
            {
                movementController.SetCanMove(true);
                checkPointManager.GoLastCheckPoint();
                isAlive = true;
                SoundManager._SOUND_MANAGER.ReviveSound();
                healthPoints = maxHealthPoints;
                animController.SetHealthValue(healthPoints);
                timeWaitedDead = 0;
            }


        }

    }

    public void GetDamage(int _damageDeal, bool _doHurtAnim = true)
    {
        if (canBeDamaged)
        {
            healthPoints -= _damageDeal;
            //Debug.Log("Te isieron " + _damageDeal + " de pupa te quedan " + healthPoints + " de vida");
            // Hacer animacion
            animController.SetHealthValue(healthPoints);
            if (_doHurtAnim || healthPoints <= 0)
            {
                animController.DamageTrigger();
            }

            canBeDamaged = false;
            CheckHP();

        }
    }

    private void CheckHP()
    {
        uiController.SetHealthUI(healthPoints);

        if (healthPoints <= 0)
        {
            if (isAlive)
            {
                Die();
                SoundManager._SOUND_MANAGER.BelethDeath();

            }
        }
        else
        {
            SoundManager._SOUND_MANAGER.BelethDamaged();
            isDamaged = true;
            movementController.SetCanMove(false);
        }
    }

    private void Die() {
       
        //Desactivar el movimiento
        movementController.SetCanMove(false);
        isAlive = false;

        //Hacer la animacion de muerte
        
        //Debug.Log("Has muerto");
        activator.PlayerExit();


    }


    public int GetHealthPoints() { 
        return healthPoints;
    }

}
