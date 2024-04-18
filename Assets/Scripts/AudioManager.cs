using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    [SerializeField]
    private AudioSource musicSourse;
    [SerializeField]
    private AudioSource uiSourse;

    [Header("Audioclips")]
    public AudioClip backgroundMenu;
    public AudioClip backgroundLevel;
    public AudioClip findEffect;
    public AudioClip endGame;
    public AudioClip loseGame;
    public AudioClip buttonClick;

    private void OnEnable()
    {
        GlobalEventManager.DifferenceFound.AddListener(() => PlaySFX(findEffect));
        GlobalEventManager.EndGame.AddListener(() => PlaySFX(endGame));
        GlobalEventManager.LoseGame.AddListener(() => PlaySFX(loseGame));
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        musicSourse.clip = backgroundMenu;
        SwitchMusic(true);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSourse.clip = clip;
        musicSourse.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        uiSourse.PlayOneShot(clip);
    }
    
    public void SwitchMusic(bool mode)
    {
        if(mode)
            musicSourse.Play();
        else
            musicSourse.Stop();
    }
}
