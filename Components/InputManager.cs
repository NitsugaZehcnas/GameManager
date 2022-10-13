using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic;

/// <summary>
/// Manage all the input
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Variables

    //Input
    public Player_Input PlayerInput { get => playerInput; }
    public DeviceType TypeOfDevice { get => typeOfDevice; }
    public InputDevice CurrentDevice { get => currentDevice; }

    private Player_Input playerInput;
    private DeviceType typeOfDevice;
    private InputDevice currentDevice;

    //Private
    private GameManager gameManager;
    [SerializeField] private float inputHoldTime = 0.2f;

    //Timers
    private float jumpStartTime;
    private float attackStartTime;
    private float throwTpStartTime;
    private float dodgeStartTime;

    //Iteract
    private bool canInteract;
    private IInteract interact;

    #endregion

    #region Input Triggers

    //Public
    public Vector2 RawMoveVector { get => rawMoveVector; }
    public int NormInputX { get => normInputX; }
    public int NormInputY { get => normInputY; }
    public bool JumpInput { get => jumpInput; }
    public bool JumpInputStop { get => jumpInputStop; }
    public bool AttackInput { get => attackInput; }
    public bool ThrowTpInput { get => throwTpInput; }
    public bool DodgeInput { get => dodgeInput; }
    public bool HealthInput { get => healthInput; }

    //Private
    private Vector2 rawMoveVector;
    private int normInputX;
    private int normInputY;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool attackInput;
    private bool throwTpInput;
    private bool dodgeInput;
    private bool healthInput;

    #endregion

    #region Base Methods

    /// <summary>
    /// Initialize the Input Manager
    /// </summary>
    /// <param name="_gameManager"></param>
    public void Initialize(GameManager _gameManager)
    {
        //Init varaibles
        gameManager = _gameManager;

        //Init input triggers
        playerInput = new Player_Input();

        //Move
        playerInput.Gameplay.Move.performed += ctx => OnMoveInput(ctx.ReadValue<Vector2>());
        playerInput.Gameplay.Move.canceled += ctx => OnMoveInput(ctx.ReadValue<Vector2>());

        //Jump
        playerInput.Gameplay.Jump.started += ctx => OnJumpInput(ctx);
        playerInput.Gameplay.Jump.canceled += ctx => OnJumpInput(ctx);

        //Attack
        playerInput.Gameplay.Attack.started += ctx => OnAttackInput();

        //Throw / Tp
        playerInput.Gameplay.ThrowTp.started += ctx => OnThrowTpInput();

        //Dodge
        playerInput.Gameplay.Dodge.started += ctx => OnDodgeInput();

        //Pause
        playerInput.Gameplay.Pause.started += ctx => OnPauseInput();

    }

    public void DoUpdate(float _dt)
    {
        //Check input hold time
        if (jumpInput && Time.time >= jumpStartTime + inputHoldTime) UseJumpInput();
        if (attackInput && Time.time >= attackStartTime + inputHoldTime) UseAttackInput();
        if (throwTpInput && Time.time >= throwTpStartTime + inputHoldTime) UseThrowTpInput();
        if (dodgeInput && Time.time >= dodgeStartTime + inputHoldTime) UseDodgeInput();

        //Check interact
        CheckInteract();
    }

    #endregion

    #region Device Methods    

    /// <summary>
    /// Check the type of inputDevice
    /// </summary>
    /// <param name="_ctx">Event input context</param>
    void CheckTypeOfDevice(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            //Save device
            InputDevice _actualDevice = _ctx.control?.device;

            //Check mouse
            if (_actualDevice.name == "Mouse" && TypeOfDevice == DeviceType.Keyboard) return;

            //Check if is different and change the device
            if (CurrentDevice != _actualDevice)
            {
                currentDevice = _actualDevice;
                ChangeDevice(_actualDevice.name);
            }
        }
    }

    /// <summary>
    /// Change type of device
    /// </summary>
    /// <param name="_deviceName">Name of the device</param>
    private void ChangeDevice(string _deviceName)
    {
        //Change device
        switch (_deviceName)
        {
            case "Mouse":
                {
                    typeOfDevice = DeviceType.Keyboard;
                    break;
                }
            case "Keyboard":
                {
                    typeOfDevice = DeviceType.Keyboard;
                    break;
                }
            case "DualShock4GamepadHID":
                {
                    typeOfDevice = DeviceType.PlayStation;
                    break;
                }
            default:
                {
                    typeOfDevice = DeviceType.Xbox;
                    break;
                }
        }

        //Send change inpu event
        if(Input_Events.changeInputEvent != null) Input_Events.changeInputEvent();
    }

    #endregion
    
    #region InputMap Methods    

    /// <summary>
    /// Change the current inputMap
    /// </summary>
    /// <param name="_actionMap">UI | Gameplay | Leave empty to deselect all</param>
    public void ChangeInputMap(string _actionMap = "")
    {
        switch (_actionMap)
        {
            case "UI":
            {
                PlayerInput.Gameplay.Get().actionTriggered -= ctx => CheckTypeOfDevice(ctx);
                PlayerInput.UI.Get().actionTriggered += ctx => CheckTypeOfDevice(ctx);

                PlayerInput.Gameplay.Disable();
                PlayerInput.UI.Enable();
                break;
            }
            case "Gameplay":
            {
                PlayerInput.UI.Get().actionTriggered -= ctx => CheckTypeOfDevice(ctx);
                PlayerInput.Gameplay.Get().actionTriggered += ctx => CheckTypeOfDevice(ctx);

                PlayerInput.UI.Disable();
                PlayerInput.Gameplay.Enable();
                break;
            }
            default:
            {
                PlayerInput.Gameplay.Get().actionTriggered -= ctx => CheckTypeOfDevice(ctx);
                PlayerInput.UI.Get().actionTriggered -= ctx => CheckTypeOfDevice(ctx);

                PlayerInput.Gameplay.Disable();
                PlayerInput.UI.Disable();
                break;
            }
        }
    }

    #endregion

    #region Input Events Methods
    
    /// <summary>
    /// Register the move input vector
    /// </summary>
    private void OnMoveInput(Vector2 vector)
    {
        rawMoveVector = vector;

        normInputX = Mathf.RoundToInt(rawMoveVector.x);
        normInputY = Mathf.RoundToInt(rawMoveVector.y);
    }

    /// <summary>
    /// Register the jump input
    /// </summary>
    private void OnJumpInput(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            jumpStartTime = Time.time;
        }

        if(_ctx.canceled)
        {
            jumpInputStop = true;
        }
    }

    /// <summary>
    /// Register the attack input
    /// </summary>
    private void OnAttackInput()
    {
        attackInput = true;
        attackStartTime = Time.time;
    }

    /// <summary>
    /// Register the throw tp input
    /// </summary>
    private void OnThrowTpInput()
    {
        throwTpInput = true;
        throwTpStartTime = Time.time;
    }

    /// <summary>
    /// Register the dodge input
    /// </summary>
    private void OnDodgeInput()
    {
        dodgeInput = true;
        dodgeStartTime = Time.time;
    }

    /// <summary>
    /// Pause game
    /// </summary>
    private void OnPauseInput()
    {
        Time.timeScale = 0;
        gameManager.StateMachine.ChangeState(gameManager.StateMachine.PauseState);
    }

    #endregion

    #region Use Input Methods
    
    /// <summary>
    /// Reset Jump Input
    /// </summary>
    public void UseJumpInput() => jumpInput = false;

    /// <summary>
    /// Reset Attack Input
    /// </summary>
    public void UseAttackInput() => attackInput = false;

    /// <summary>
    /// Reset Throw/Tp Input
    /// </summary>
    public void UseThrowTpInput() => throwTpInput = false;

    /// <summary>
    /// Reset Dodge Input
    /// </summary>
    public void UseDodgeInput() => dodgeInput = false;

    #endregion

    #region Interact Methods
    
    /// <summary>
    /// Set interact
    /// </summary>
    /// <param name="_value">Interact type</param>
    public void SetInteract(IInteract _value) 
    {
        interact = _value;
    } 

    /// <summary>
    /// Check if can interact
    /// </summary>
    private void CheckInteract()
    {
        //TODO: Change the interact system

        if (canInteract && interact != null && NormInputY > 0)
        {
            canInteract = false;
            interact.Interact();
        }

        else if ((!canInteract && interact != null && NormInputY == 0) || (!canInteract && interact == null))
        {
            canInteract = true;
        }
    }

    #endregion
}

/// <summary>
/// Type of device
/// </summary>
public enum DeviceType
{
    Keyboard,
    PlayStation,
    Nintendo,
    Xbox,

    Count
}