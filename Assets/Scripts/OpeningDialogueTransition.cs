using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OpeningDialogueTransition : MonoBehaviour
{
    private int cDialogue = 0;           //must be zero (for build purpose = 12)
    private bool dialogue0Again = false;    // also use for opening weapon door //main value true
    private AudioSource objectAudioSource;
    //private bool pressedOnce = false;     //not needed, Dialogue will play full
    //private bool hidePanelOff;
    //private bool pAppear;

    public Text[] dialogues;
    public GameObject skipButton;
    //public GameObject hidePanel;

    private void Start()
    {
        objectAudioSource = transform.root.gameObject.GetComponent<AudioSource>();
        if(SaveScript.openedOnce == true)
        {
            skipButton.SetActive(true);                         //enable skip button if game has been opened more than once
        }
    }

    private void Update()
    {
        if(cDialogue < 1 && GameManager.Instance.bGSclr.creditEnded)
        {
            //Debug.Log("Dialogue Starts");
            SaveScript.openedOnce = true;

            //To change the rocket oncoming to rocket present audio clip //not needed
            /*
            {
                GameManager.Instance.rCtrlr.GetComponent<AudioSource>().clip = GameManager.Instance.rCtrlr.rocketSounds[1];
                GameManager.Instance.rCtrlr.GetComponent<AudioSource>().Play();
            }
            */

            objectAudioSource.enabled = true;
            ChangeDialogue();
            {
                //Debug.Log(a.GetCurrentAnimatorClipInfo(1)[0].clip.name);
                //Debug.Log(Animator.StringToHash("OpeningDialogue.DialogueAppear_All"));     //1928193914
                //Debug.Log(Animator.StringToHash("OpeningDialogue.OpeningNotEnded"));          //-504601260
                //Debug.Log(Animator.StringToHash("OpeningDialogue"));                          //-1071968500
                //Debug.Log(GameManager.Instance.bGSclr.bs.GetCurrentAnimatorStateInfo(1).fullPathHash); //1928193914
            }
        }
        {
            /*
            if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("OpeningDialogue.DialogueAppear_All")
                && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).normalizedTime >= .5f)
            {
                Debug.Log(dialogues[cDialogue - 1].name);
                //gameObject.GetComponent<Animator>().Play("OpeningDialogue.DialogueAppear_All", 1, .75f);
                //gameObject.GetComponent<Animator>().PlayInFixedTime("OpeningDialogue.DialogueAppear_All", 1, .5f);
                //gameObject.GetComponent<Animator>().playbackTime = 0.5f;
            }
            */
        }
    }

    public void ChangeDialogue()                    //change if state is not playing, else skip to state end
    {
        GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(false);        //turn off next dialogue button
        var tempBgScrAnimator = gameObject.GetComponent<Animator>();

        //Debug.Log(cDialogue + "  "+ dialogues[cDialogue].name);
        if(cDialogue == 9)                         //main game starts
        {
            //Debug.Log("Dialogue 16");

            GameManager.Instance.dialoguePanel.SetActive(false);
            GameManager.Instance.rCtrlr.allAnimationForOpeningEnded = true;  //all entry animation and dialogue finished
            GameManager.Instance.rCtrlr.returnCameraToNormal = true;     // return camera to  normal position
            tempBgScrAnimator.SetBool("StartGameAnim", true);   //start game animation, return camera to normal
            gameObject.GetComponent<UFOSpawner>().totalUFOSpawned = 0;
            GameManager.Instance.rCtrlr.playerHealthSlider.SetActive(true);
            //gameObject.GetComponent<UFOSpawner>().doneOnce = true;         //not neccesary, set true from beginning

            {
            //gameObject.GetComponent<UFOSpawner>().StartSpawningUFO();
            //GameManager.Instance.bGSclr.GetComponent<GameObject>().GetComponent<Camera>().orthographicSize = 5.4f; // not necessary            
            }

            return;                                 //early return
        }

        if ((!GameManager.Instance.rCtrlr.doorStuck) && cDialogue < 9)
        {
            //Old Code, don't need this
            {
                /*
                if (dialogue0Again && cDialogue == 3)                   //dialogue 4th is 1st dialogue so check
                {
                    dialogues[2].gameObject.SetActive(false);    //deactivate 3rd before activating 1st
                    dialogues[0].gameObject.SetActive(true);
                    dialogue0Again = false;
                    Debug.Log("Dialogue 3");
                }
                else
                {
                }
                */
            }

            dialogues[cDialogue++].gameObject.SetActive(true);      //turn on respective dialogue, enabled does not work
            //Debug.Log("Dialogue true : " + cDialogue); 
           
            if (cDialogue != 1)
            {
                //Old Code, Don't need this also
                {
                    /*        //was activating and deactivating at the same time the current object
                    if (cDialogue == 4)
                    { 
                        dialogues[0].gameObject.SetActive(false);      //deactivate after 4th dialogue, repeating 1st dialogue
                    }
                    else
                    {}
                    */
                }
                //Debug.Log(dialogues[cDialogue - 1].name); 

                dialogues[cDialogue - 2].gameObject.SetActive(false); // deactivate previous dialogue, 2 because cDialogue++
                
                tempBgScrAnimator.SetBool("makeExit", true);
            }
            tempBgScrAnimator.SetBool("openingFinish", true);                        //main dialogue starts

            //code to make hide dialogue according to dialogue length 
            //Old Code, don't need this
            {
                /*
                if (!dialogue0Again && cDialogue == 3)  // enable dialogue 0 again, after 3rd dialogue
                {
                    StartCoroutine(WaitFor1Second(0)); // pass 0 for 1st dialogue
                    return;
                }
                */
            }

            //Debug.Log("Dialogue : " + (cDialogue - 1));
            StartCoroutine(WaitForSecond(cDialogue - 1));               // because of cDialogue++
            if (cDialogue == 7)                                         // same here as above
            {
                GameManager.Instance.rCtrlr.doorStuck = true;
                //Debug.Log("Dialogue : " + cDialogue);
                //GameManager.Instance.dialoguePanel.SetActive(false);                           //can't do it here
            }

            { 
                /* //old one
            Transition of dialogue finally works after shit ton of trials and experiments. The exit time is disabled in dialogue appear (second one),
            because the transition was becoming too slow(taking at least 4 seconds to go back to start),but animation event is close to one frame or
            it will cause infinite loop, the first one's exit time cannot be disabled(it loops indefinitely,
            particularly if transition is 0),
            */
            }

            {
                //WaitFor1Second();      //(Incorrect)does above but stops animation for 1 sec and sets infinite loop, WaitFor1Second never enters
                //StartCoroutine(WaitFor1Second());   //correct way
                //can't do this, immdediately turns off after turning on
                /*
                GameManager.Instance.bGSclr.bs.SetBool("openingFinish", false);
                GameManager.Instance.bGSclr.bs.SetBool("makeExit", false);
                */

                //pAppear = true;
            }
        }
        else    //start rocket weapon door animation
        {
            //Debug.Log("Prepare" + dialogue0Again);
            if (!dialogue0Again)
            {
                //Debug.Log("to");
                GameManager.Instance.dialoguePanel.SetActive(false);            //deactivate the dialogue panel after 12, before WDoor stuck
                dialogues[cDialogue - 1].gameObject.SetActive(false);              //deactivate the last text(dialogue 12)
                GameManager.Instance.rCtrlr.rcAnimator.SetBool("OpenWeaponDoorLeft", true);
                dialogue0Again = true;
                StartCoroutine(WaitForSecond(-1));
                {
                    //didn't fix immediate appearence of canon
                    //GameManager.Instance.rCtrlr.disappear.SetActive(true);   //parent is inactive, set parent to active. Immediately turns active, cannon appears on screen
                    //Debug.Log("Ahoy End");
                }
            }
        }
    }

    public void HidePanelReset()
    {
        var tempBgScrAnimator = gameObject.GetComponent<Animator>();
        //Debug.Log("hidePanel");
        tempBgScrAnimator.SetBool("openingFinish", false);
        tempBgScrAnimator.SetBool("makeExit", false);

        // Pain in the A method(abandoned)
        {
            /*
            if (x < 4)
            {
                hidePanel.transform.GetChild(x).gameObject.SetActive(true);      //turn on individual panel
                if (x == 4)
                    hidePanel.transform.GetChild(x).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);   //set sizeDelta (width, length)
                hidePanelOff = true;                                             //turn off if all panel has activated
            }
            */
        }
    }

    public void ButtonPressSkip()
    {
        // can't figure out how to allow only 2 clicks of next Dialogue button, so commented out code, instead Dialogue plays full
        {
            //pressedOnce++;
            /*
            var tempBgScrAnimator = gameObject.GetComponent<Animator>();
            if (tempBgScrAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
            {
                {
                    //Debug.Log("Inside");
                    //not useful, produces UnityEngine.animatorStateInfo not the name of the state
                    // var b = a.GetCurrentAnimatorStateInfo(1).ToString();   // was producing warning if directly used in function below that no state found
                }
                StopPlayingAudio();
                tempBgScrAnimator.Play("OpeningDialogue.DialogueAppear_All", 1, 1);
                pressedOnce = true;
            }
            else
            {
            }
            */
        }
        ChangeDialogue();
        //pressedOnce = false; 
    }
    
    IEnumerator WaitForSecond(int n)
    {
        var tempBgScrAnimator = gameObject.GetComponent<Animator>();

        switch (n)
        {
            case -1:
                yield return new WaitForSeconds(.5f);
                GameManager.Instance.rCtrlr.disappear.SetActive(true);      //enable Left Cannon 
                break;
            case 32:           // no case for first line
                yield return new WaitForSeconds(1.5f);                      // display first line of dialogue
                //if (!pressedOnce)
                {
                    StopPlayingAudio();
                    tempBgScrAnimator.Play("OpeningDialogue.DialogueAppear_All", 1, 1);
                    GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
                }

                break;
            case 8:
                yield return new WaitForSeconds(3f);                       //display 2nd line of dialogue
                //if (!pressedOnce)
                {
                    StopPlayingAudio();
                    tempBgScrAnimator.Play("OpeningDialogue.DialogueAppear_All", 1, 1);
                    GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
                }

                break;
            case 0:
            case 1:
            case 7:
                yield return new WaitForSeconds(4.5f);                      //display 3rd line of dialogue
                //if (!pressedOnce)
                {
                    StopPlayingAudio();
                    tempBgScrAnimator.Play("OpeningDialogue.DialogueAppear_All", 1, 1);
                    GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
                }

                break;
            case 2:
            case 3:
            case 4:
            case 6:
                yield return new WaitForSeconds(1.5f);                     // display 1st line of dialogue
                //if (!pressedOnce)
                {
                    StopPlayingAudio();
                    tempBgScrAnimator.Play("OpeningDialogue.DialogueAppear_All", 1, 0.5f);
                    PlayDialogueAudio();
                    yield return new WaitForSeconds(1.5f);                       // display 3rd line of dialogue
                    StopPlayingAudio();
                    tempBgScrAnimator.Play("OpeningDialogue.DialogueAppear_All", 1, 1);
                    GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
                }
                break;
            case 5:
                yield return new WaitForSeconds(6f);
                //if (!pressedOnce)
                { 
                    GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
                }

                break;
            case -2:
                yield return new WaitForSeconds(0.3f);
                GameManager.Instance.rCtrlr.rcAnimator.Play("Open Weapon Door.WDAppearRightStuck", 0, 1);
                GameManager.Instance.rCtrlr.MakeRocketPartsDisappear(3);
                GameManager.Instance.rCtrlr.disappear.SetActive(true);      //enable Left Cannon 
                dialogues[cDialogue - 1].gameObject.SetActive(false);
                cDialogue = 8;
                ChangeDialogue();
                break;
        }
        //HidePanelReset();
    }

    public void skipAll()                           //skips to last dialogue
    {
        transform.GetComponent<Animator>().Play("OpeningCredit.SkipAll", 0, 0);
        StartCoroutine(WaitForSecond(-2));          // wait for some seconds before skipping so everything is covered
    }

    public void PlayDialogueAudio()
    {
        objectAudioSource.Play();
    }

    public void StopPlayingAudio()
    {
        objectAudioSource.Stop();
    }

    /*
    public void StopAnim()                 //try to end anim after dialogue over
    {
        //gameObject.GetComponent<Animator>().Play("OpeningDialogue.DialogueAppear_All", 1, .5f);
        //gameObject.GetComponent<Animator>().speed = 0f;
    }
    */
}