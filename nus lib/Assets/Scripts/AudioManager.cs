using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sourceA;
    public AudioSource sourceB;
    private bool usingA = true;
    public float fadeDuration = 1.5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CrossfadeTo(AudioClip newClip)
    {
        StartCoroutine(Crossfade(newClip));
    }

    IEnumerator Crossfade(AudioClip newClip)
    {
        AudioSource current = usingA ? sourceA : sourceB;
        AudioSource next = usingA ? sourceB : sourceA;

        next.clip = newClip;
        next.volume = 0f;
        next.Play();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float ratio = t / fadeDuration;
            current.volume = Mathf.Lerp(1f, 0f, ratio);
            next.volume = Mathf.Lerp(0f, 1f, ratio);
            yield return null;
        }

        current.Stop();
        usingA = !usingA;
    }
}