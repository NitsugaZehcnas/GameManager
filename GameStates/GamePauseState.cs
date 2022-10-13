using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : GameManagerState
{
    public GamePauseState(GameManager gameManager, GameManagerStateMachine stateMachine, string musicName) : base(gameManager, stateMachine, musicName) { }
    
    public override void Enter()
    {
        base.Enter();

        gameManager.InputManager.ChangeInputMap("UI");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        gameManager.InputManager.ChangeInputMap("");        
    }
}
