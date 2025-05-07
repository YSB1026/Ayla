using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    public void UnloadMemoryScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        CustomSceneManager.Instance.UnloadMemoryScene(currentSceneName);
    }
}
