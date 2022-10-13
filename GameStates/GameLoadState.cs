using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Logic of Load State
/// </summary>
public class GameLoadState : GameManagerState
{
    #region Variables

    private AsyncOperation loadSceneOperation;
    private LoadState currentState;

    #endregion

    public GameLoadState(GameManager gameManager, GameManagerStateMachine stateMachine, string menuName) : base(gameManager, stateMachine, menuName) { }

    public override void Enter()
    {
        base.Enter();

        //Inactive input
        gameManager.InputManager.ChangeInputMap("");

        //Save references
        loadSceneOperation = gameManager.LoadSceneManager.LoadSceneOperation;

        //Init references
        currentState = LoadState.ChargingScene;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Update the substate logicUpdate
        switch (currentState)
        {
            case LoadState.ChargingScene:
            {
                ChargingScene_LogicUpdate();
                break;
            }
            case LoadState.InitializeScene:
            {
                InitializeScene_LogicUpdate();
                break;
            }
            case LoadState.CloseState:
            {
                CloseState_LogicUpdate();
                break;
            }
        }
    }

    #region Substates Methods

    /// <summary>
    /// Change the current substate
    /// </summary>
    /// <param name="_newState">New substate</param>
    private void ChangeSubState(LoadState _newState)
    {
        currentState = _newState;
    }
    
    /// <summary>
    /// Logic update of ChargingScene state
    /// </summary>
    private void ChargingScene_LogicUpdate()
    {
        if (isAnimOpenFinish && loadSceneOperation.progress == 0.9f)
        {
            //Activate scene
            loadSceneOperation.allowSceneActivation = true;

            //Change substate
            ChangeSubState(LoadState.InitializeScene);
        }
    }

    /// <summary>
    /// Logic update of InitializeScene state
    /// </summary>
    private void InitializeScene_LogicUpdate()
    {   
        if (SceneManager.GetActiveScene().buildIndex == 0 || gameManager.LoadSceneManager.SceneInitialize)
        {
            //Close menu
            gameManager.UIManager.CurrentMenu.CloseMenu();

            //Change state
            ChangeSubState(LoadState.CloseState);
        }

        else
        {
            //Initialize scene
            gameManager.LoadSceneManager.InitializeGameScene();
        }
    }

    /// <summary>
    /// Logic update of CloseState state
    /// </summary>
    private void CloseState_LogicUpdate()
    {
        if (isAnimCloseFinish)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) //MAIN MENU
            {
                gameManager.StateMachine.ChangeState(gameManager.StateMachine.MainMenuState);
            }

            else //IN GAME
            {
                gameManager.StateMachine.ChangeState(gameManager.StateMachine.InGameState);                
            }
        }
    }

    #endregion

    /// <summary>
    /// LoadScene substates
    /// </summary>
    public enum LoadState
    {
        ChargingScene,
        InitializeScene,
        CloseState
    }
}
