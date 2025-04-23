using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    #region ���� �Ŵ���
    //�ӽ÷� �ۼ�
    //public PlayerManager playerManager;
    public UIManager uiManager;
    public SoundManager soundManager;
    public CustomSceneManager sceneManager;
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
            TogglePause();
        }

        //if (Input.GetKeyDown(KeyCode.H))//�׽�Ʈ
        //{
        //    uiManager.FadeOut(() => {
        //        sceneManager.LoadScene("YSB_test2");
        //        uiManager.FadeIn();
        //    });
        //    soundManager.StopAllSFX();
        //}
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
                //���� �Ŵ����� ȣ��?
                //�������� 
                break;
            case GameState.Cutscene:
                //���� �Ŵ����� ȣ��?
                break;
            case GameState.InGame:
                //���� �Ŵ����� ȣ��?
                Time.timeScale = 1f;
                soundManager.PlayBGM("MainTheme");//�׽�Ʈ��
                break;
            case GameState.Paused:
                //���� �Ŵ����� ȣ��?
                Time.timeScale = 0f;
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
            case GameState.Lobby:
                uiManager.FadeOut(() => {
                    sceneManager.LoadScene("Lobby");
                    uiManager.FadeIn();
                });
                soundManager.PlayBGM("MainTheme");
                //���� �Ŵ��� ȣ��?
                break;

            case GameState.Cutscene:
                //���� �Ŵ��� ȣ��?
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
