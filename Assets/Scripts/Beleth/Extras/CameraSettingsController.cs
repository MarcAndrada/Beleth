using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraSettingsController : MonoBehaviour
{
    private CinemachineFreeLook cinemachineCamera;

    private float xMaxSpeed = 0;

    [SerializeField]
    private float xMinStrongSpeed;
    private float normalAccelTime = 0;
    private float timeWaitedResetAccel = 0;
    [SerializeField]
    private float timeToWaitResetAccel = 0;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineFreeLook>();

        if (SettingsController._SETTINGS_CONTROLLER != null)
        {
            cinemachineCamera.m_XAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedX * 3f;
            xMaxSpeed = cinemachineCamera.m_XAxis.m_MaxSpeed;
            cinemachineCamera.m_YAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedY / 50;
        }

        normalAccelTime = cinemachineCamera.m_XAxis.m_AccelTime;
    }

    private void Update()
    {

        Debug.Log(cinemachineCamera.m_XAxis.m_InputAxisValue);
        SetCameraAccel();
        WaitToResetMouse();
    }

    public void SetSpeedOnCamera()
    {
        if (SettingsController._SETTINGS_CONTROLLER != null)
        {
            cinemachineCamera.m_XAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedX * 3f;
            xMaxSpeed = cinemachineCamera.m_XAxis.m_MaxSpeed;
            cinemachineCamera.m_YAxis.m_MaxSpeed = SettingsController._SETTINGS_CONTROLLER.mouseSpeedY / 50;
        }
    }

    private void SetCameraAccel() 
    {
        if (cinemachineCamera.m_XAxis.m_InputAxisValue > xMinStrongSpeed || cinemachineCamera.m_XAxis.m_InputAxisValue < -xMinStrongSpeed)
        {
            timeWaitedResetAccel = 0;
            cinemachineCamera.m_XAxis.m_AccelTime = 0.1f;
            cinemachineCamera.m_XAxis.m_MaxSpeed = xMaxSpeed * 1.5f;
        }
    }

    private void WaitToResetMouse() 
    {
        if (cinemachineCamera.m_XAxis.m_AccelTime < normalAccelTime)
        {
            timeWaitedResetAccel += Time.deltaTime;
            if (timeWaitedResetAccel >= timeToWaitResetAccel)
            {
                cinemachineCamera.m_XAxis.m_AccelTime = normalAccelTime;
                timeWaitedResetAccel = 0;
                cinemachineCamera.m_XAxis.m_MaxSpeed = xMaxSpeed;
            }
        }
        else
        {
            timeWaitedResetAccel = 0;
        }
    }
}
