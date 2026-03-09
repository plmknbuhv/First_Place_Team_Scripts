using Code.Core;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxPrefab;
    [SerializeField] private int sfxPoolSize = 10;

    private AudioSource[] sfxPool;
    private int sfxIndex;

    private const string BGM_PARAM = "BGMVolume";
    private const string SFX_PARAM = "SFXVolume";

    #region Unity

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        InitSFXPool();
    }

    #endregion

    #region Init

    private void InitSFXPool()
    {
        sfxPool = new AudioSource[sfxPoolSize];

        for (int i = 0; i < sfxPoolSize; i++)
        {
            var src = Instantiate(sfxPrefab, transform);
            src.playOnAwake = false;
            sfxPool[i] = src;
        }
    }

    #endregion

    #region BGM

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    #endregion

    #region SFX

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        var src = sfxPool[sfxIndex];
        sfxIndex = (sfxIndex + 1) % sfxPool.Length;

        src.clip = clip;
        src.Play();
    }

    public void PlayUISFX(AudioClip clip)
    {
        PlaySFX(clip);
    }

    #endregion

    #region Volume

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat(BGM_PARAM, LinearToDb(value));
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(SFX_PARAM, LinearToDb(value));
    }

    private float LinearToDb(float value)
    {
        if (value <= 0f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }

    #endregion


    public static void PlayBGMStatic(AudioClip clip)
        => Instance?.PlayBGM(clip);

    public static void PlaySFXStatic(AudioClip clip)
        => Instance?.PlaySFX(clip);
}
