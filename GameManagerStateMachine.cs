using UnityEngine;

/// <summary>
/// GameManager state machine
/// </summary>
public class GameManagerStateMachine
{
    #region Variables
    
    public GameManagerState CurrentState { get; private set; }
    public bool IsChangingState{ get => isChangingState; }

    private GameManagerState newGameState;
    private bool isChangingState;

    #endregion

    #region States

    public GameMainMenuState MainMenuState { get; private set; }
    public GameInGameState InGameState { get; private set; }
    public GameLoadState LoadState { get; private set; }
    public GamePauseState PauseState { get; private set; }
    public GameInventoryState InventoryState { get; private set; }

    #endregion

    /// <summary>
    /// Initialize the GameStateMachine with all the states
    /// </summary>
    /// <param name="_manager">GameManeger reference</param>
    public void Initialize(GameManager _manager)
    {
        //Init States
        MainMenuState = new GameMainMenuState(_manager, this, "MainMenu");
        InGameState = new GameInGameState(_manager, this, "InGameMenu");
        LoadState = new GameLoadState(_manager, this, "LoadMenu");
        PauseState = new GamePauseState(_manager, this, "PauseMenu");

        //Init start state
        CurrentState = MainMenuState;
        CurrentState.Enter();
    }

    /// <summary>
    /// StateMachine Logic Update
    /// </summary>
    /// <param name="_dt">Delta Time</param>
    public void LogicUpdate(float _dt)
    {
        //Check if change state
        if (isChangingState && CurrentState.IsAnimCloseFinish)
        {
            //Reset change state
            isChangingState = false;

            //CurrentState.Exit();
            CurrentState = newGameState;
            CurrentState.Enter();
        }
    }

    /// <summary>
    /// Change the currente state of the game
    /// </summary>
    /// <param name="_newState">State to change</param>
    public void ChangeState(GameManagerState _newState)
    {
        newGameState = _newState;
        isChangingState = true;

        CurrentState.Exit();
    }
}
