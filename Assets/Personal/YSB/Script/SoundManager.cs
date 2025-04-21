using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("오디오 소스")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private List<AudioClip> bgmClips;
    private Dictionary<string, int> bgmDict = new();

    [Header("Player SFX Clips")]
    [SerializeField] private List<AudioClip> playerSfxClips;
    private Dictionary<string, int> playerSfxDict = new();

    [Header("Enemy SFX Clips")]
    [SerializeField] private List<AudioClip> enemySfxClips;
    private Dictionary<string, int> enemySfxDict = new();

    [Header("Env SFX Clips")]
    [SerializeField] private List<AudioClip> envSfxClips;
    private Dictionary<string, int> envSfxDict = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionaries()
    {
        //List 순서에 맞게 Key - Value 설정해주세요 :)

        bgmDict["MainTheme"] = 0;

        playerSfxDict["Walk"] = 0;

        //enemySfxDict["Walk"] = 0;

        envSfxDict["Thunder1"] = 0;
        envSfxDict["Thunder2"] = 1;
    }

    public void StopAllSFX()
    {
        bgmSource.Stop();
        sfxSource.Stop();
    }

    #region BGM
    public void PlayBGM(string key)
    {
        if (!bgmDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] BGM '{key}' not found!");
            return;
        }

        StopBGM();

        bgmSource.clip = bgmClips[idx];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying && bgmSource.loop)
            bgmSource.Stop();
    }
    #endregion

    #region SFX
    public void PlayPlayerSFX(string key)
    {
        if (!playerSfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Player SFX '{key}' not found!");
            return;
        }
        sfxSource.PlayOneShot(playerSfxClips[idx]);
    }

    public void PlayEnemySFX(string key)
    {
        if (!enemySfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Enemy SFX '{key}' not found!");
            return;
        }
        sfxSource.PlayOneShot(enemySfxClips[idx]);
    }

    public void PlayEnvSFX(string key)
    {
        if (!envSfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Env SFX '{key}' not found!");
            return;
        }
        sfxSource.PlayOneShot(envSfxClips[idx]);
    }

    #endregion
}
