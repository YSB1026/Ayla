using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //setting panel이랑 paused panel 같아도 될거같은데
    [Header("공통 UI")]
    [Space]

    [Header("Setting")]    
    [SerializeField] private GameObject settingsPanel;
    private bool isSettingsOpen = false;
    [Header("Fade Image")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
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
    public void FadeIn(System.Action onComplete = null)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(1, 0, () => {
            fadeImage.gameObject.SetActive(false); 
            onComplete?.Invoke();
        }));
    }
    public void FadeOut(System.Action onComplete = null)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0, 1, onComplete));
    }
    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, System.Action onComplete)
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        onComplete?.Invoke();
    }
    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);

        if (isSettingsOpen)
        {
            GameManager.Instance.ChangeState(GameState.Paused);
        }
        else
        {
            GameManager.Instance.ChangeState(GameState.InGame);
        }
    }

    public bool IsSettingsOpen()
    {
        return isSettingsOpen;
    }
}
