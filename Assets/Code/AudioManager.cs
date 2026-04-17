using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioSource effectAudio;

    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;

    [SerializeField] private AudioClip attackClip1;
    [SerializeField] private AudioClip attackClip2;
    [SerializeField] private AudioClip attackClip3;
    [SerializeField] private AudioClip defendClip;

    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip deathClip;

    [SerializeField] private AudioClip enemyattackClip;
    [SerializeField] private AudioClip enemydeathClip;

    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    void Start()
    {
        PlayBackGroundMusic();
    }

    public void PlayBackGroundMusic()
    {
        backgroundAudio.clip = backgroundClip;
        backgroundAudio.Play();
    }

    public void PlayJumpSound()
    {
        effectAudio.PlayOneShot(jumpClip);
    }

    public void PlayCoinSound()
    {
        effectAudio.PlayOneShot(coinClip);
    }

    public void PlayAttackOneSound()
    {
        effectAudio.PlayOneShot(attackClip1);
    }

    public void PlayAttackTwoSound()
    {
        effectAudio.PlayOneShot(attackClip2);
    }

    public void PlayAttackThreeSound()
    {
        effectAudio.PlayOneShot(attackClip3);
    }

    public void PlayDefendSound()
    {
        effectAudio.PlayOneShot(defendClip);
    }

    public void PlayDeathSound()
    {
        effectAudio.PlayOneShot(deathClip);
    }

    public void PlayHurtSound()
    {
        effectAudio.PlayOneShot(hurtClip);
    }

    public void PlayEnemyAttackSound()
    {
        effectAudio.PlayOneShot(enemyattackClip);
    }

    public void PlayEnemyDeathSound()
    {
        effectAudio.PlayOneShot(enemydeathClip);
    }

    public void PlayWinSound()
    {
        backgroundAudio.clip = winClip;
        backgroundAudio.Play();
    }

    public void PlayLoseSound()
    {
        backgroundAudio.clip = loseClip;
        backgroundAudio.Play();
    }

    public void PauseBackgroundMusic()
    {
        backgroundAudio.Pause();
    }

    public void UnPauseBackgroundMusic()
    {
        backgroundAudio.UnPause();
    }
}
