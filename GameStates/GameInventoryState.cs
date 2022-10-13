using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic of the inventory state
/// </summary>
public class GameInventoryState : GameManagerState
{        
    public List<GameObject> MapRoom { get => mapRoom; }
    private List<GameObject> mapRoom = new List<GameObject>();

    public GameInventoryState(GameManager gameManager, GameManagerStateMachine stateMachine, string menuName) : base(gameManager, stateMachine, menuName) { }

    public override void Enter()
    {
        base.Enter();

        //if (MapRoom == null) gameManager.UIManager.CurrentMenu.MenuAction("InitMapRooms");
        /*
        foreach (GameObject room in MapRoom)
        {
            if (room.activeInHierarchy && gameManager.DataManager.CurrentGameData.sceneData.ContainsKey(room.name)) room.SetActive(false);
        }*/
    }
}
