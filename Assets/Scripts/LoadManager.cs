using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class LoadManager : MonoBehaviour
{
    //private static GameObject instance;
    //public MusicScript mScript;

    public GameObject loadingScreen;
    public Image readyToLoad;
    public bool[] toggleM_SE;

    private static LoadManager l_instance;
    public static LoadManager InstanceLoad
    {
        get
        {
            {
            /*
            if (m_instance == null)
            {
                Debug.Log("LoadManager Created");
                GameObject obj = new GameObject("LoadManager");
                obj.AddComponent<LoadManager>();
            }
            */
            }
            return l_instance;
        }
    }

    private void Awake()
    {
        if (l_instance != null && l_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            l_instance = this;
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(720, 1280, true);
        // doesn't work as don't destroy works on root.
        //transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).transform;
    }
     
    //public void LoadScene(int tempIndex)
    //{

    //} 

    public IEnumerator LoadScene(int tempIndex)          //loading progress bar
    {
        //int y = SceneManager.GetActiveScene().buildIndex;
        //mScript.StartMusic(tempIndex);
        //GameObject.Find("Bckgrnd Music").GetComponent<MusicScript>().StartMusic(tempIndex);
        MusicScript.InstanceMusic.AdjustMusicLevel(tempIndex);
        toggleM_SE[0] = MusicScript.InstanceMusic.musicSource.enabled ? true : false;
        if (tempIndex == 0)             //go back to true from level 1 to level 0, when lvl 0 will be loaded
        {
            toggleM_SE[1] = GameManager.Instance.toggleSound ? true : false;
            {
                /*
                if (!GameManager.Instance.toggleSound)
                {
                    toggleM_SE[1] = false;
                }
                else
                {
                    toggleM_SE[1] = true;
                }
                */
            }
        }

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(tempIndex);

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        if(tempIndex == 0)
        {
            loadingScreen.transform.GetChild(0).gameObject.SetActive(false);
            loadingScreen.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            loadingScreen.transform.GetChild(0).gameObject.SetActive(true);
            loadingScreen.transform.GetChild(1).gameObject.SetActive(false); 
        }

        loadingOperation.allowSceneActivation = false;
        while (!loadingOperation.allowSceneActivation)
        {
            float progressIL = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            {
                //Debug.Log(progressIL);
                //ProgressSlider.value = progressIL;
                //ProgressText.text = (progressIL * 100).ToString("F0") + "%";
            }
            
            yield return null;

            if (progressIL >= 1)                  //if loading done, then wait for ready to go green.
            {
                gameObject.GetComponent<Animator>().SetBool("reStart", false);               //set to true from previous

                yield return new WaitForSeconds(3f);         // wait some seconds to turn green, loading happens fast
                gameObject.GetComponent<Animator>().SetBool("exitLoading", true); 
                readyToLoad.color = new Color(0f, 1f, 0f, 1f);
                yield return new WaitForSeconds(2f);         // ready is green, wait then load the game
                loadingOperation.allowSceneActivation = true;
                // sound toggle can go here maybe.
                yield return new WaitForEndOfFrame();

                loadingScreen.SetActive(false);                     //wait for next scene to fully load and then deactivate
                readyToLoad.color = new Color(1f, 0f, 0f, 1f);      //change ready colour back to red.
                gameObject.GetComponent<Animator>().SetBool("exitLoading", false);            //set to false so it does not loop
                gameObject.GetComponent<Animator>().SetBool("reStart", true);                 //start over
            }
        }
    }
}
