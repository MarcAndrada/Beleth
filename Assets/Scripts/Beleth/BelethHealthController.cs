using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethHealthController : MonoBehaviour
{

    [Header("Health")]
    [SerializeField]
    private int maxHealthPoints;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    [Tooltip("Tiempo de invulnerabilidad despues de que te peguen")]
    private float inmortalTime;

    private bool canBeDamaged = true;
    private bool isAlive = true;
    private BelethUIController uiController;
    private BelethAnimController animController;
    private BelethCheckPointManager checkPointManager;
    private BelethMovementController movementController;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<BelethUIController>();
        animController = GetComponent<BelethAnimController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        movementController = GetComponent<BelethMovementController>();

        healthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(int _damageDeal, bool _doHurtAnim)
    {
        if (canBeDamaged)
        {
            healthPoints -= _damageDeal;
            Debug.Log("Te isieron " + _damageDeal + " de pupa te quedan " + healthPoints + " de vida");
            // Hacer animacion
            animController.SetHealthValue(healthPoints);
            if (_doHurtAnim || healthPoints <= 0)
            {
                animController.DamageTrigger();
            }
            movementController.SetCanMove(false);
            movementController.SetCurrentSpeed(0);
            canBeDamaged = false;
            StartCoroutine(WaitForInmortalFrames());
            CheckHP();
        }
    }

    IEnumerator WaitForInmortalFrames() 
    {
        yield return new WaitForSeconds(inmortalTime);
        canBeDamaged = true;
    }

    private void CheckHP()
    {
        uiController.SetHealthUI(healthPoints);

        if (healthPoints <= 0)
        {
            if (isAlive)
            {
                Die();
            }
        }
        else
        {
            StartCoroutine(WaitToMoveAgain());
        }
    }

    private void Die() {

        movementController.SetCanMove(false);
        isAlive = false;
        //Hacer la animacion de muerte
        //Mostrar menu de reiniciar nivel

        //Desactivar el movimiento
        Debug.Log("Has muerto");
        StartCoroutine(Respawn());
    
    }

    private IEnumerator Respawn() {

        yield return new WaitForSeconds(3);
        movementController.SetCanMove(true);
        checkPointManager.GoLastCheckPoint();
        healthPoints = maxHealthPoints;
        animController.SetHealthValue(healthPoints);
        isAlive = true;
    }

    public int GetHealthPoints() { 
        return healthPoints;
    }

    private IEnumerator WaitToMoveAgain() { 

        yield return new WaitForSeconds (0.5f);
        
        movementController.SetCanMove(true);
    }
}
