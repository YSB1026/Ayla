using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager Instance { get; private set; }

    private Scene previousScene;
    private string memorySceneName; // ȸ�� �� �̸� �����

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
            return;

        UIManager.Instance.FadeOut(() => {
            SceneManager.LoadScene(sceneName);
            UIManager.Instance.FadeIn();
        });
    }

    public void LoadSceneFromTimeline(string sceneName)
    {
        LoadScene(sceneName);
    }

    public void LoadMemoryScene(string memorySceneNameParam)
    {
        previousScene = SceneManager.GetActiveScene();
        memorySceneName = memorySceneNameParam;

        StartCoroutine(LoadSceneAdditiveAndDeactivateCurrent(memorySceneName));
    }

    public void UnloadMemoryScene()
    {
        if (!string.IsNullOrEmpty(memorySceneName))
        {
            SceneManager.UnloadSceneAsync(memorySceneName).completed += (op) =>
            {
                if (previousScene.IsValid() && previousScene.isLoaded)
                {
                    SetSceneActive(previousScene);
                }
            };
        }
    }

    private IEnumerator LoadSceneAdditiveAndDeactivateCurrent(string memorySceneName)
    {
        // ���� ���� ��Ʈ ������Ʈ ��Ȱ��ȭ
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

        // Ȱ�� ������ ����
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
