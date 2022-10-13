using UnityEngine;

/// <summary>
/// Base logic for all game states
/// </summary>
public class GameManagerState
{
    #region Variables
        
    //Protected
    protected GameManager gameManager;
    protected GameManagerStateMachine stateMachine;
    protected string menuName;

    protected float startTime;
    protected bool isAnimOpenFinish;
    protected bool isAnimCloseFinish;

    //Public
    public bool IsAnimOpenFinish { get => isAnimOpenFinish; }
    public bool IsAnimCloseFinish { get => isAnimCloseFinish; }

    //Private
    private bool isExit;

    #endregion

    public GameManagerState(GameManager gameManager, GameManagerStateMachine stateMachine, string menuName)
    {
        this.gameManager = gameManager;
        this.stateMachine = stateMachine;
        this.menuName = menuName;
    }

    #region Base Methods        
    
    public virtual void Enter()
    {
        //Start variables
        startTime = Time.time;
        isAnimOpenFinish = false;
        isAnimCloseFinish = false;
        isExit = false;

        //Open current menu
        gameManager.UIManager.OpenMenu(menuName);
    }

    public virtual void LogicUpdate()
    {
        if (isExit) return;
    }

    public virtual void Exit() 
    {
        //Set exit
        isExit = true;

        //Close Menu
        gameManager.UIManager.CloseMenu(menuName);
    }

    public virtual void SetAnimationOpenFinish() 
    {        
        isAnimOpenFinish = true;
    } 
    
    public virtual void SetAnimationCloseFinish() 
    {
        isAnimCloseFinish = true;
    } 

    public virtual void AnimationTrigger() 
    { 

    }

    #endregion
}
