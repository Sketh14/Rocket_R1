using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //private Animator anim;
    private bool changeBckgrnd = true;
    private int counterImageBckgrnd;
    private bool creditOpen; 

    public Sprite[] mainImages;
    public Sprite[] bckgrndImages;
    public Image bckgrndMainChange;
    public Image bckgrndChange;
    public Text loadedHighScore;
    public GameObject creditContainer;
    public Toggle[] toggleButtonM_SE;
    //public Slider ProgressSlider;
    //public Text ProgressText;
    //private GameObject loadingScreen;
    //private Image readyToLoad;

    private void Start()
    {
        //mScript = GameObject.Find("Bckgrnd Music").GetComponent<MusicScript>(); 
        int n = SaveScript.LoadHighScore();
        loadedHighScore.text = n.ToString();
        if(!LoadManager.InstanceLoad.toggleM_SE[1])
        {
            toggleButtonM_SE[1].isOn = true;
        }

        if (!LoadManager.InstanceLoad.toggleM_SE[0])
        {
            toggleButtonM_SE[0].isOn = true;
            MusicScript.InstanceMusic.musicSource.enabled = false;
        }
    }

    void Update()
    {
         
        if (changeBckgrnd)
        {
            StartCoroutine(TransitionBackground());
        }

        if(creditOpen)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Back");
                gameObject.transform.GetChild(4).gameObject.SetActive(false);
                creditOpen = false;
            }
        }
        if (creditContainer.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log("Pressed"); 
                creditContainer.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        changeBckgrnd = false;
        StartCoroutine(LoadManager.InstanceLoad.LoadScene(1));
        //StartCoroutine(LoadNextScene());
        //Debug.Log("Clicked Start");
        //SceneManager.LoadScene(1); 
    }

    /*
    public void MusicToggle()
    {
        {      //doesnrt work
            if (GameManager.Instance.toggleMusic == false)
            {
                Debug.Log("Turn On MainMenu");
                GameManager.Instance.toggleMusic = true;
            }
            Debug.Log("Turn Off MainMenu");
            GameManager.Instance.toggleMusic = false;
        }
    }
    */

    /*
    public void SoundToggle()
    {
        {
            if (GameManager.Instance.toggleSound == false)
            {
                GameManager.Instance.toggleSound = true;
            }
            else
            {
                GameManager.Instance.toggleSound = false; 
            } 
        }
    }
    */

    public void ExitGame()           //quit from game
    {
        Application.Quit();
    }

    //using LoadManager instead
    /*
    private IEnumerator LoadNextScene()          //loading progress bar
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(++y);

        loadingScreen.SetActive(true);

        loadingOperation.allowSceneActivation = false;
        while (!loadingOperation.allowSceneActivation)
        {
            float progressIL = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            //Debug.Log(progressIL);
            //ProgressSlider.value = progressIL;
            //ProgressText.text = (progressIL * 100).ToString("F0") + "%";

            yield return null;

            if (progressIL >= 1)                  //if loading done, then wait for ready to go green.
            {
                yield return new WaitForSeconds(3f);         // wait some seconds to turn green, loading happens fast
                gameObject.GetComponent<Animator>().SetBool("exitLoading", true);
                Color newColor = new Color(0f, 1f, 0f, 1f);
                readyToLoad.color = newColor;
                yield return new WaitForSeconds(2f);         // ready is green, wait then load the game
                loadingOperation.allowSceneActivation = true;
            }
        }
    }
    */

    private IEnumerator TransitionBackground()
    {
        changeBckgrnd = false;
        gameObject.GetComponent<Animator>().SetBool("appearBckgrndAnim", false);
        gameObject.GetComponent<Animator>().SetBool("fadeBckgrndAnim", true);        //activate animation to zero out alpha
        bckgrndMainChange.sprite = mainImages[Random.Range(0, 3)];
        //gameObject.GetComponent<Animator>().SetBool("resetAnim", false);
        yield return new WaitForSeconds(4f);

        //cycle images by doing counter++
        bckgrndChange.sprite = bckgrndImages[counterImageBckgrnd++];         // set the bckgrnd image on 2nd, doesn't have to be random

        gameObject.GetComponent<Animator>().SetBool("fadeBckgrndAnim", false);
        gameObject.GetComponent<Animator>().SetBool("appearBckgrndAnim", true);     //new bckgrnd appears
        if (counterImageBckgrnd == 3) 
        {
            counterImageBckgrnd = 0;
        }

        yield return new WaitForSeconds(5f);
        changeBckgrnd = true;
    }

    public void AMusicToggle()
    { 
        MusicScript.InstanceMusic.MusicToggleOn_Off();
        //GameObject.Find("Bckgrnd Music").GetComponent<MusicScript>().MusicToggleOn_Off();
    }

    public void ASoundToggle()
    {
        if (LoadManager.InstanceLoad.toggleM_SE[1])
        { 
            LoadManager.InstanceLoad.toggleM_SE[1] = false;
        }
        else
        { 
            LoadManager.InstanceLoad.toggleM_SE[1] = true;
        }
        //LoadManager.InstanceLoad.soundOff = true;
    }

    /*
    public void CreditOpen()
    {
        creditOpen = true;
    }
    */
}
