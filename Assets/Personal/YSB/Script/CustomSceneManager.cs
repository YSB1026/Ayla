using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager Instance { get; private set; }

    private Scene houseScene;
    private string additiveSceneName;

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

        if (SceneManager.GetActiveScene().name == "House")
        {
            houseScene = SceneManager.GetActiveScene();
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

        if(houseScene == null && sceneName == "House")
        {
            houseScene = SceneManager.GetActiveScene();
        }
    }

    public void ReloadScene()
    {
        if (!string.IsNullOrEmpty(additiveSceneName))
        {
            StartCoroutine(ReloadAdditiveSceneCoroutine());
        }
    }

    private IEnumerator ReloadAdditiveSceneCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(additiveSceneName);
        while (!unloadOp.isDone)
            yield return null;

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(additiveSceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        UIManager.Instance.FadeIn();

        Scene additiveScene = SceneManager.GetSceneByName(additiveSceneName);
        if (additiveScene.IsValid())
        {
            SceneManager.SetActiveScene(additiveScene);
        }


        PlaySceneBGM(additiveSceneName);
    }

    public void LoadSceneFromTimeline(string sceneName)
    {
        LoadScene(sceneName);
    }

    public void LoadAdditiveScene(string additiveSceneNamePram)
    {
        additiveSceneName = additiveSceneNamePram;

        StartCoroutine(LoadSceneAdditiveAndDeactivateCurrent(additiveSceneName));
    }

    public void UnloadAdditiveScene()
    {
        if (!string.IsNullOrEmpty(additiveSceneName))
        {
            SceneManager.UnloadSceneAsync(additiveSceneName).completed += (op) =>
            {
                if (houseScene.IsValid() && houseScene.isLoaded)
                {
                    SetSceneActive(houseScene);
                }
            };
        }
    }

    private IEnumerator LoadSceneAdditiveAndDeactivateCurrent(string memorySceneName)
    {
        // 현재 씬의 루트 오브젝트 비활성화
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

        // 활성 씬으로 설정
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
                SoundManager.Instance.StopBGM();
                break;
        }
    }
}
