using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _current;

    [Header("环境声音")]
    public AudioClip AmbientClip;
    public AudioClip MusicClip;

    [Header("FX音效")]
    public AudioClip DeathFXClip;
    public AudioClip OrbFxClip;
    public AudioClip OpenDoorClip;
    public AudioClip StartLevelClip;
    public AudioClip WinClip;


    [Header("Robbie音效")] public AudioClip[] WalkStepClips;
    public AudioClip[] CrouchStepClips;
    public AudioClip JumpClip;
    public AudioClip DeathClip;

    public AudioClip JumpVoiceClip;
    public AudioClip DeathVoiceClip;
    public AudioClip OrbVoiceClip;

    private AudioSource _ambientSource;
    private AudioSource _musicSource;
    private AudioSource _fxSource;
    private AudioSource _playerSource;
    private AudioSource _voiceSource;

    public AudioMixerGroup AmbientGroup, MusicGroup, FXGroup, PlayerGroup, VoiceGroup;

    void Awake()
    {
        if (_current != null)
        {
            Destroy(gameObject);
            return;
        }
        _current = this;
        DontDestroyOnLoad(gameObject);

        _ambientSource = gameObject.AddComponent<AudioSource>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        _fxSource = gameObject.AddComponent<AudioSource>();
        _playerSource = gameObject.AddComponent<AudioSource>();
        _voiceSource = gameObject.AddComponent<AudioSource>();

        _ambientSource.outputAudioMixerGroup = AmbientGroup;
        _musicSource.outputAudioMixerGroup = MusicGroup;
        _fxSource.outputAudioMixerGroup = FXGroup;
        _playerSource.outputAudioMixerGroup = PlayerGroup;
        _voiceSource.outputAudioMixerGroup = VoiceGroup;

        StartLevelAudio();

    }

    void StartLevelAudio()
    {
        _current._ambientSource.clip = _current.AmbientClip;
        _current._ambientSource.loop = true;
        _current._ambientSource.Play();

        _current._musicSource.clip = _current.MusicClip;
        _current._musicSource.loop = true;
        _current._musicSource.Play();

        _current._fxSource.clip = _current.StartLevelClip;
        _current._fxSource.Play();
    }

    public static void PlayerWinAudio()
    {
        _current._fxSource.clip = _current.WinClip;
        _current._fxSource.Play();
        _current._playerSource.Stop();
    }


    public static void PlayFootStepAudio()
    {
        int index = Random.Range(0, _current.WalkStepClips.Length);
        _current._playerSource.clip = _current.WalkStepClips[index];
        _current._playerSource.Play();
    }

    public static void PlayCrouchFootStepAudio()
    {
        int index = Random.Range(0, _current.CrouchStepClips.Length);
        _current._playerSource.clip = _current.CrouchStepClips[index];
        _current._playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        _current._playerSource.clip = _current.JumpClip;
        _current._playerSource.Play();

        _current._voiceSource.clip = _current.JumpVoiceClip;
        _current._voiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        _current._playerSource.clip = _current.DeathClip;
        _current._playerSource.Play();

        _current._voiceSource.clip = _current.DeathVoiceClip;
        _current._voiceSource.Play();

        _current._fxSource.clip = _current.DeathFXClip;
        _current._fxSource.Play();

    }

    public static void PlayDoorOpenAudio()
    {
        _current._fxSource.clip = _current.OpenDoorClip;
        _current._fxSource.PlayDelayed(1.0f);
    }

    public static void PlayOrbAudio()
    {
        _current._fxSource.clip = _current.OrbFxClip;
        _current._fxSource.Play();

        _current._voiceSource.clip = _current.OrbVoiceClip;
        _current._voiceSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
