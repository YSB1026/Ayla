using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance {  get; private set; }
    [Header("Setting Button")]
    [SerializeField] private Button backButton;//로비로 돌아가는 버튼

    #region 로비 버튼
    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
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
    }

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnQuitToLobbyClicked);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnQuitToLobbyClicked()
    {
        UIManager.Instance.ToggleSettings();
        CustomSceneManager.Instance.LoadScene("Lobby");
    }

    #region Lobby 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            var lobbyCanvas = GameObject.Find("LobbyCanvas");
            if (lobbyCanvas == null)
            {
                Debug.LogWarning("[ButtonManager] Lobby Canvas cannot found");
                return;
            }

            TryFindButton(lobbyCanvas, "StartButton", ref startButton, OnPlayClicked);
            TryFindButton(lobbyCanvas, "SettingButton", ref settingsButton, OnSettingsClicked);
            TryFindButton(lobbyCanvas, "ExitButton", ref exitButton, OnExitClicked);
        }
    }

    private void TryFindButton(GameObject root, string name, ref Button button, UnityEngine.Events.UnityAction action)
    {
        var btnGO = root.transform.Find(name);
        if (btnGO != null)
        {
            button = btnGO.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogWarning($"[ButtonManager] {root} - {name} Button cannot found");
        }
    }

    private void OnPlayClicked()
    {
        //임시로 테스트
        CustomSceneManager.Instance.LoadScene("YSB_test");
    }

    private void OnSettingsClicked()
    {
        UIManager.Instance.ToggleSettings();
    }

    private void OnExitClicked()
    {
        GameManager.Instance.ExitGame();
    }
    #endregion
}
