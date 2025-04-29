using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("오디오 소스")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource entitySource;
    [SerializeField] private AudioSource envSource;
    [SerializeField] private AudioSource ambienceSource;

    [Header("BGM Clips")]//bgm
    [SerializeField] private List<AudioClip> bgmClips;
    private Dictionary<string, int> bgmDict = new();

    [Header("Player SFX Clips")]//player sfx
    [SerializeField] private List<AudioClip> playerSfxClips;
    private Dictionary<string, int> playerSfxDict = new();

    [Header("Enemy SFX Clips")]//enemy sfx
    [SerializeField] private List<AudioClip> enemySfxClips;
    private Dictionary<string, int> enemySfxDict = new();

    [Header("Ambience SFX Clips")]//bgm 배경으로 깔리는 sfx
    [SerializeField] private List<AudioClip> ambienceSfxClips;
    private Dictionary<string, int> ambienceSfxDict = new();

    [Header("Environment SFX Clips")]//천둥같은 sfx
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

        //bgm
        #region BGM 
        bgmDict["MainTheme"] = 0;
        #endregion

        #region Player
        playerSfxDict["Footstep_Forest"] = 0;
        #endregion

        #region Enemy
        //enemySfxDict["Walk"] = 0;
        #endregion

        #region Environment
        envSfxDict["Thunder1"] = 0;
        envSfxDict["Thunder2"] = 1;
        #endregion

        #region ambience
        ambienceSfxDict["ForestAmbience"] = 0;
        #endregion

        /*
        //for (int i = 0; i < bgmClips.Count; i++)
        //{
        //    bgmDict[bgmClips[i].name] = i;
        //}

        //for (int i = 0; i < playerSfxClips.Count; i++)
        //{
        //    playerSfxDict[playerSfxClips[i].name] = i;
        //}

        //for (int i = 0; i < enemySfxClips.Count; i++)
        //{
        //    enemySfxDict[enemySfxClips[i].name] = i;
        //}

        //for (int i = 0; i < envSfxClips.Count; i++)
        //{
        //    envSfxDict[envSfxClips[i].name] = i;
        //}
        */
    }

    //public void StopAllSFX()
    //{
    //    bgmSource.Stop();
    //    entitySource.Stop();
    //}

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

    #region Ambience
    public void PlayAmbienceSFX(string key)
    {
        if (!ambienceSfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Ambience '{key}' not found!");
            return;
        }

        StopAmbience();

        ambienceSource.clip = bgmClips[idx];
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    public void StopAmbience()
    {
        if (ambienceSource.isPlaying && ambienceSource.loop)
            ambienceSource.Stop();
    }

    #endregion

    #region Entity
    public void PlayPlayerSFX(string key)
    {
        if (!playerSfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Player SFX '{key}' not found!");
            return;
        }
        entitySource.PlayOneShot(playerSfxClips[idx]);
    }

    public void PlayEnemySFX(string key)
    {
        if (!enemySfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Enemy SFX '{key}' not found!");
            return;
        }
        entitySource.PlayOneShot(enemySfxClips[idx]);
    }

    public void PlayFootstep(SurfaceType type)
    {
        switch (type)
        {
            case SurfaceType.Forest:
                PlayPlayerSFX("Footstep_Forest");
                break;
            case SurfaceType.Stone:
                //PlayPlayerSFX("Footstep_Stone");
                break;
            case SurfaceType.Wood:
                //PlayPlayerSFX("Footstep_Wood");
                break;
            default:
                Debug.LogWarning("[SoundManager] - Something wrong with Ground Layer");
                break;
        }
    }
    #endregion

    #region Env
    public void PlayEnvSFX(string key)
    {
        if (!envSfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Env SFX '{key}' not found!");
            return;
        }
        envSource.PlayOneShot(envSfxClips[idx]);
    }
    #endregion

}
