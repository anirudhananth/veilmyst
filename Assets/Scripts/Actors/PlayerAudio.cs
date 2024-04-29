using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    public AudioClip Jump;
    public AudioClip[] Walk;
    public AudioClip Dash;
    public AudioClip Death;

    private AudioSource audioSource;
    private bool walking = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWalk()
    {
        if (audioSource.isPlaying && !walking) return;
        if (audioSource.isPlaying && audioSource.time < 0.3) return;
        audioSource.Stop();
        int index = Random.Range(0, Walk.Length);
        audioSource.clip = Walk[index];
        audioSource.Play();
        walking = true;
    }

    public void PlayJump()
    {
        walking = false;
        audioSource.Stop();
        audioSource.PlayOneShot(Jump);
    }

    public void PlayDash()
    {
        walking = false;
        audioSource.Stop();
        audioSource.PlayOneShot(Dash);
    }

    public void PlayDeath()
    {
        walking = false;
        audioSource.Stop();
        audioSource.PlayOneShot(Death);
    }
}