using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour
{
    #region Singleton
    public static GameCoordinator Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields

    [HideInInspector] public GameplaySystem GameplaySystem;
    //[HideInInspector] public GameManager GameManager;
    public Screens Screens;

    #endregion
    
    #region Unity Methods
    
    private void Start()
    {
        Initialize();
    }
    
    #endregion
    
    private void Initialize()
    {
        //create gameplay system
        GameplaySystem = gameObject.AddComponent<GameplaySystem>();
        
        Screens.ShowHomeScreen();
    }

    #region Public Methods

    public async void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Action OnSceneLoadingCompleted = null)
    {
        var response = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        
        while (!response.isDone)
        {
            // Wait for the scene to load
            await Task.Yield();
        }
        
        OnSceneLoadingCompleted?.Invoke();
    }

    #endregion
}

public enum SceneName
{
    HomeScene,
    GamePlayScene
}