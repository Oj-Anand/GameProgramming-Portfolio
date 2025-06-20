using UnityEngine;

public class SoundHub : MonoBehaviour
{
    private AudioSource _coinSound;
    void Awake()
    {
        _coinSound = GetComponent<AudioSource>();
    }
    public void PlayCoinSound()
    {
        _coinSound.Play();
    }
}
