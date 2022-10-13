using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic of the InGame State
/// </summary>
public class GameInGameState : GameManagerState
{
    public GameInGameState(GameManager gameManager, GameManagerStateMachine stateMachine, string musicName) : base(gameManager, stateMachine, musicName) { }
    
    public override void Enter()
    {
        base.Enter();

        //Rescale time
        if (Time.timeScale != 1) Time.timeScale = 1;

        //Change input map
        gameManager.InputManager.ChangeInputMap("Gameplay");
    }

    public override void Exit()
    {
        base.Exit();

        //Inactive input map
        gameManager.InputManager.ChangeInputMap("");
    }
}
