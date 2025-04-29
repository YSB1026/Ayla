using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager Instance { get; private set; }

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

    public void LoadScene(string sceneName)
    {
        PlaySceneBGM(sceneName);

        if (SceneManager.GetActiveScene().name == sceneName)
        {
            return;
        }

        UIManager.Instance.FadeOut(() => {
            SceneManager.LoadScene(sceneName);
            UIManager.Instance.FadeIn();
        });
    }

    private void PlaySceneBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "Lobby":
                SoundManager.Instance.PlayBGM("MainTheme");
                break;
            case "Forest"://나중에 수정할 것
                SoundManager.Instance.PlayBGM("ForestBGM");
                break;
            default:
                break;
        }
    }
}