//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CustomSceneManager : MonoBehaviour
//{
//    public static CustomSceneManager Instance { get; private set; }

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void LoadScene(string sceneName)
//    {
//        PlaySceneBGM(sceneName);

//        if (SceneManager.GetActiveScene().name == sceneName)
//        {
//            return;
//        }

//        UIManager.Instance.FadeOut(() => {
//            SceneManager.LoadScene(sceneName);
//            UIManager.Instance.FadeIn();
//        });
//    }

//    public void LoadSceneFromTimeline(string sceneName)
//    {
//        // 씬 로딩
//        CustomSceneManager.Instance.LoadScene(sceneName);
//    }
//    public void UnloadMemoryScene(string memorySceneName)
//    {
//        // 회상 씬 언로드
//        SceneManager.UnloadSceneAsync(memorySceneName);
//    }

//    private void PlaySceneBGM(string sceneName)
//    {
//        switch (sceneName)
//        {
//            case "Lobby":
//                SoundManager.Instance.PlayBGM("MainTheme");
//                break;
//            case "Forest"://나중에 수정할 것
//                SoundManager.Instance.PlayBGM("ForestBGM");
//                break;
//            default:
//                break;
//        }
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager Instance { get; private set; }

    private Scene previousScene;

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

    public void LoadSceneFromTimeline(string sceneName)
    {
        LoadScene(sceneName);
    }

    public void UnloadMemoryScene(string memorySceneName)
    {
        SceneManager.UnloadSceneAsync(memorySceneName);

        // 이전 씬 다시 활성화
        if (previousScene.IsValid())
        {
            SetSceneActive(previousScene);
        }
    }

    public void LoadMemoryScene(string memorySceneName)
    {
        previousScene = SceneManager.GetActiveScene();

        StartCoroutine(LoadSceneAdditiveAndDeactivateCurrent(memorySceneName));
    }

    private IEnumerator LoadSceneAdditiveAndDeactivateCurrent(string memorySceneName)
    {
        // 현재 씬 비활성화
        Scene current = SceneManager.GetActiveScene();
        GameObject[] rootObjects = current.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }

        // 회상 씬 additive로 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(memorySceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 회상 씬을 활성 씬으로 설정
        Scene memoryScene = SceneManager.GetSceneByName(memorySceneName);
        if (memoryScene.IsValid())
        {
            SceneManager.SetActiveScene(memoryScene);
        }

        PlaySceneBGM(memorySceneName);
    }

    private void SetSceneActive(Scene scene)
    {
        SceneManager.SetActiveScene(scene);

        // 루트 오브젝트 다시 활성화
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(true);
        }

        PlaySceneBGM(scene.name);
    }

    private void PlaySceneBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "Lobby":
                SoundManager.Instance.PlayBGM("MainTheme");
                break;
            case "Forest":
                SoundManager.Instance.PlayBGM("ForestBGM");
                break;
            default:
                break;
        }
    }
}
