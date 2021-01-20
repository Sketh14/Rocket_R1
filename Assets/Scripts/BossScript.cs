using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    private int healthOfUFO = 1000;
    private GameObject ammoEnemy;
    private int shootCounter;
    private readonly int scoreValue = 1000;                   //for Boss UFO 
    private bool isShooting = false;                 // also used for recharge health
    private bool laserExtended = false;
    private bool shieldsOff = false;
    private bool regenHealth = false;
    private bool doneOnce = false;
    //private readonly float realLocation = 3f;      //height of UFO from the player

    private int intToggle = 1;                                      // -1, default value 
    /* can have one int value toggle for all these update functions
     * private bool AtLocation = false;                                 //intToggle = 1
     * private bool lowerGunBoss = true;                                //intToggle = 2
     * private bool shootBullet;                                        //intToggle = 3
     * private bool shootLaser;                                         //intToggle = 4
     * private bool shootMissile;                                       //intToggle = 5
     * private bool rechargeAll;                                        //intToggle = 6
     * public Slider healthSlider;
     */

    //[SerializeField]
    private int[] intToggle2 = new int[6] { 0, 0, 0, 0, 0, 0 };                                    //0, default value 
    /* this toggle is for keeping check how many times a particular weapons system is fired.
     
     * intToggle2 = 0       // is for normal bullets
     * intToggle2 = 1       // is for lasers
     * intToggle2 = 2       // is for missiles
     * intToggle2 = 3       // is for how many boss defeated.
     * intToggle2 = 4       // is for how many missiles fired.
     * intToggle2 = 5       // for how many times any weapon system is fired
     */
    private int wpnIndxBfrRchrge = 0;
    private AudioSource bossSoundsSource;

    public int fireRateEnemy;
    public Transform[] spawnAmmoAtEnemy;                            //can also be used to store missile location
    public GameObject[] laserTemp;
    public AudioClip[] bossSounds;

    public bool fireTest;
    //test toggle
    /*
    public bool level1;
    public bool level2;
    public bool level3;
    */

    void Start()
    {
        //Debug.Log("Health : " + healthOfUFO); 
        ammoEnemy = GameManager.Instance.ammoContainer[1];
        bossSoundsSource = gameObject.GetComponent<AudioSource>();
        GameManager.Instance.healthSliderBoss.SetActive(true);
        GameManager.Instance.healthSliderBoss.GetComponent<Slider>().value = 0;
        intToggle2[3] = GameManager.Instance.bossDefeated;
    }

    private void FixedUpdate()
    {

        //Debug.Log( 2>2 ? "Yes" : "No" );
        //test toggle
        { /*
            if (fireTest)
            {
                //intToggle = 6;
            }
            */
        }

        //progress system for Boss (didn't need this here)
        {

        }

        // follow player 
        {
            if (intToggle != 1 && intToggle < 6 && intToggle != 0)
            {
                float positionDifference = GameManager.Instance.mainRocket.transform.position.x - transform.position.x;
                //Debug.Log(positionDifference);
                if (positionDifference >= 0.1f)
                {
                    // Debug.Log("Positive");
                    FollowPlayer(0.01f);
                }

                else if (positionDifference <= -0.1f)
                {
                    // Debug.Log("Negative");
                    FollowPlayer(-0.01f);
                }
            }
        }

        //intToggle script
        {
            //get to real location after being spawned.
            if (intToggle == 1)
            {
                //GetToSpawnPoint(3f);

                float tempBossPosition = transform.position.y;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0f, 3f)
                                                              , Time.fixedDeltaTime * 2f);
                if (tempBossPosition == 3f)
                {
                    //Debug.Log("At Location!!");
                    intToggle = 2;
                }
            }

            //lower the guns.
            else if (intToggle == 2)
            {
                //Debug.Log("The Boss Is Lowering Guns!!");
                BossLowersGun();
            }

            // shooting bullet script
            else if (intToggle == 3)
            {
                // shooting mechanism
                if (shootCounter % fireRateEnemy == 0)       //shoot after every 120th frame (assuming)(worked)
                {
                    //Debug.Log("Shooting Bullets");

                    ShootEnemy(0);
                    //Debug.Log(shootCounter);

                    if (shootCounter == 2400)
                    {
                        //Debug.Log("Reached UFO");
                        shootCounter = 0;
                    }
                }
                ++shootCounter;                // increment counter 

                if (isShooting)
                {
                    //Debug.Log("Going To WaitForSeconds" + isShooting);

                    StartCoroutine(WaitForGivenSeconds(3, 0));
                    isShooting = false;
                }
            }

            //wait for sometime before firing laser
            else if(intToggle == 4)
            { 
                if (isShooting)
                {
                    isShooting = false;
                    StartCoroutine(WaitForGivenSeconds(9, 1));
                }
            }

            //wait before firing missiles to warn player
            else if (intToggle == 5)
            {
                if (isShooting)
                {
                    //Debug.Log("Charging Missiles");
                    isShooting = false;
                    StartCoroutine(WaitForGivenSeconds(9, 2));
                }
            }

            //go to recharge position
            else if (intToggle == 6)
            {
                //GetToSpawnPoint(4f);

                // go to recharge position
                float tempBossPosition = transform.position.x;
                if (tempBossPosition >= 0.1f || tempBossPosition <= -0.1f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(0f, 4f), Time.fixedDeltaTime * 2f);
                    transform.localScale -= new Vector3(0.002f, 0.002f, 0f);         //decrease size to make it look like it backs up
                    //FollowPlayer(-0.1f);
                }

                else
                {
                    Debug.Log("Recharge Point Reached!!");

                    transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                    //GameManager.Instance.bossAttacks = false;
                    GameManager.Instance.ufoSpwn.spawnWhileBossRegenerates = true;         //inverse for normal spawns
                    GameManager.Instance.ufoSpwn.possibleToSpawnUfO = true;
                    regenHealth = doneOnce = true;
                    intToggle = 7;                         //do nothing
                }
            }

            //regenerate health
            else if (intToggle == 7)
            {
                if (regenHealth && healthOfUFO < 1000)              //isShooting alone doesnt work well with the Health regen
                {
                    if (doneOnce)
                    {
                        StartCoroutine(WaitForGivenSeconds(8, -1));
                        doneOnce = false;
                    }
                }

                if (!GameManager.Instance.ufoSpwn.spawnWhileBossRegenerates)
                {
                    regenHealth = false;
                    if (transform.localScale.x <= 0.7f)
                    {
                        transform.localScale += new Vector3(0.002f, 0.002f, 0f);       //get back to normal size
                    }

                    if (transform.localPosition.y >= 3f)
                    {
                        //isShooting = true;
                        transform.Translate(-0.01f, -0.01f, 0f);                           //get back to original position
                    }
                    else
                    {

                        if (isShooting)
                        {
                            //Debug.Log("Reached Position and Scale");

                            StartCoroutine(WaitForGivenSeconds(4, wpnIndxBfrRchrge));
                            transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                            isShooting = false;
                        }
                    }
                }
            }

            // fire missiles
            else if(intToggle == 8)
            {
                if (isShooting)
                {
                    //Debug.Log("Firing Missiles");
                    ShootEnemy(2);
                    StartCoroutine(WaitForGivenSeconds(6, 2));
                    isShooting = false;
                } 
            }

            //shoot laser
            else if (intToggle == 9)
            {
                if (!laserExtended)
                {
                    //Debug.Log("Calling Method");

                    ShootLasers();
                }
                RaycastHit2D[] hitContainer = new RaycastHit2D[2];

                //for detecting if laser hit anything.
                for (int i = 0; i < 2; i++)
                {
                    hitContainer[i] = Physics2D.Raycast(laserTemp[i].transform.parent.transform.position, transform.up * -1, 8);

                    //Debug.DrawRay(laserTemp[0].transform.parent.transform.position, transform.up * -8, Color.red);
                    if (hitContainer[i].transform)
                    {
                        if (hitContainer[i].transform.CompareTag("Player"))
                        {
                            //Debug.Log("Hit!!!" + hitContainer[i].transform.name);          // works!!

                            GameManager.Instance.rCtrlr.TakeDamagePlayer(5);
                        }
                    }

                }

                if (isShooting)
                {
                    StartCoroutine(WaitForGivenSeconds(5, 1));           // turn off laser after shooting
                    isShooting = false;
                }
            } 
        }
    }

    private void ShootEnemy(int tempIndex)
    {
        if (tempIndex == 0)
        {
            PlayAudioForBoss(0);
            Instantiate(ammoEnemy, spawnAmmoAtEnemy[0].position, spawnAmmoAtEnemy[0].rotation);
            StartCoroutine(WaitForGivenSeconds(2, 0));
        }

        else
        { 
            PlayAudioForBoss(4);
            Instantiate(GameManager.Instance.missilePrefab, spawnAmmoAtEnemy[2].position, spawnAmmoAtEnemy[2].rotation);
            StartCoroutine(WaitForGivenSeconds(7, 2));
        }
    }

    public void TakeDamageEnemy(int damageTaken)
    {
        //Debug.Log("Hit");
        if (shieldsOff)
        {
            healthOfUFO -= damageTaken;
            GameManager.Instance.healthSliderBoss.GetComponent<Slider>().value = 1000 - healthOfUFO;
            if (healthOfUFO <= 0 && healthOfUFO > (-20))
            {
                transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = false; 
                transform.GetChild(0).GetComponent<Animator>().Play("Shield.BossDestroyed", 1, 0);
                PlayAudioForBoss(7);
                StartCoroutine(WaitForGivenSeconds(10, -1));
                GameManager.Instance.bossDefeated++;
                GameManager.Instance.ufoSpwn.SetBufferAndUpdateScore(-1, scoreValue);       // for boss
                GameManager.Instance.ufoSpwn.spawnWhileBossRegenerates = true;
                GameManager.Instance.ufoSpwn.possibleToSpawnUfO = true;
                GameManager.Instance.healthSliderBoss.SetActive(false);
                GameManager.Instance.ufoSpwn.enemyWave++;                                  // enemy
                //Debug.Log("Increasing Wave : " + GameManager.Instance.ufoSpwn.enemyWave + " Health : " + healthOfUFO + " Name : "+ gameObject.name);
                if(GameManager.Instance.bossDefeated == 1)
                {
                    GameManager.Instance.rCtrlr.WeaponRFree();
                }
            }
        }
    }

    void BossLowersGun()
    {
        if (transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition.y <= -0.31f)
        {
            //Debug.Log("Guns Lowered");
            intToggle = -1;                    //if cannon in position, then turn off lowering.
            StartCoroutine(WaitForGivenSeconds(1, 0));
        }
        transform.GetChild(0).gameObject.transform.GetChild(0).transform.Translate(0f, -0.005f, 0f);
    }
     
    //didn't need this anymore, updated code
    /*
    public void GetToSpawnPoint(float getToLocation)       //after spawn, during recharge
    {
        //Debug.Log(Math.Abs(transform.position.y - realLocation.y));

        if (getToLocation == 4f)
        {
            if (Mathf.Abs(transform.position.y - getToLocation) >= 0.05f)
            {
                transform.Translate(0.0f, 0.05f, 0f);
            } 
        } 
        else
        {
            if (Mathf.Abs(transform.position.y - getToLocation) >= 0.05f)       // not less than but greater than
            {
                transform.Translate(0.0f, -0.05f, 0f);             //positive for L->R, vice versa for R->L
            } 
            else
            {
                //Debug.Log("At Location!!");
                intToggle = 2;            // works properly         // is at position 
            } 
        }


    }
    */
     
    private void ShootLasers()
    {
        //Debug.Log("Executing"); 
        if (laserTemp[0].transform.localScale.x < 106)
        {
            //Debug.Log("Extending");
            for(int i =0; i<2; i++)
            {
                laserTemp[i].transform.localScale += new Vector3(20f, 0f, 0f);
                laserTemp[i].transform.localPosition += new Vector3(0f, -6.1f, 0f); 
            }
        }
        else
        {
            Debug.Log("Fully Extended");
            laserExtended = true;
        }
    }

    private void FollowPlayer(float tempDifference)         //during game, during recharge
    {
        transform.Translate(tempDifference, 0f, 0f);
    }

    private void PlayAudioForBoss(int tempIndex)            //created a single function as it was being called several times
    { 
        if (GameManager.Instance.toggleSound == true)
        {
            bossSoundsSource.PlayOneShot(bossSounds[tempIndex]);
        }
    }

    void ProgressSystem(int tempToggle)         //which weapon to fire according to the boss defeated
    {
        if(intToggle2[3] >= 3)           // missiles, will recieve 0, 1 or 2
        {
            //Debug.Log("Boss Defeated >= 3");

            switch (tempToggle)
            {
                case 0:
                    WeaponSystem(0);       // bullets

                    break;

                case 1:
                    {
                        //as missile can only be fired once, no need to call WeaponSystem
                        //wpnIndxBfrRchrge = 1;       //schtewpit move, innit     //temp storage to turn off shields for laser
                        intToggle2[1] = 0; 
                        int tempRandomMltply2 = UnityEngine.Random.Range(0, 2) * 2;           // to add +2 to 3 in next statement
                        intToggle = 3 + tempRandomMltply2;         //fire bullets or missiles
                    }

                    break;

                default:
                    WeaponSystem(2);       // missiles

                    break;
            }
        }

        else if (intToggle2[3] == 1 || intToggle2[3] == 2)      // lasers,  will recieve 1 or 2 
        {
            //Debug.Log("Boss Defeated 1/2");
            switch(tempToggle)
            {
                case 0:
                    {
                        //Debug.Log("Bullets Spawn");

                        WeaponSystem(0); 
                    }

                    break;

                default:
                    {
                        //Debug.Log("Laser Spawn");

                        // as laser can only be fired once, reset and continue to fire bullet
                        // as boss is 2nd or 3rd, no need to take missile into account.
                        //wpnIndxBfrRchrge = 1;             //schtewpit move, innit         //temp storage to turn off shields for laser
                        intToggle = 3;
                        intToggle2[1] = 0; 
                    }

                    break;
            }
        } 
          
        else                                                // bullets, will recieve 0
        {
            //Debug.Log("Boss Defeated 0");

            intToggle = 3;

            if(intToggle2[0] == 3)
            {
                intToggle2[0] = 0;
                //intToggle = 6;
            }
        }
    }

    //since laser can only spawn once at any given tme, no need to add it here
    void WeaponSystem(int systemIndex)                     // will get 0 or 2
    {
        int tempRandomNo = (intToggle2[3] >= 3) ? 6 : 5;

        //Debug.Log(tempRandomNo);

        switch(systemIndex)
        {
            case 0:                            // check if bullets has spawned enough
                {
                    if (intToggle2[0] == 3)   // if spawned full, then have to change, reset bullet count
                    {
                        //Debug.Log("Reset Bullet Count"); 

                        intToggle = 4;
                        intToggle2[0] = 0;
                    }
                    else if (intToggle2[0] == 2)    // if spawned enough, then use random to decide is continue spawn or change
                    { 
                        intToggle = UnityEngine.Random.Range(3, tempRandomNo);

                        //Debug.Log("Spawned Enough Bullet, Spawning :" + intToggle);
                    }
                    else                // if spawned low, continue spawn
                    {
                        //Debug.Log("Bullets not Enough Spawning more");

                        intToggle = 3;
                    }
                }

                break;

            default:                       //check if missiles have spawned enough times
                {
                      // if spawned full, then change to bullet or laser,check if previous was laser or not
                    if (intToggle2[2] == 2)
                    {
                        //Debug.Log("Spawned Enough Missiles");
                        intToggle2[2] = 0;

                        if (intToggle2[1] == 1)
                        {
                            //Debug.Log("Spawning Bullets");

                            intToggle = 3;
                        }
                        else
                        {
                            //Debug.Log("Spawning Laser");

                            intToggle = 4;
                        }
                    }

                    // if spawned some or only 1, then use random to decide if laser or bullet or missile
                    else
                    {
                        intToggle = UnityEngine.Random.Range(3, 6);

                        //Debug.Log("Spawned enough missiles, spawning :" + intToggle);
                    }

                    // if spawned low, continue spawn.( This won't be needed as you cant span low as 
                    //there is only 1 or 2 spawns, if this is called, then there is already a spawn present)
                }
                break;
        }
    }

        IEnumerator WaitForGivenSeconds(int tempIndex, int weaponFiredIndex)
    {
        if (tempIndex == 2)                        //for shooting ammo
        {
            //Debug.Log("Shooting from 2nd point");

            yield return new WaitForSeconds(0.2f);
            Instantiate(ammoEnemy, spawnAmmoAtEnemy[1].position, spawnAmmoAtEnemy[1].rotation);
        }

        else if (tempIndex == 3)                          //code for bullets
        { 
            intToggle2[0]++;
            intToggle2[5]++;                            //increase counter

            //Debug.Log("Shooting Bullets" + intToggle2[5]);

            yield return new WaitForSeconds(5f); 

            intToggle = -1;                              // go into recharge mode
            //test toggle 
            /*
            if (fireTest)
            {
                wpnIndxBfrRchrge = 3;                       //for testing
            }
            */
            StartCoroutine(WaitForGivenSeconds(4, 0));  // recharge for 5 sceonds. 
        }

        else if (tempIndex == 4)                    //shoot after recharging, after 3 seconds.
        {
            //Debug.Log("Recharging");

            {   // didn't need this
                /*
                switch(weaponFiredIndex)
                {
                    case 0:


                        break;
                    case 1:


                        break;
                    case 2:


                        break;
                }
                */

                /*
                if(intToggle2[5] == 0)
                {
                    intToggle2[5]++;
                    ProgressSystem(weaponFiredIndex);
                    yield break;
                }
                */
            }

            if (intToggle2[5] != 0 && wpnIndxBfrRchrge != 4)
            {
                //Debug.Log("Shields Off");
                shieldsOff = true; 
                PlayAudioForBoss(6);
                transform.GetChild(0).GetComponent<Animator>().SetBool("shieldToggle", false);            //turn off shields here 
                transform.GetChild(0).GetComponent<Animator>().Play("Shield.ShieldAppearBoss", 1, 1f);    // play shield disappear at full
                transform.GetChild(0).GetComponent<Animator>().SetFloat("speedMultiplier", -1f);          //play shield disappear backwards
                //transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;                   //turn on collision after shields off

                yield return new WaitForSeconds(1.6f);         //(1.6f) 
                transform.GetChild(0).GetComponent<Animator>().SetFloat("speedMultiplier", .5f); 
            }

            //if in recharge mode, return early(6), //(OB)+1 as need to increase after coming out of recharge
          
            if (intToggle2[5] == 6)
            {
                Debug.Log("Recharging");

                intToggle2[5] = 0;
                intToggle = 6;
                wpnIndxBfrRchrge = weaponFiredIndex;
                isShooting = true; 
                yield break;
            }

            if (intToggle2[5] != 0)
            {
                yield return new WaitForSeconds(1.4f);   // recharge after 3 seconds.(1.6 + 1.4)
            }

            ProgressSystem(weaponFiredIndex);            // set which weapon to fire next

            //test toggle 
            /*
            if (!fireTest)
            {
                ProgressSystem(weaponFiredIndex);            // set which weapon to fire next
            }
            //test Block
            if (fireTest)
            {
                if (wpnIndxBfrRchrge == 4)
                {
                    intToggle = 5;
                }
                else
                {
                    intToggle = 4;
                }
            }
            */

            if (intToggle != 4)
            {
                shieldsOff = false;
                wpnIndxBfrRchrge = -1;                   //for laser, if laser was fired earlier

                //test Block
                /*
                if (!fireTest)
                {
                    wpnIndxBfrRchrge = -1;                   //for laser, if laser was fired earlier
                }
                */
                 
                PlayAudioForBoss(5);
                transform.GetChild(0).GetComponent<Animator>().SetFloat("speedMultiplier", 1f);          // play shield appear normally
                transform.GetChild(0).GetComponent<Animator>().SetBool("shieldToggle", true);            //start the shield turns on anim here.
                //transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = false;               //No need(OB), turn off collision as shields are on

                //Debug.Log("Shields On - WaitForGivenSeconds" + wpnIndxBfrRchrge);
            }
            yield return new WaitForSeconds(1f);

            isShooting = true; 
        }

        else if (tempIndex == 5)                             // code for laser
        {
            //Debug.Log("Shooting Laser");

            intToggle2[1]++;
            intToggle2[5]++;                            //increase counter 
            PlayAudioForBoss(2);
            yield return new WaitForSeconds(3f);

            //Debug.Log("Laser Empty");
            laserExtended = false;
            transform.GetChild(0).GetComponent<Animator>().SetBool("laserChargeUp", false); 
            foreach (var laser in laserTemp)            //turn off the laser 
            {
                laser.transform.localScale = new Vector3(0f, 6f, 1f);                     //reset scale of the laser
                laser.transform.localPosition = new Vector3(-0.007f, -0.9f, -0.04f);      //reset the position
            }

            intToggle = -1;                             //go into recharge mode 
            wpnIndxBfrRchrge = 4;
            StartCoroutine(WaitForGivenSeconds(4, 1));

            // start shooting again, or turn off the shields
            //ProgressSystem(1);
        }

        else if (tempIndex == 6)            // code for missiles goes here
        { 
            intToggle2[4]++;
            //Debug.Log("Shooting Missiles 1st point : " + intToggle2[4]);

            yield return new WaitForSeconds(0.5f);
            isShooting = true;
            if (intToggle2[4] == 6)
            {
                //Debug.Log("Enough Missile Spawn");
                bossSoundsSource.Stop();
                intToggle = -1;                             //go into recharge mode
                transform.GetChild(0).GetComponent<Animator>().SetBool("launchRocket", false);
                yield return new WaitForSeconds(1f);        //wait for some time after firing missile

                //test Block
                /*
                if (fireTest)
                {
                    wpnIndxBfrRchrge = 5;                           //for testing
                }
                */

                isShooting = false;
                StartCoroutine(WaitForGivenSeconds(4, 2));
                intToggle2[2]++;
                intToggle2[5]++;                            //increase counter
                intToggle2[4] = 0;
                //ProgressSystem(2);
            }
        }

        else if (tempIndex == 7)            //for shooting missiles from second point
        {
            yield return new WaitForSeconds(0.3f);
            Instantiate(GameManager.Instance.missilePrefab, spawnAmmoAtEnemy[3].position, spawnAmmoAtEnemy[3].rotation);
            intToggle2[4]++;
            //Debug.Log("Shooting Missiles 2nd point : " + intToggle2[4]);
        }

        else if (tempIndex == 8)            //for regenerating health
        {
            yield return new WaitForSeconds(5f);

            doneOnce = true;
            //Debug.Log("Rgenerating Health : " + healthOfUFO);
            healthOfUFO += 10;
            // Keep Updating the health Bar
            GameManager.Instance.healthSliderBoss.GetComponent<Slider>().value = 1000 - healthOfUFO;
        }

        else if(tempIndex == 9)                    // wait for few seconds before firing the missile / laser.
        {
            //Debug.Log("Wait");
            if (weaponFiredIndex == 2)            // wait for missile   
            {
                //Debug.Log(isShooting);
                transform.GetChild(0).GetComponent<Animator>().SetBool("launchRocket", true);                    //start firing sequence
                transform.GetChild(0).GetComponent<Animator>().Play("Base Layer.LaunchMissile", 0, 1f);           //finish BossRotate
                bossSoundsSource.clip = bossSounds[3];
                bossSoundsSource.Play();
                yield return new WaitForSeconds(2f);
                //bossSoundsSource[0].Stop();
                intToggle = 8;
                isShooting = true;
            }
            else                                  //wait for laser
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("laserChargeUp", true);                   //play laser charge anim
                PlayAudioForBoss(1);
                yield return new WaitForSeconds(1.5f);                   //3f seems too long, 1.5f is ok
                bossSoundsSource.Stop();
                intToggle = 9;
                isShooting = true;
            }
            //Debug.Log("Start Firing");
        }

        else if(tempIndex == 10)
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }

        else                                       // for rotating weapons
        {
            intToggle = 3;
            isShooting = true;
            transform.GetChild(0).GetComponent<Animator>().SetBool("shieldToggle", true);            //turn shield on
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(0).GetComponent<Animator>().SetBool("rotateWeapons", true);
        }
    }   
} 
