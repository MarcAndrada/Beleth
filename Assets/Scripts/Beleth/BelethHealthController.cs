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

    private BelethUIController uiController;
    private BelethAnimController animController;
    private BelethCheckPointManager checkPointManager;
    private BelethMovementController charController;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<BelethUIController>();
        animController = GetComponent<BelethAnimController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        charController = GetComponent<BelethMovementController>();

        healthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(int _damageDeal)
    {
        if (canBeDamaged)
        {
            healthPoints -= _damageDeal;
            Debug.Log("Te isieron " + _damageDeal + " de pupa te quedan " + healthPoints + " de vida");
            // Hacer animacion
            animController.DamageTrigger();
            canBeDamaged = false;
            StartCoroutine(WaitForInmortalFrames());

            CheckHP();
        }
    }

    IEnumerator WaitForInmortalFrames() {


        yield return new WaitForSeconds(inmortalTime);
        canBeDamaged = true;
    }

    private void CheckHP()
    {
        uiController.SetHealthUI(healthPoints);

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    private void Die() {
        //Hacer la animacion de muerte
        //Mostrar menu de reiniciar nivel

        //Desactivar el movimiento
        charController.SetCanMove(false);
        StartCoroutine(Respawn());
        Debug.Log("Has muerto");
    }

    IEnumerator Respawn() {

        
        yield return new WaitForSeconds(2);
        charController.SetCanMove(true);
        checkPointManager.GoLastCheckPoint();
        healthPoints = maxHealthPoints;
    }

    public int GetHealthPoints() { 
        return healthPoints;
    }

}
