using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip click, win, lose, time;

    public static SoundPlayer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void PlayAtkSound(Attack attack)
    {
        Instance.audioSource.PlayOneShot(attack.sfx);
    }

    public static void PlayClick()
    {
        Instance.audioSource.PlayOneShot(Instance.click);
    }
    public static void PlayWin()
    {
        Instance.audioSource.PlayOneShot(Instance.win);
    }
    public static void PlayLose() 
    {
        Instance.audioSource.PlayOneShot(Instance.lose, 0.5f);    
    }

    public static void PlayTime()
    {
        Instance.audioSource.PlayOneShot(Instance.time);
    }
}