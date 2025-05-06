using NUnit.Framework;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CustomSceneManager.Instance.LoadScene(sceneName);
        }
    }
}
