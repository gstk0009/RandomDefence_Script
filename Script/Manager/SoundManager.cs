using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance = 0.1f;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    private AudioSource musicAudioSource;
    public AudioClip musicClip;
    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        InitializeSliders();
        ApplyAudioSettings();

        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(SetMusicVolume);

        ChangeBackgroundMusic(musicClip);
    }

    public void ChangeBackgroundMusic(AudioClip newMusicClip)
    {
        StartCoroutine(FadeOutAndChangeMusic(newMusicClip));
    }

    private IEnumerator FadeOutAndChangeMusic(AudioClip newMusicClip)
    {
        float startVolume = musicAudioSource.volume;

        while (musicAudioSource.volume > 0)
        {
            musicAudioSource.volume -= startVolume * Time.deltaTime / 1.0f;
            yield return null;
        }

        musicAudioSource.Stop();
        musicAudioSource.clip = newMusicClip;
        musicAudioSource.Play();

        while (musicAudioSource.volume < musicVolume)
        {
            musicAudioSource.volume += startVolume * Time.deltaTime / 1.0f;
            yield return null;
        }
    }

    public static void PlayClip(string clipName)
    {
        if (_instance.soundEffects.TryGetValue(clipName, out AudioClip clip))
        {
            GameObject obj = GameManager.Instance.ObjectPool.SpawnFromPool("SoundSource");
            obj.SetActive(true);
            SoundSource soundSource = obj.GetComponent<SoundSource>();
            soundSource.Play(clip, _instance.soundEffectVolume, _instance.soundEffectPitchVariance, _instance.audioMixer.FindMatchingGroups("SFX")[0]);
        }
    }

    public void RegisterSoundEffect(string name, AudioClip clip)
    {
        if (!soundEffects.ContainsKey(name))
        {
            soundEffects.Add(name, clip);
        }
    }

    private void InitializeSliders()
    {
        if (masterSlider != null)
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        if (sfxSlider != null)
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        if (bgmSlider != null)
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.75f);
    }

    private void ApplyAudioSettings()
    {
        if (masterSlider != null) SetMasterVolume(masterSlider.value);
        if (sfxSlider != null) SetSFXVolume(sfxSlider.value);
        if (bgmSlider != null) SetMusicVolume(bgmSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void ReinitializeSliders()
    {
        masterSlider = GameObject.Find("MasterSlider")?.GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider")?.GetComponent<Slider>();
        bgmSlider = GameObject.Find("BGMSlider")?.GetComponent<Slider>();

        InitializeSliders();
        ApplyAudioSettings();

        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(SetMusicVolume);
    }
}
