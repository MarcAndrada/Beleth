using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BelethUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject deathCanvas;

    public void SetHealthUI(int _toatLive)
    {
        switch (_toatLive)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            default:
                break;
        }
    }

    public void ShowDeathUI(bool _canShow) {

        deathCanvas.SetActive(_canShow);
    
    }

}
