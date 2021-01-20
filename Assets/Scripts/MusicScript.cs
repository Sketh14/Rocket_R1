using System.Collections;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    //private bool inNextScene; 
    private int tempIndex = 0;

    public AudioSource musicSource;
    public AudioClip[] musicClips; 
     
    private static MusicScript m_instance;
    public static MusicScript InstanceMusic
    {
        get
        {
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject); 
        }
        else
        {
            m_instance = this;
        } 

        DontDestroyOnLoad(gameObject); 

        //m_instance = this;
        //musicSource = gameObject.GetComponent<AudioSource>();

    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    void Update()
    {
        {
            /*
            if(inNextScene)
            {
                gameObject = GameManager.Instance.musicController;
                inNextScene = false;
            }
        if(GameManager.Instance.musicController)
        {
            MusicToggleOn_Off();
        }

        if(GameManager.Instance.soundEffectController)
        {
            SoundToggleOn_Off();
        }
            */
        }
    } 

    public void AdjustMusicLevel(int sceneIndex)
    {
        if(sceneIndex == 1)
        {
            musicSource.volume = 0.1f;
        }
        else
        {
            musicSource.volume = 0.3f;
        }
    }

    public void MusicToggleOn_Off()
    { 
        if (!musicSource.enabled)
        { 
            musicSource.enabled = true;
        }
        else
        { 
            musicSource.enabled = false;
        } 
    }

    public void SoundToggleOn_Off()
    {
        if (GameManager.Instance.toggleSound)
        {
            GameManager.Instance.toggleSound = false;
        }
        else
        {
            GameManager.Instance.toggleSound = true;
        }
    }

    private void PlayBackgroundMusic()
    {
        musicSource.Play();
        StartCoroutine(WaitAround());
    }

    IEnumerator WaitAround()
    {
        //for primary purpose
        yield return new WaitForSeconds(musicSource.clip.length);
        if (tempIndex == 1)                       //adjust for how many clips will be in there
        {
            tempIndex = -1;                      //as increment will always increase to 1, if not set to -1
        }
        musicSource.clip = musicClips[++tempIndex];
        PlayBackgroundMusic();

        /*
        else                    //to check after regular intervals
        {
            yield return new WaitForSeconds(120f);
        }
        */
    }
}
