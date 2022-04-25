using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraSpeedController : MonoBehaviour
{
    private CinemachineFreeLook cinemachineCamera;


    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineFreeLook>();
        
        if (SettingsController._SETTINGS_CONTROLLER != null)
        {
            cinemachineCamera.m_XAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedX;
            cinemachineCamera.m_YAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedY;
        }
    }

    public void SetSpeedOnCamera() 
    {
        if (SettingsController._SETTINGS_CONTROLLER != null)
        {
            cinemachineCamera.m_XAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedX;
            cinemachineCamera.m_YAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedY;
        }
    }
}
