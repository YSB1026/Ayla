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
//        // �� �ε�
//        CustomSceneManager.Instance.LoadScene(sceneName);
//    }
//    public void UnloadMemoryScene(string memorySceneName)
//    {
//        // ȸ�� �� ��ε�
//        SceneManager.UnloadSceneAsync(memorySceneName);
//    }

//    private void PlaySceneBGM(string sceneName)
//    {
//        switch (sceneName)
//        {
//            case "Lobby":
//                SoundManager.Instance.PlayBGM("MainTheme");
//                break;
//            case "Forest"://���߿� ������ ��
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

        // ���� �� �ٽ� Ȱ��ȭ
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
        // ���� �� ��Ȱ��ȭ
        Scene current = SceneManager.GetActiveScene();
        GameObject[] rootObjects = current.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }

        // ȸ�� �� additive�� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(memorySceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ȸ�� ���� Ȱ�� ������ ����
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

        // ��Ʈ ������Ʈ �ٽ� Ȱ��ȭ
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
