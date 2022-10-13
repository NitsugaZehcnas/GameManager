using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Manage all the GameData
/// </summary>
public class DataManager : MonoBehaviour
{
    #region Variables
    
    //Slots
    public GameData[] Slots { get => slots; }
    public int SlotIndex { get => slotIndex; }

    private GameData[] slots;
    private int slotIndex;

    //Current Game Data
    public GameData CurrentGameData { get => currentGameData; }
    [SerializeField] private GameData currentGameData;

    //Private
    private GameManager gameManager;
    private string path;

    #endregion

    #region Initialization

    /// <summary>
    /// Initialize the Data manager
    /// </summary>
    /// <param name="_gameManger">Game Manager</param>
    public void Initialize(GameManager _gameManger)
    {
        //Init references
        gameManager = _gameManger;

        path = Application.persistentDataPath + "/Saves";
    }

    #endregion

    #region Load Methods
    
    /// <summary>
    /// Load game
    /// </summary>
    /// <param name="_slotIndex">Index of slot to load</param>
    public void LoadData(int _slotIndex)
    {
        //Save current slot index
        slotIndex = _slotIndex;

        //Save the file name
        string _fileName = string.Format("/{0}.dat", "PlayerData_" + _slotIndex);

        //Check if exists a game
        if (!CheckGameData(_slotIndex))
        {
            NewGame();
        }

        else
        {
            //Open the file
            BinaryFormatter _bf = new BinaryFormatter();
            FileStream _file = File.Open(path + _fileName, FileMode.Open);

            //Set to CurrentGameData
            currentGameData = (GameData)_bf.Deserialize(_file);
            _file.Close();

            //Continue game
            ContinueGame();
        }
    }

    /// <summary>
    /// Save the current data in his slot
    /// </summary>
    public void SaveData()
    {
        //Saves the file name
        string _fileName = string.Format("/{0}.dat", "PlayerData_" + slotIndex);

        //Create the directory if doesn't exists
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        //Create the file
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(path + _fileName);

        //Set to CurrentGameData
        _bf.Serialize(_file, currentGameData);
        _file.Close();
    }

    /// <summary>
    /// Delete the gameData
    /// </summary>
    /// <param name="_slot">Slot to delete</param>
    public void DeleteGame(int _slot)
    {
        //Save the file name
        string _fileName = string.Format("/{0}.dat", "PlayerData_" + _slot);

        //Delete game data
        if (CheckGameData(_slot)) File.Delete(path + _fileName);
    }

    #endregion

    #region Data Methods

    /// <summary>
    /// Starts new game
    /// </summary>
    private void NewGame()
    {
        //Init values
        currentGameData = new GameData();

        //Init game
        gameManager.LoadSceneManager.LoadScene(gameManager.DataManager.CurrentGameData.spawnScene);
    }

    /// <summary>
    /// Continue new game
    /// </summary>
    private void ContinueGame()
    {
        //Init game
        gameManager.LoadSceneManager.LoadScene(gameManager.DataManager.CurrentGameData.spawnScene);
    }

    /// <summary>
    /// Set slot index
    /// </summary>
    /// <param name="_slot">New index</param>
    public void SetSlotIndex(int _slot)
    {
        slotIndex = _slot;
    }

    /// <summary>
    /// Check if exists data in the slot
    /// </summary>
    /// <param name="_slotIndex">SLot index</param>
    /// <returns>Exists data in the slot</returns>
    public bool CheckGameData(int _slotIndex)
    {
        string _fileName = string.Format("/{0}.dat", "PlayerData_" + _slotIndex);
        return Directory.Exists(path) && File.Exists(path + _fileName);
    }

    #endregion
}
