using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    //public Vector3 savePoint;
    public SaveData currentSave { get; private set; } = new SaveData();

    #region Private Fields
    private Player currentPlayer => GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    private UIManager uiManager;
    private SoundManager soundManager;
    private CustomSceneManager sceneManager;
    private SpawnManager spawnManager;
    private LightColorController lightColorController;
    #endregion

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
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        //playerManager = PlayerManager.Instance;
        uiManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
        sceneManager = CustomSceneManager.Instance;
        spawnManager = SpawnManager.Instance;
        //skillManager = SkillManager.Instance;
        //ChangeState(GameState.Lobby);
    }

    private void Update()
    {
        HandleState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.ToggleSettings();
        }
    }

    public void InitGame()
    {
        //ChangeState(GameState.Lobby);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Lobby:
                sceneManager.LoadScene("Lobby");
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.InGame:
                Time.timeScale = 1f;
                break;
            case GameState.Respawn:
                break;
        }
    }
    private void HandleState()
    {
        switch (CurrentState)
        {
            case GameState.Lobby:
                break;

            case GameState.Paused:
                break;

            case GameState.InGame:
                break;

            case GameState.Respawn:
                UIManager.Instance.FadeOut(() => {
                    UIManager.Instance.FadeIn();
                });
                RespawnPlayer();
                break;
        }
    }
    
    public void RegistLightController(LightColorController controller)
    {
        lightColorController = controller;
    }
    public void OnPendantCollected(LightColorController.ColorOption color)
    {
        Debug.Log($"[GameManager] Pendant Collected: {color}");
        currentSave.pendantColor = color;

        if(lightColorController != null)
        {
            lightColorController.ChangeColorWithFade(color);
        }
        //LightManager.Instance?.SetLightColor(color);

        //uiManager.PlayPendantCutscene(color);
        //soundManager.PlayPendantCollectSFX(color);

        //sceneManager.LoadAdditiveScene(sceneName);
    }

    public void SetSavePoint(Vector3 point)
    {
        currentSave.savePoint = point;
        //Debug.Log($"[GameManager] Save Point Set: {point}");
    }

    public void RespawnPlayer()
    {
        if (SceneManager.GetActiveScene().name == "Forest")
        {
            spawnManager.PlayerSpawn(currentSave.savePoint, PlayerType.FOREST);
        }
        else if (SceneManager.GetActiveScene().name == "House")
        {
            spawnManager.PlayerSpawn(currentSave.savePoint, PlayerType.HOUSE);
        }

        ChangeState(GameState.InGame);
    }

    public void SetPlayerControlEnabled(bool isEnabled)
    {
        if (currentPlayer != null)
        {
            currentPlayer.GetComponent<Player>().SetControlEnabled(isEnabled);
        }
    }

    public void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
