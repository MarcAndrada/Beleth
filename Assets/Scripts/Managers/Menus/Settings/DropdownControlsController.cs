using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownControlsController : MonoBehaviour
{

    [SerializeField]
    private ControlsMenuController controlsMenu;

    private Dropdown dropdown;

    private void Awake()
    {
        dropdown= GetComponent<Dropdown>();
    }

    // Start is called before the first frame update
    void Start()
    {


        dropdown.onValueChanged.AddListener(delegate { 
            controlsMenu.ChangeValue(dropdown.value); 
        });

        dropdown.value = SettingsController._SETTINGS_CONTROLLER.GetControlsType();

    }

    
}
