using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("����� �ҽ�")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("���� ����")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Audio Clip List")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    // =============================
    // BGM ���
    // =============================
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[SoundManager] BGM Clip is null!");
            return;
        }

        if (bgmSource.clip == clip)
            return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlayBGM(string clipName)
    {
        AudioClip clip = FindClipByName(bgmClips, clipName);

        if (clip != null)
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] BGM Clip '{clipName}' not found!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // =============================
    // ȿ���� ���
    // =============================
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[SoundManager] SFX Clip is null!");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFX(string clipName)
    {
        AudioClip clip = FindClipByName(sfxClips, clipName);

        if (clip != null)
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] SFX Clip '{clipName}' not found!");
        }
    }

    // =============================
    // ���� ����
    // =============================
    public void StopAllSound()
    {
        bgmSource.Stop();
        sfxSource.Stop();
    }

    // =============================
    // Ŭ�� �̸����� �˻�
    // =============================
    private AudioClip FindClipByName(AudioClip[] clips, string clipName)
    {
        foreach (var clip in clips)
        {
            if (clip != null && clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }
}
