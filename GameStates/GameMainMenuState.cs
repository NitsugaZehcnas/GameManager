using UnityEngine;

/// <summary>
/// Logic of the MainMenu State
/// </summary>
public class GameMainMenuState : GameManagerState
{
    public GameMainMenuState(GameManager gameManager, GameManagerStateMachine stateMachine, string menuName) : base(gameManager, stateMachine, menuName) { }
    
    public override void Enter()
    {
        base.Enter();

        //Rescale time
        if (Time.timeScale != 1) Time.timeScale = 1;

        //Active UI input
        gameManager.InputManager.ChangeInputMap("UI");
    }

    public override void Exit()
    {
        base.Exit();

        //Inactive Input
        gameManager.InputManager.ChangeInputMap("");
    }
}
