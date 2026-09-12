using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip musicClip;

    void Start()
    {
        AudioManager.Instance.CrossfadeTo(musicClip);
    }
}