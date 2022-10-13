//Se encarga de la navegaci�n entre escenas
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the scene navigation
/// </summary>
public class LoadSceneManager : MonoBehaviour
{
    #region Variables

    //SceneData
    public SceneData CurrentSceneData { get => currentSceneData; }
    public SO_SceneInfo CurrentSceneInfo { get => currentSceneInfo; }

    private SceneData currentSceneData;
    private SO_SceneInfo currentSceneInfo;

    public string PrevSceneName{ get => prevSceneName; }
    private string prevSceneName;

    //Load operation
    public AsyncOperation LoadSceneOperation { get => sceneLoadOperation; }
    private AsyncOperation sceneLoadOperation;

    //Initialize
    public bool SceneInitialize { get => sceneInitialize; }
    private bool sceneInitialize;

    private GameManager gameManager;

    #endregion

    #region Base Methods

    /// <summary>
    /// Initialization of LoadSceneManager
    /// </summary>
    /// <param name="_gameManager">GameManager</param>
    public void Initialize(GameManager _gameManager)
    {
        //Get components
        gameManager = _gameManager;
    }

    /// <summary>
    /// Update method of LoadSceneManager
    /// </summary>
    /// <param name="_dt">Delta Time</param>
    public void DoUpdate(float _dt)
    {
        
    }

    #endregion

    #region Scene Manager Methods

    /// <summary>
    /// Load main scene
    /// </summary>
    public void LoadMainScene()
    {
        //Reset initialize
        sceneInitialize = false;

        //Init load
        sceneLoadOperation = SceneManager.LoadSceneAsync(0);
        sceneLoadOperation.allowSceneActivation = false;

        //Change state
        gameManager.StateMachine.ChangeState(gameManager.StateMachine.LoadState);
    }

    /// <summary>
    /// Load scene async by his name
    /// </summary>
    /// <param name="_sceneName">Scene Name</param>
    public void LoadScene(string _sceneName)
    {
        //Reset initialize
        sceneInitialize = false;

        //Init load
        sceneLoadOperation = SceneManager.LoadSceneAsync(_sceneName);
        sceneLoadOperation.allowSceneActivation = false;

        //Change state
        gameManager.StateMachine.ChangeState(gameManager.StateMachine.LoadState);
    }

    /// <summary>
    /// Load scene async by his index
    /// </summary>
    /// <param name="_sceneIndex">Scene Index</param>
    public void LoadScene(int _sceneIndex)
    {
        //Reset initialize
        sceneInitialize = false;

        //Init load
        sceneLoadOperation = SceneManager.LoadSceneAsync(_sceneIndex);
        sceneLoadOperation.allowSceneActivation = false;

        //Change state
        gameManager.StateMachine.ChangeState(gameManager.StateMachine.LoadState);
    }

    #endregion

    #region Game Scene Methods

    /// <summary>
    /// Init any game scene
    /// </summary>
    public void InitializeGameScene()
    {
        //Get sceneName
        string _sceneName = SceneManager.GetActiveScene().name;

        //Check if save PrevSceneName
        if (currentSceneData != null && currentSceneInfo != null) prevSceneName = currentSceneInfo.sceneName;

        //Asing the current scriptable object
        currentSceneInfo = Resources.Load<SO_SceneInfo>($"ScriptableObjects/SceneInfo/{_sceneName}");

        //Check if needs create a new sceneData
        if (!gameManager.DataManager.CurrentGameData.sceneData.ContainsKey(_sceneName)) //DOESN'T EXISTS
        {
            //Create a new sceneData
            currentSceneData = new SceneData();

            //Init arrays
            currentSceneData.enemyDead = new bool[currentSceneInfo.spawnEnemyDetails.Count];

            //Save sceneData
            gameManager.DataManager.CurrentGameData.sceneData.Add(_sceneName, currentSceneData);
            
            Debug.Log("NO EXISTE");
        }

        else //EXISTS
        {
            //Get save scene data
            currentSceneData = gameManager.DataManager.CurrentGameData.sceneData[_sceneName];

            Debug.Log("EXISTE");
        }

        //Spawn player
        SpawnPlayer();

        //Spawn enemies
        SpawnEnemies();

        //Set initialize
        sceneInitialize = true;

        Debug.Log("Scene Initialize");
    }

    /// <summary>
    /// Instantiate the player in the gamescene
    /// </summary>
    private void SpawnPlayer()
    {
        //Save list reference
        List<SpawnPlayerDetails> _spawnPositions = currentSceneInfo.spawnPlayerDetails;

        //Init values
        Vector2 _spawnPos = Vector2.zero;
        int _facinDirection = 1;

        //Check which spawn point init
        for (int i = 0; i < _spawnPositions.Count; i++)
        {
            if (_spawnPositions[i].preSceneName == prevSceneName)
            {   
                //Save the spawn position and start facing direction
                _spawnPos = _spawnPositions[i].spawnPos;
                _facinDirection = _spawnPositions[i].facingDirection;

                break;
            }
        }   
        
        //Spawn player and save refereneces
        GameObject _playerGo =  GameObject.Instantiate(gameManager.PrefabIndex.player, _spawnPos, Quaternion.identity);
        Player _playerScript = _playerGo.GetComponent<Player>();

        //Set reference of player in gamemanager
        gameManager.InitPlayer(_playerScript);

        //Init player rotation
        _playerScript.Core.Movement?.CheckInitFlip(_facinDirection);
    }

    /// <summary>
    /// Instantiate all the enemies in the scene
    /// </summary>
    private void SpawnEnemies()
    {
        //Save list reference
        List<SpawnEnemyDetails> _spawnPositions = currentSceneInfo.spawnEnemyDetails;

        //Check if the scene has enemies
        if (_spawnPositions.Count <= 0) return;

        for (int i = 0; i < _spawnPositions.Count; i++)
        {   
            //Check if enemy is dead
            if (!currentSceneData.enemyDead[i]) 
            {
                //Init values
                GameObject _enemyGO = GetEnemyType(_spawnPositions[i].enemyType);
                Enemy _enemyScript = _enemyGO.GetComponent<Enemy>();

                //Instantiate enemy
                GameObject.Instantiate(_enemyGO, _spawnPositions[i].spawnPos, Quaternion.identity);

                //Initialize enemy
                _enemyScript.SetEnemyIndex(i);
                //_enemyScript.Movement?.CheckInitFlip(_spawnPositions[i].facingDirection);
            }
        }
    }

    /// <summary>
    /// Gets the enemy type
    /// </summary>
    /// <param name="_enemyType">Enemy Type</param>
    /// <returns>Returs the enemy GameObject</returns>
    private GameObject GetEnemyType(EnemyType _enemyType)
    {
        GameObject _enemyGO = null;

        switch (_enemyType)
        {
            case EnemyType.Fire:
            {
                _enemyGO = gameManager.PrefabIndex.fireEnemy;
                break;
            }
            case EnemyType.Hooded:
            {
                _enemyGO = gameManager.PrefabIndex.hoodedEnemy;
                break;
            }
            case EnemyType.Fly:
            {
                _enemyGO = gameManager.PrefabIndex.flyEnemy;
                break;
            }
        }

        //Check if the enemy type is not implemented
        if (_enemyGO == null) Debug.LogError($"The {_enemyType} is not implemented");

        return _enemyGO;
    }

    #endregion
}