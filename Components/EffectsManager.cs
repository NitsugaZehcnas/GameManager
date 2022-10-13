using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Manage game effects
/// </summary>
public class EffectsManager : MonoBehaviour
{
    #region Variables    

    //Public
    public CinemachineVirtualCamera VirtualCamera { get => virtualCamera; }
    private CinemachineVirtualCamera virtualCamera;

    //Private
    private float cameraShakeTimer;
    private float cameraShakeStartTimer;
    private float cameraShakeStartIntensity;
    private float slowTimeTimer;

    private GameManager gameManager;

    #endregion

    #region Base Methods
    
    /// <summary>
    /// Initialize Effect Manager
    /// </summary>
    /// <param name="_gameManager">Game Manager</param>
    public void Initialize(GameManager _gameManager)
    {
        //Get components
        gameManager = _gameManager;
    }

    /// <summary>
    /// Update the Effect manager
    /// </summary>
    /// <param name="_dt">Delta Time</param>
    public void DoUpdate(float _dt)
    {
        CheckCameraShakeTime();
        CheckSlowTime();
    }

    #endregion

    #region Camera Shake Methods

    /// <summary>
    /// Shake Camera
    /// </summary>
    /// <param name="_intensity">Intensity</param>
    /// <param name="_time">Duration</param>
    public void ShakeCamera(float _intensity, float _time)
    {
        if (VirtualCamera != null)
        {
            CinemachineBasicMultiChannelPerlin _perlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _perlin.m_AmplitudeGain = _intensity;
            cameraShakeStartIntensity = _intensity;
            cameraShakeStartTimer = _time;
            cameraShakeTimer = _time;
        }
    }

    /// <summary>
    /// Check if effect is done
    /// </summary>
    private void CheckCameraShakeTime()
    {
        if (cameraShakeTimer > 0 && VirtualCamera != null)
        {
            cameraShakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin _perlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _perlin.m_AmplitudeGain = Mathf.Lerp(cameraShakeStartIntensity, 0, 1 - (cameraShakeTimer / cameraShakeStartTimer));
        }
    }

    #endregion

    #region Slow Time Methods

    /// <summary>
    /// Slow the game
    /// </summary>
    /// <param name="_timeScale">Time Scale</param>
    /// <param name="_duration">Duration</param>
    public void SlowTime(float _timeScale, float _duration)
    {
        slowTimeTimer = _duration;
        Time.timeScale = _timeScale;
    }

    /// <summary>
    /// Check if effect is done
    /// </summary>
    void CheckSlowTime()
    {
        if (!gameManager.StateMachine.IsChangingState && gameManager.StateMachine.CurrentState == gameManager.StateMachine.InGameState)
        {
            if ((slowTimeTimer) > 0) slowTimeTimer -= Time.unscaledDeltaTime;

            else if (Time.timeScale != 1) Time.timeScale = 1;
        }
    }

    #endregion

    #region Set Methods

    /// <summary>
    /// Add reference of virtual camera to effect manager
    /// </summary>
    /// <param name="_cam"></param>
    public void SetVirtualCamera(CinemachineVirtualCamera _cam) 
    {
        virtualCamera = _cam;
    } 

    #endregion
}
