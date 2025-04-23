using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    #region 참조 매니저
    //임시로 작성
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

        //if (Input.GetKeyDown(KeyCode.H))//테스트
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
        //고민중 
        //ChangeState(GameState.Lobby);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Lobby:
                //각종 매니저들 호출?
                //사운드라든지 
                break;
            case GameState.Cutscene:
                //각종 매니저들 호출?
                break;
            case GameState.InGame:
                //각종 매니저들 호출?
                Time.timeScale = 1f;
                soundManager.PlayBGM("MainTheme");//테스트용
                break;
            case GameState.Paused:
                //각종 매니저들 호출?
                Time.timeScale = 0f;
                break;
            case GameState.Respawn:
                //각종 매니저들 호출?
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
                //각종 매니저 호출?
                break;

            case GameState.Cutscene:
                //각종 매니저 호출?
                break;

            case GameState.InGame:
                //제가 respawn이나 팬던트 획득 함수를 따로 만들어서 별다른걸
                //처리 안해도 될수도있는데 나중에 리팩토링 해봐야겠네요
                //테스트용
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
                //정지 상태에서는 별도로 처리 안해도 될 거 같아요.
                break;

            case GameState.Respawn:
                // Fade in/out 처리같은걸 해주면 좋겠네요.
                // 리스폰 처리는 단발성이라, game manager에서 안해도 될수도있겠네요.
                //Player Manager가 담당한다고하면 리팩토링 or game manager에서 흐름만 담당하니
                //game manager respawn -> player manager 리스폰 이렇게해도 되겠네요.
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

    //팬던트 획득 처리
    public void OnPendantCollected()
    {
        //soundManager에서 사운드 재생
        //uiManager에서 컷씬 ?
        //skill manager 호출해서 현재 에일라의 빛 설정 등
        
        // 컷씬 or 다음 상태로 전환
        ChangeState(GameState.Cutscene);
    }
    
    public void RespawnPlayer()
    {
        //실제 리스폰 로직은 플레이어 매니저에서 Respawn을 구현하는게 좋을거같아요
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
