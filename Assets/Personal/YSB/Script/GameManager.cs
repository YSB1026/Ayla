using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    public Vector3 savePont;

    #region ���� �Ŵ���
    //�ӽ÷� �ۼ�
    //private PlayerManager playerManager;
    private UIManager uiManager;
    private SoundManager soundManager;
    private CustomSceneManager sceneManager;
    //private SkillManager skillManager;
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
        //playerManager = PlayerManager.Instance;
        uiManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
        sceneManager = CustomSceneManager.Instance;
        //skillManager = SkillManager.Instance;
        ChangeState(GameState.Lobby);
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
        //����� 
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
            case GameState.Cutscene:
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

            case GameState.Cutscene:
                break;

            case GameState.InGame:
                //���� respawn�̳� �Ҵ�Ʈ ȹ�� �Լ��� ���� ���� ���ٸ���
                //ó�� ���ص� �ɼ����ִµ� ���߿� �����丵 �غ��߰ڳ׿�
                //�׽�Ʈ��
                //if (Input.GetKeyDown(KeyCode.Alpha1))
                //{
                //    soundManager.PlayEnvSFX("Thunder1");
                //}
                //else if(Input.GetKeyDown(KeyCode.Alpha2))
                //{
                //    soundManager.PlayEnvSFX("Thunder2");
                //}
                break;

            case GameState.Respawn:
                // Fade in/out ó�������� ���ָ� ���ڳ׿�.
                // ������ ó���� �ܹ߼��̶�, game manager���� ���ص� �ɼ����ְڳ׿�.
                //Player Manager�� ����Ѵٰ��ϸ� �����丵 or game manager���� �帧�� ����ϴ�
                //game manager respawn -> player manager ������ �̷����ص� �ǰڳ׿�.
                RespawnPlayer();
                break;
        }
    }

    //�Ҵ�Ʈ ȹ�� ó��
    public void OnPendantCollected()
    {
        //soundManager���� ���� ���
        //uiManager���� �ƾ� ?
        //skill manager ȣ���ؼ� ���� ���϶��� �� ���� ��
        
        // �ƾ� or ���� ���·� ��ȯ
        ChangeState(GameState.Cutscene);
    }
    
    public void RespawnPlayer()
    {

		if (SceneManager.GetActiveScene().name == "Forest")
        {
            SpawnManager.Instance.PlayerSpawn(savePont, PlayerType.FOREST);
        }
        else if(SceneManager.GetActiveScene().name == "House")
        {
			SpawnManager.Instance.PlayerSpawn(savePont, PlayerType.HOUSE);
        }

        ChangeState(GameState.InGame);
    }

    public void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
