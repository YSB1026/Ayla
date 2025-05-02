using System.Collections;
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

    [Header("BGM Clips")]
    [SerializeField] private List<AudioClip> bgmClips;
    private Dictionary<string, int> bgmDict = new();

    #region Player
    [Header("Player FootStep SFX Clips")]
    [SerializeField] private List<AudioClip> footstepForestClips;
    [SerializeField] private List<AudioClip> footstepStoneClips;
    [SerializeField] private List<AudioClip> footstepWoodClips;
    private Dictionary<SurfaceType, List<AudioClip>> footstepDict = new();

    [Header("Player Crawling SFX Clips")]
    [SerializeField] private List<AudioClip> crawlingForestClips;
    [SerializeField] private List<AudioClip> crawlingStoneClips;
    [SerializeField] private List<AudioClip> crawlingWoodClips;
    private Dictionary<SurfaceType, List<AudioClip>> crawlingDict = new();

    private enum EntitySoundType { Footstep, Crawling }
    private HashSet<EntitySoundType> playingSounds = new();
    #endregion

    [Header("Enemy SFX Clips")]
    [SerializeField] private List<AudioClip> enemySfxClips;
    private Dictionary<string, int> enemySfxDict = new();

    [Header("Ambience SFX Clips")]
    [SerializeField] private List<AudioClip> ambienceSfxClips;
    private Dictionary<string, int> ambienceSfxDict = new();

    [Header("Environment SFX Clips")]
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
        bgmDict["MainTheme"] = 0;
        bgmDict["ForestBGM"] = 1;

        footstepDict[SurfaceType.Forest] = footstepForestClips;
        footstepDict[SurfaceType.Stone] = footstepStoneClips;
        footstepDict[SurfaceType.Wood] = footstepWoodClips;
        footstepDict[SurfaceType.Stair] = footstepStoneClips;

        crawlingDict[SurfaceType.Forest] = crawlingForestClips;
        crawlingDict[SurfaceType.Stone] = crawlingStoneClips;
        crawlingDict[SurfaceType.Wood] = crawlingWoodClips;

        envSfxDict["Thunder1"] = 0;
        envSfxDict["Thunder2"] = 1;

        ambienceSfxDict["ForestAmbience"] = 0;
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

    #region Ambience
    public void PlayAmbienceSFX(string key)
    {
        if (!ambienceSfxDict.TryGetValue(key, out int idx))
        {
            Debug.LogWarning($"[SoundManager] Ambience '{key}' not found!");
            return;
        }

        StopAmbience();
        ambienceSource.clip = ambienceSfxClips[idx];
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
        if (!footstepDict.TryGetValue(type, out var clipList)) return;
        PlayPlayerSound(EntitySoundType.Footstep, clipList);
    }

    public void PlayCrawling(SurfaceType type)
    {
        if (!crawlingDict.TryGetValue(type, out var clipList)) return;
        PlayPlayerSound(EntitySoundType.Crawling, clipList);
    }

    private void PlayPlayerSound(EntitySoundType soundType, List<AudioClip> clips)
    {
        if (playingSounds.Contains(soundType)) return;

        if (clips == null || clips.Count == 0)
        {
            Debug.LogWarning($"[SoundManager] No clips found for {soundType}");
            return;
        }

        int randomIdx = Random.Range(0, clips.Count);
        AudioClip clip = clips[randomIdx];

        entitySource.PlayOneShot(clip);
        StartCoroutine(WaitForSoundEnd(clip.length, soundType));
    }

    private IEnumerator WaitForSoundEnd(float clipLength, EntitySoundType type)
    {
        playingSounds.Add(type);
        yield return new WaitForSeconds(clipLength);
        playingSounds.Remove(type);
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
