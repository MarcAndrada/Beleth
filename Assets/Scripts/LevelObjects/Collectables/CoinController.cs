using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField]
    private int coinQuantity = 0;


    private BelethUIController uiController;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<BelethUIController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin() 
    {
        coinQuantity++;
        uiController.UpdateCoinUI(coinQuantity);
    }

}
