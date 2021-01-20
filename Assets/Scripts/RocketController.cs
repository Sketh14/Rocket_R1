using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class RocketController : MonoBehaviour
{
    /*
    private Vector2 direction;
    private Vector3 position;
    private float width, height;
    */ 
    private bool rWDOpen;
    private GameObject ammoPlayer;
    private int shootCounter; 
    
    public int healthOfPlayer = 100;
    public int fireRatePlayer;
    public GameObject[] disAppear;
    public GameObject disappear;                    //left canon
    public Animator rcAnimator;
    public bool allAnimationForOpeningEnded;        //all opening animation has ended
    public bool returnCameraToNormal;
    public bool doorStuck;
    public Transform[] spawnAmmoAtPlayer;
    public AudioClip[] rocketSounds;
    public GameObject playerHealthSlider;
    //private float speedModifier = 2.0f;
    //private bool withinScreenBoundsV;

    /*
    void Awake()
    {
        {
            width = (float)Screen.width / 2.0f;
            Debug.Log(width);
            height = (float)Screen.height / 2.0f;
            Debug.Log(height);
        }
    }
    */

    private void Start()
    {
        ammoPlayer = GameManager.Instance.ammoContainer[0]; 
    }

    /*                           //continues to work in timeScale =0, so moved code to FixedUpdate.
    void Update()
    {
        if (HasTouchedScreen())
        {
            MoveShipToFinger();
        }

        if (GameManager.Instance.rCtrlr.allAnimationForOpeningEnded)
        {
            if (shootCounter % 80 == 0)       //shoot after every 80th frame (assuming)(worked)
            {
                ShootPlayer();
                //Debug.Log(shootCounter);

                {   //no need
                    
                    //if (shootCounter == 1640)
                    //    shootCounter = 0;              // only at start , doesnt work 
                    
                }

                if (shootCounter == 1600)
                {
                    //Debug.Log("Reached");
                    shootCounter = 0;
                }
            }
            ++shootCounter;                // increment counter
        }

        //shootPlayer();
    }
    */

    private void FixedUpdate()
    {
        {
            /*
        if (Time.timeScale == 0f)
        {
            Debug.Log("Time = 0");
        }
        */
        } 

        if (HasTouchedScreen())
        {
            MoveShipToFinger();
        }

        {
            //withinScreenBoundsV = withinScreenBounds();

            //move ship
            //probelm with spawning UFO
            //transform.Translate(0.0f, 0.1f, 0.0f); //doesn't work if animator is on in the same object //#1Main
        }

        if (GameManager.Instance.rCtrlr.allAnimationForOpeningEnded)
        {
            if (shootCounter % fireRatePlayer == 0)       //shoot after every 80th frame (assuming)(worked)
            {
                ShootPlayer();
                //Debug.Log(shootCounter);

                if (shootCounter == 1400)
                {
                    //Debug.Log("Reached");
                    shootCounter = 0;
                }
            }
            ++shootCounter;                // increment counter
        }

        else
        {
            if (doorStuck
                && rcAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open Weapon Door.WDAppearLeft")
                && rcAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                //Debug.Log("After WDAppearLeft"); 
                rcAnimator.SetBool("OpenWeaponDoorRight", true);
                rcAnimator.SetBool("OpenWeaponDoorLeft", false);
                StartCoroutine(Waitfor1second(1));
                //rWDOpen = false;

                MakeRocketPartsDisappear(3);               //called too many times
            }
            else if (doorStuck                             //(Wrong and updated)we already know that WDAppearLeft has closed, so directly check if animation has finished
                && rcAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open Weapon Door.WDAppearRightStuck")
                && rcAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                //Debug.Log("After WDAppearRight"); 
                StartCoroutine(Waitfor1second(3));     //activate dialogue for door stuck
            }
            //(Fixed)doesnt work as intended , the 3 images still there untill else if is reached so they blink after whole anim plays

            { //replaced by coroutine
                /*
                else if (rcAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open Weapon Door.WDAppearRightStuck" ) && doorStuck) //!rWDOpen)
                {
                    rcAnimator.SetBool("OpenWeaponDoorRight", false);
                }
                */
            }

            {  // testing the LeftWD mechanism
                /*
                if (rcAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open Weapon Door.WDAppearLeftEnd"))
                {
                    disAppear[0].SetActive(false);
                    disAppear[1].SetActive(false);
                    disAppear[2].SetActive(false);
                    {
                        //disAppear[3].SetActive(false);      //will have to define explicitly
                        //disappear.SetActive(false);         //doesn't work   //will work with parent
                        //disappear.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);   //not work with child if it has anim with color prprty
                    }
                    Debug.Log("In IF  " + disappear.name + "  " + disappear.activeSelf);
                }
                //Debug.Log(disAppear[3].name + "  " + disAppear[3].activeInHierarchy);
                */
            }

        }

        //transform.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2f)); //addforce doesnt work with kinematic bodies

        //for disabling Audio Source if Sounds are turned off
        {
            if(!GameManager.Instance.toggleSound)
            {
                gameObject.GetComponent<AudioSource>().enabled = false;
            }
            else
            { 
                gameObject.GetComponent<AudioSource>().enabled = true;
            }
        }
    }

    bool HasTouchedScreen()
    {
        if(Input.touchCount > 0)
        {
            //Debug.Log("Screen Touhced");
            return true; 
        }

        return false;
    }
    
    void MoveShipToFinger()
    {
        Touch touch1 = Input.GetTouch(0);

        Vector3 startPos = Camera.main.ScreenToWorldPoint(touch1.position);

        if (touch1.phase == TouchPhase.Moved)
        {
            //Debug.Log("In IF");

            //startPos.z = startPos.y = 0.0f; 
            transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
        }
    }

    public void MakeRocketPartsDisappear(int countTill)   
    {
        int tempPartsDisabled = 0; 
        foreach (var element in disAppear)
        { 
            if (tempPartsDisabled < countTill )
            {
                {
                    /*
                    if (element.name == "RS-WD-right")
                    {
                        rWDOpen = true;
                        break;
                    }
                    */
                }

                element.SetActive(false);
                //Debug.Log(element.name + "  " + element.activeInHierarchy + "Set False");
            }
            ++tempPartsDisabled;
        }
        {

        /*
        int countTill = (tempPartsDisabled == 0) ? 3 : 6;
        Debug.Log(countTill + "  :  " + tempPartsDisabled);
        for (int i = tempPartsDisabled; i < countTill; i++)
        {
            //if (tempPartsDisabled < 3)
            {
                {  
                //if (element.name == "RS-WD-right")
                {
                    //rWDOpen = true;
                    //break;
                }
                }

                disAppear[i].SetActive(false);
                //Debug.Log(disAppear[i].name + "  " + disAppear[i].activeInHierarchy + "Set False");
            }
            //++tempPartsDisabled; 
        }
        */

            /*
        { //didn't work,(O.I.)activate the deactivated canon or parent
            else if(onlyLeftWD == 3)
            {
                element.SetActive(true);
                Debug.Log(element.name + "  " + element.activeInHierarchy + "Set True");
            }
            ++onlyLeftWD;
        }
            */
        }
    }

    public void WeaponRFree()     //and for this
    {
        Debug.Log("Right Weapon Free");
        rcAnimator.SetBool("RightWeaponFree", true);
        rWDOpen = true; 
        MakeRocketPartsDisappear(6);
    }

    IEnumerator Waitfor1second(int n)
    {
        yield return new WaitForSeconds(.5f);
        if(n == 1)
        {
            rcAnimator.SetBool("OpenWeaponDoorRight", false);
        }
        else if(n == 2)                //Game Over
        {
            yield return new WaitForSeconds(2.5f);
            gameObject.SetActive(false);
            GameManager.Instance.ufoSpwn.GameOver();
            GameManager.Instance.bGSclr.GameOver();
        }
        else
        {
            GameManager.Instance.dialoguePanel.SetActive(true);
            doorStuck = false;
            GameManager.Instance.dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    /*
    IEnumerator WaitFor()
    {
        yield return new WaitForSeconds(1.0f);
    }
    */

    public void ShootPlayer()
    {
        PlayAudioForPlayer(0);
        Instantiate(ammoPlayer, spawnAmmoAtPlayer[0].position, Quaternion.identity);
        if (rWDOpen)                                // shoot right canon
        {
            PlayAudioForPlayer(0);
            Instantiate(ammoPlayer, spawnAmmoAtPlayer[1].position, Quaternion.identity);
        }
    }

    public void TakeDamagePlayer(int damageTaken)
    {
        healthOfPlayer -= damageTaken;
        //Debug.Log("Damage taken" + healthOfPlayer); 
        PlayAudioForPlayer(2);
        playerHealthSlider.GetComponent<Slider>().value = 100 - healthOfPlayer;
        if (healthOfPlayer <= 0)
        {
            transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = false;
            gameObject.GetComponent<AudioSource>().volume = 0.5f;
            PlayAudioForPlayer(1);
            rcAnimator.SetBool("Destroyed", true);
            rcAnimator.Play("Rocket Sway.PlayerDestroyed", 1, 0f);
            StartCoroutine(Waitfor1second(2));                // wait for Rocket to blow up
            //Destroy(gameObject);
        }
    }

    /*           //not needed    //used PlayAudioForPlayer insted
    public void PlaySound()                            //Play powerUp Sound
    {
        Debug.Log("Playing Sound");
    }
    */

    public void PlayAudioForPlayer(int tempIndex)
    {
        if (GameManager.Instance.toggleSound == true)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(rocketSounds[tempIndex]);
        }
    }
    
    /* // open weapon door and all
    public void WeaponsAhoy()     //bundle these in one function and use switch to call them in another function activated by aniamtion event
    {
        //Debug.Log("Ahoy");
        rcAnimator.SetBool("OpenWeaponDoorLeft", true);
    }
    

    public void openRWD()         //for this
    {
        rcAnimator.SetBool("OpenWeaponDoorRight", true);
    }
    */ 

    /*
    void moveShipToFinger()                        // did not perform as expected
    {
        Touch touch1 = Input.GetTouch(0);

        Vector3 startPos = touch1.position;

        if(touch1.phase == TouchPhase.Moved)
        {
            //Debug.Log("In IF");

            Vector2 pos = touch1.position;
            pos.x = ((pos.x - width) / width) * speedModifier;
            //pos.y = (pos.y - height) / height;
            //position = new Vector3(pos.x, pos.y, 0.0f);
            position = new Vector3(pos.x, 0.0f, 0.0f);

            // Position the ship.
            //transform.position = position;    //Will position ship to the touch position on the screen(not perfect)
            //transform.position += position;     //ship goes out of the screen
            if(withinScreenBounds())
            {
                //transform.position = position;
                transform.position = new Vector3(position.x, transform.position.y, transform.position.z);
            }

            //direction = touch1.position - startPos;
            //transform.position = new Vector3(-direction.x,0.0f, 0.0f);
        }
    }
    */

    /*
    bool withinScreenBounds()
    {
        if (transform.position.x >= 2.8f || transform.position.x <= -2.65f)
        {
            //Debug.Log("Escaped Bounds!!!");
            return false;
        } 

        return true;
    }
    */
}
