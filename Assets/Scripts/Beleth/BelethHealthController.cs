using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethHealthController : MonoBehaviour
{

    [Header("Health")]
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    [Tooltip("Tiempo de invulnerabilidad despues de que te peguen")]
    private float inmortalTime;

    private bool canBeDamaged;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Damaged(int _damageDeal)
    {
        if (canBeDamaged)
        {
            healthPoints -= _damageDeal;
            // Hacer animacion
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
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    private void Die() { 
        //Hacer la animacio
        //Mostrar menu de reiniciar nivel
        //
    }

}
