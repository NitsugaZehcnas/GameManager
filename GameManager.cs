using System.Collections;
using UnityEngine;

/// <summary>
/// Controlls all the game
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    
    //Singleton
    public static GameManager Instance { get; private set; }
    
    //Variables
    public GameManagerStateMachine StateMachine { get => stateMachine; }
    private GameManagerStateMachine stateMachine;

    public SO_PrefabIndex PrefabIndex { get => prefabIndex; }
    private SO_PrefabIndex prefabIndex;

    public Player Player { get => player; }
    private Player player;

    //Components
    public DataManager DataManager { get => dataManager; }
    public LocationManager LocationManager {get => locationManager; }
    public UIManager UIManager { get => uiManager; }
    public InputManager InputManager { get => inputManager; }
    public LoadSceneManager LoadSceneManager { get => loadSceneManager; }
    public EffectsManager EffectsManager { get => effectsManager; }

    private DataManager dataManager;
    private LocationManager locationManager;
    private UIManager uiManager;
    private InputManager inputManager;
    private LoadSceneManager loadSceneManager;
    private EffectsManager effectsManager;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        //Create a singleton
        CreateSingleton();

        //Load prefab index
        prefabIndex = Resources.Load<SO_PrefabIndex>("ScriptableObjects/PrefabIndex");

        //Init all the components
        InitializeComponents();

        //Create a initialize State MAchine
        stateMachine = new GameManagerStateMachine();
        StateMachine.Initialize(this);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        UpdateComponents(dt);

        StateMachine.LogicUpdate(dt);
        StateMachine.CurrentState.LogicUpdate();

        //Update the player
        if(player != null) player.LogicUpdate(dt);
    }

    private void FixedUpdate() 
    {
        float dt = Time.deltaTime;

        //Update the player
        if(player != null) player.PhysicsUpdate(dt);
    }

    #endregion
    
    #region GameManeger Methods
    
    /// <summary>
    /// Creates the singleton
    /// </summary>
    private void CreateSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Catch all the references and init all the componentes
    /// </summary>
    private void InitializeComponents()
    {
        locationManager = GetComponentInChildren<LocationManager>();
        dataManager = GetComponentInChildren<DataManager>();
        uiManager = GetComponentInChildren<UIManager>();
        inputManager = GetComponentInChildren<InputManager>();
        loadSceneManager = GetComponentInChildren<LoadSceneManager>();
        effectsManager = GetComponentInChildren<EffectsManager>();

        locationManager.Initialize(this);
        dataManager.Initialize(this);
        uiManager.Initialize(this);
        inputManager.Initialize(this);
        loadSceneManager.Initialize(this);
        effectsManager.Initialize(this);
    }

    /// <summary>
    /// Update all the components
    /// </summary>
    /// <param name="_dt">Delta Time</param>
    private void UpdateComponents(float _dt)
    {
        inputManager.DoUpdate(_dt);
        loadSceneManager.DoUpdate(_dt);
        effectsManager.DoUpdate(_dt);
    }

    #endregion

    #region Set Methods

    public void InitPlayer(Player _player) 
    {
        player = _player;
        player.Initialize();
    } 

    #endregion
}
