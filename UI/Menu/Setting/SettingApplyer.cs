using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingApplyer : MonoBehaviour
{
    [SerializeField] private GameDataSO data;

    [Header("UI")]
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider bgm;
    [SerializeField] private Slider master;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("SFX")]
    [SerializeField] private AudioClip changeSFX;

    private const string SFX_PARAM = "SFXVolume";
    private const string BGM_PARAM = "BGMVolume";
    private const string MASTER_PARAM = "MASTERVolume";

    private void Start()
    {
        GetData();
    }

    public void GetData()
    {
        sfx.value = data.SFX;
        bgm.value = data.BGM;
        master.value = data.MASTER;
    }

    public void OnSFXChanged(float value)
    {
        data.SFX = value;
        audioMixer.SetFloat(SFX_PARAM, LinearToDb(value));
        SoundManager.Instance.PlayUISFX(changeSFX);
    }

    public void OnBGMChanged(float value)
    {
        data.BGM = value;
        audioMixer.SetFloat(BGM_PARAM, LinearToDb(value));
        SoundManager.Instance.PlayUISFX(changeSFX);
    }
    public void OnMASTERChanged(float value)
    {
        data.MASTER = value;
        audioMixer.SetFloat(MASTER_PARAM, LinearToDb(value));
        SoundManager.Instance.PlayUISFX(changeSFX);
    }
    private float LinearToDb(float value)
    {
        if (value <= 0f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }
}
