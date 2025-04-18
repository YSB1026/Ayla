using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    #region ���� �Ŵ���
    //�ӽ÷� �ۼ�
    //public PlayerManager playerManager;
    //public UIManager uiManager;
    //public SoundManager soundManager;
    //public CustomSceneManager sceneManager;
    //public SkillManager skillManager;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //playerManager = PlayerManager.Instance;
        //uiManager = UIManager.Instance;
        //soundManager = SoundManager.Instance;
        //sceneManager = CustomSceneManager.Instance;
        //skillManager = SkillManager.Instance;
        //CurrentState = GameState.Title; //ó������ Ÿ��Ʋ
        CurrentState = GameState.InGame;
    }

    private void Update()
    {
        HandleState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC �������");
            TogglePause();
        }
    }

    public void InitGame()
    {
        ChangeState(GameState.Title);
    }

    public void ChangeState(GameState newState)
    {
        Debug.Log("change state ȣ�� �ƾ��.");
        Debug.Log("���� " + CurrentState + " ������Ʈ �ٲ�����.");

        CurrentState = newState;
        // ���� ���� �� ���� ó��
        switch (newState)
        {
            case GameState.Title:
                //���� �Ŵ����� ȣ��?
                //�������� 
                break;
            case GameState.Cutscene:
                //���� �Ŵ����� ȣ��?
                break;
            case GameState.InGame:
                Time.timeScale = 1f;
                //���� �Ŵ����� ȣ��?
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                //���� �Ŵ����� ȣ��?
                break;
            case GameState.Respawn:
                //���� �Ŵ����� ȣ��?
                break;
        }
    }
    private void HandleState()
    {
        switch (CurrentState)
        {
            case GameState.Title:
                //���� �Ŵ��� ȣ��?
                break;

            case GameState.Cutscene:
                //���� �Ŵ��� ȣ��?
                break;

            case GameState.InGame:
                //���� respawn�̳� �Ҵ�Ʈ ȹ�� �Լ��� ���� ���� ���ٸ���
                //ó�� ���ص� �ɼ����ִµ� ���߿� �����丵 �غ��߰ڳ׿�
                break;

            case GameState.Paused:
                //���� ���¿����� ������ ó�� ���ص� �� �� ���ƿ�.
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
    private void TogglePause()
    {
        Debug.Log("��� ���� ȣ��ƾ��.");
        if (CurrentState == GameState.Paused)
            ResumeGame();
        else if (CurrentState == GameState.InGame)
            PauseGame();
    }

    private void PauseGame()
    {
        ChangeState(GameState.Paused);
    }

    private void ResumeGame()
    {
        ChangeState(GameState.InGame);
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
        //���� ������ ������ �÷��̾� �Ŵ������� Respawn�� �����ϴ°� �����Ű��ƿ�
        //playerManager.Respawn();
        ChangeState(GameState.InGame);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITO
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
