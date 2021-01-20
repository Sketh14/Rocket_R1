//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScroller : MonoBehaviour
{
    private bool openingCreditEnd;
    private float lowerCameraByY = 3.5f;
    //private bool gamePaused;
    private int tempBckgrnd;

    public Transform spawnPointBackGround;
    public Transform rocketMain;
    public bool creditEnded;
    public GameObject scoreContainer;
    public GameObject[] pauseLogic;
    public GameObject[] bckgrndToMove;
    public GameObject resumeButton;                             // for gameOver, to remove Resume and replace with GameOver
    public GameObject gameOverContainer;                        //turn off for GameOver
    public Toggle[] toggleButtonM_SE;

    /*
    //public Animator bs;
    public float yValueForRocket;
    public float yValueForRayCastDelete;
    public GameObject backGround1, backGround2;
    */

    /*
    private void Awake()
    {
        bs = GetComponent<Animator>();
    }
    */

    private void Awake()
    {
        if(!LoadManager.InstanceLoad.toggleM_SE[1])
        {
            toggleButtonM_SE[1].isOn = true;
            GameManager.Instance.toggleSound = false;
        }
        if(!LoadManager.InstanceLoad.toggleM_SE[0])
        {
            toggleButtonM_SE[0].isOn = true;
            //MusicScript.InstanceMusic.musicSource.enabled = false;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log("Pressed");
                TogglePause();
            }
        }
    }

    void FixedUpdate()
    {
        {//bckgrndToMove.transform.Translate(0.0f, 0.1f, 0.0f);
            foreach (var bckgrnd in bckgrndToMove)
            {
                bckgrnd.transform.Translate(0.0f, -0.1f, 0.0f);
            }
        }

        if (openingCreditEnd)
        {
            {
            //if (!bs.GetCurrentAnimatorStateInfo(0).IsName("OpeningCredit"))                          //check if opening credit animation has ended
                //Debug.Log("In IF GetAnim");
            }
            if (transform.position.y >= rocketMain.position.y + 1.0f)                                 //lower position until camera is equal to rocket
            {
                {
                //transform.Translate(0f, rocketMain.position.y - .00000005f, 0f);                    // failed attempt( subtract from rocket position), also too slow
                //transform.Translate(0f, -0.03f, 0f);                                              // rocket leaves camera behind(fixed by instance) // camera appears to drop down                   
                }
                transform.position = new Vector3(0f, rocketMain.position.y + lowerCameraByY, -10.0f);
                lowerCameraByY -= 0.03f;
            }
            
            //didn't work as expected
            /*
            else if(GameManager.Instance.rCtrlr.allAnimationForOpeningEnded)
            {
                openingCreditEnd = false;
            }
            */

            else
            {
                transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                creditEnded = true;
                openingCreditEnd = false;
                {
                //transform.gameObject.GetComponent<Camera>().orthographicSize = 5.4f;    //does nothing
                //bs.SetBool("openingFinish", true);                        //(old) main dialogue starts
                }
            }
        }
        else
        {
            //Debug.Log("Out of IF GetAnim");
            
            if(creditEnded && !GameManager.Instance.rCtrlr.allAnimationForOpeningEnded)      //should be not instance
            {
                transform.position = new Vector3(0f, rocketMain.position.y + 1f, -10f);     //transition to dialogue
            }
            
            //will have to transition back to normal mode
            else if(GameManager.Instance.rCtrlr.returnCameraToNormal)     // transition back to normal
            {
                if(transform.position.y <= rocketMain.position.y + 3.5f)
                {
                    transform.position = new Vector3(0f, rocketMain.position.y + lowerCameraByY, -10.0f);
                    lowerCameraByY += 0.03f;
                    //Debug.Log("Returning to normal");
                }
                else
                {
                    scoreContainer.SetActive(true);
                    pauseLogic[0].SetActive(true);
                    GameManager.Instance.rCtrlr.returnCameraToNormal = false;
                }
            }

            else     // main game starts
            {
                //Debug.Log("Not normal");
                transform.position = new Vector3(0f, rocketMain.position.y + 3.5f, -10f);  //main game
            }
        }

        DetectBackGroundChange();
        //DestroyPassedBackGround();     //main , (OB) no need for this now.
    }

    void ChangeBackGround(GameObject g, int value)
    {
        g.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.backgroundSprites[Random.Range(value,3)];
    }

    void DetectBackGroundChange()
    {
        //Vector3 spawnPointBackgroundVector = spawnPointBackGround.transform.position;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.0f, 6.0f, 10.5f), transform.forward, 2);
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(0.0f, 5.9f, 10.5f), transform.forward, 2);        //camera doesnt move
        //Debug.DrawRay(transform.position + new Vector3(0.0f, 5.9f, 10.5f), transform.forward, Color.red);
        if (hit.transform)
        {
            if (hit.transform.CompareTag("changeBackground"))
            {
                {
                //Debug.Log("Hit Once - Detect");
                //Debug.Log(hit.transform.root.transform.name); 
                }

                //move the bckgrnd below to up
                bckgrndToMove[++tempBckgrnd].transform.position = spawnPointBackGround.position;

                {
                //main Old
                //var cloneBackGround = Instantiate(GameManager.Instance.mainBackGround, spawnPointBackGround.position, Quaternion.identity);
                //ChangeBackGround(cloneBackGround, Random.Range(0, 3));
                }

                {
                //spawnPointBackGround.Translate(0, 12.96f, 0);              //old value 10.8,because bckgrnd size increase to 1.2
                //changeBackGround(hit.transform.root.transform.gameObject, Random.Range(0,3));
                }

                ChangeBackGround(bckgrndToMove[tempBckgrnd], Random.Range(0, 3));
                bckgrndToMove[tempBckgrnd].transform.GetChild(0).gameObject.SetActive(true);

               /*
                *does not check for 2 because increment cant go to 2 before position line, i.e. increment goes to 2
                *and tempBckgrnd is repositioned, but 2 does not exist so error. If its placed before, then cannot
                *check if it reaches 2, because check happens before increment.
                */
                if (tempBckgrnd == 1)
                {
                    //Debug.Log("Reached 1");
                    tempBckgrnd = -1;            // -1 because increment will convert 0 to 1 always, but -1 to 0.
                }

                hit.transform.gameObject.SetActive(false);    //the bckgrnd disappears before reaching second one
            }
        }
    }

    /*
    void DestroyPassedBackGround()               // is not needed now
    {
        //Vector3 spawnPointBackgroundVector = spawnPointBackGround.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.0f, -17f, 10.5f), transform.forward, 1);
        Debug.DrawRay(transform.position + new Vector3(0.0f, -17f, 10.5f), transform.forward, Color.red);
        if (hit.transform)
        {
            if (hit.transform.CompareTag("destroyBackground"))
            {
                //Debug.Log(hit.transform.root.transform.name);

                Destroy(hit.transform.root.transform.gameObject);
            }
        }
    }
    */

    void OpeningCreditEndSwitch()   //used as animation event
    {
        openingCreditEnd = true;
    }

    public void TogglePause()
    {
        // gamePaused = true;
        if (!pauseLogic[0].activeSelf && !gameOverContainer.activeSelf)
        {
            pauseLogic[0].SetActive(true);
            Time.timeScale = 1f;
            pauseLogic[1].SetActive(false);
        }
        else
        {
            pauseLogic[0].SetActive(false);
            Time.timeScale = 0f;
            pauseLogic[1].SetActive(true);
        }
    }

    public void ToggleMusicOn_Off()
    {
        MusicScript.InstanceMusic.MusicToggleOn_Off();
        //GameObject.Find("Bckgrnd Music").GetComponent<MusicScript>().MusicToggleOn_Off();
    } 

    public void ToggleSoundOn_Off()
    {
        MusicScript.InstanceMusic.SoundToggleOn_Off();
        //GameObject.Find("Bckgrnd Music").GetComponent<MusicScript>().SoundToggleOn_Off();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;            // reset the time Scale as the menu will be paused
        if (GameManager.Instance.rCtrlr.healthOfPlayer > 0)               //disable pollygon collider as game goes on in the bckgrnd
        {
            GameManager.Instance.rCtrlr.transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = false; 
        }
        //SceneManager.LoadScene(0);
        StartCoroutine(LoadManager.InstanceLoad.LoadScene(0));
        //SceneManager.UnloadSceneAsync(1);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
    
    public void GameOver()
    {
        Debug.Log("Displaying Game Over");
        resumeButton.SetActive(false);
        gameOverContainer.SetActive(true);
        gameOverContainer.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.highScoreContainer.text;
        //resumeButton.GetComponent<Image>().enabled = false;
        //resumeButton.GetComponent<Button>().enabled = false;
        TogglePause();
    }
}
