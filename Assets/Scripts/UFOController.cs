using System;
using System.Collections;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UFOController : MonoBehaviour
{
    private int healthOfUFO = 100;
    private GameObject ammoEnemy;
    private int shootCounter;
    private Vector3 realLocation;
    private bool AtLocation;
    private float toggleLeftOrRight;
    private int toggleHolder;
    private int scoreValue;        //for normal UFO
    //[SerializeField]
    private int fireRateEnemy;
    //private bool lowerGunBoss = true;
    //private BossUFO bossUfO;
    //private bool true1, true2;

    public int idUFO;
    public Transform[] spawnAmmoAtEnemy;
    public AudioClip[] ufoSoundClips;
    //public bool isBoss;

    private void Start()
    {
        fireRateEnemy = GameManager.Instance.ufoSpwn.ufoFRHolder;
        scoreValue = GameManager.Instance.ufoSpwn.ufoScoreHolder;
        ammoEnemy = GameManager.Instance.ammoContainer[1];
    }

    private void FixedUpdate()
    {
        if (!AtLocation)                            //get to real location after being spawned.
        {
            GetToSpawnPoint();
        }

        else
        {
            { // shooting mechanism
                if (shootCounter % fireRateEnemy == 0 && healthOfUFO > 0)       //shoot after every 120th frame (assuming)(worked)
                {
                    ShootEnemy();
                    //Debug.Log(shootCounter);

                    if (shootCounter == 2400)
                    {
                        //Debug.Log("Reached UFO");
                        shootCounter = 0;
                    }
                }
                ++shootCounter;                // increment counter
            } 
        }
    }

    /*
    private void FixedUpdate()
    {
        //move UFO
        // problem with spawning UFO, also can improve the anim of appearing from the top of the screen if removed
        //transform.Translate(0.0f, 0.1f, 0.0f);   //move ship, #Main  
    }
    */

    private void ShootEnemy()
    {
        if (GameManager.Instance.toggleSound == true)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(ufoSoundClips[0]);
        }
        Instantiate(ammoEnemy, spawnAmmoAtEnemy[0].position, spawnAmmoAtEnemy[0].rotation);
        //StartCoroutine(WaitForGivenSeconds(2)); 
    }

    public void TakeDamageEnemy(int damageTaken)
    {
        {
            /*
            if (isBoss)                        // call the Boss Function
            {
                bossUfO.TakeDamageBoss(damageTaken);
            }

            else
            {
            }
            */ 
        }
            healthOfUFO -= damageTaken;
        if (healthOfUFO <= 0 && healthOfUFO > (-20))
        {
            //Debug.Log("Hit : " + gameObject.name);
            transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            gameObject.GetComponent<AudioSource>().volume = 0.5f;
            if (GameManager.Instance.toggleSound == true)
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(ufoSoundClips[1]);
            }
            transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Base Layer.EnemyDestroyed", 0, 0); 
            StartCoroutine(WaitForSeconds());
            --GameManager.Instance.ufoSpwn.totalUFOSpawned;
            GameManager.Instance.ufoSpwn.SetBufferAndUpdateScore(idUFO, scoreValue);
        }
    }

    /*
    private void BossUFO()
    {
        //Debug.Log("It's the BOSS!!!!");

        //bossUfO = new BossUFO();
        //Debug.Log(bossUfO.cale);

        healthOfUFO = 0;
        idUFO = 100;
        isBoss = true;
        RealLocationSet(false, new Vector3(2f, 3.8f, 0f), 0);
    }
    */

    public void RealLocationSet(bool tempReached, Vector3 tempLocation, int tempToggleTwo)
    {
        //Debug.Log(tempLocation);
        realLocation = tempLocation;
        AtLocation = tempReached;
        toggleHolder = tempToggleTwo;

        {  // shifted to spawn function.
            /*
            true2 = transform.position.y >= realLocation.y;
            if (toggleHolder == 2 || toggleHolder == 0)
            {
                //true2 = transform.position.y >= realLocation.y;

                true1 = transform.position.x <= realLocation.x;
                toggleLeftOrRight = -0.05f;
                Debug.Log(" Toggel is 2");
            }
            else                                               //tempToggleTwo == (-2)
            {
                //true2 = transform.position.y >= realLocation.y;

                true1 = transform.position.x >= realLocation.x;
                toggleLeftOrRight = 0.05f;
                Debug.Log(" Toggel is -2");
            }
            */
        }

        {
            /*
            if (realLocation.x > 0.8f)
            {
                toggleLeftOrRight = -0.05f;
            }
            else if (realLocation.x < (-0.8f))
            {
                toggleLeftOrRight = 0.05f;
            }
            toggleLeftOrRight = 0.05f;
            */
        }

    }

    public void GetToSpawnPoint()
    {
        //Debug.Log(Math.Abs(transform.position.y - realLocation.y));
        if (Math.Abs(transform.position.y - realLocation.y) >= 0.005f)       // not less than but greater than
        {
            bool true1, true2;
            true2 = transform.position.y >= realLocation.y;
            if (toggleHolder == 2 || toggleHolder == 0)
            {
                {
                    //true2 = transform.position.y >= realLocation.y;
                    //Debug.Log(" Toggel is 2");  
                }

                true1 = transform.position.x <= realLocation.x;
                toggleLeftOrRight = -0.05f;
            }
            else                                                 //tempToggleTwo == (-2)
            {
                {
                    //true2 = transform.position.y >= realLocation.y;
                    //Debug.Log(" Toggel is -2");
                }

                true1 = transform.position.x >= realLocation.x;
                toggleLeftOrRight = 0.05f;
            }

            //transform.position.x >= realLocation.x
            if (true1)    //greater for L->R, vice versa for R->L
            {
                if (true2)               //transform.position.y >= realLocation.y
                {
                    //Debug.Log("Reached x" + transform.position.x);
                    transform.Translate(0f, -0.05f, 0f); 
                }
            }
            else if (!true2)           //transform.position.y <= realLocation.y
            {
                if (true1)             //transform.position.x >= realLocation.x
                {
                    //Debug.Log("Reached y" + transform.position.y);
                    transform.Translate(toggleLeftOrRight, 0f, 0f);              //positive for L->R, vice versa for R->L
                }
            }
            else
            {
                //Debug.Log("Going on" + transform.position.x);
                transform.Translate(toggleLeftOrRight, -0.05f, 0f);             //positive for L->R, vice versa for R->L
            }
        }
        else
        {
            //Debug.Log("At Location!!");
            AtLocation = true;            // works properly
        }
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    /*
    void BossLowersGun()
    {
        transform.GetChild(0).gameObject.transform.GetChild(0).transform.Translate(0f, -0.005f, 0f);
        if (transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition.y <= -0.31f)
        {
            //Debug.Log("Guns Lowered");
            lowerGunBoss = false;                    //if cannon in position, then turn off lowering.
            StartCoroutine(WaitForGivenSeconds(1));
        }
    }
    */

    /*
    IEnumerator WaitForGivenSeconds(int tempIndex)
    { 
            if (tempIndex == 2)                        //for shooting ammo
            {
                //Debug.Log("Shooting from 2nd point");

                yield return new WaitForSeconds(0.2f);
                Instantiate(ammoEnemy, spawnAmmoAtEnemy[1].position, spawnAmmoAtEnemy[1].rotation);
            }
            else                                       // for rotating weapons
            {
                yield return new WaitForSeconds(0.5f);
                transform.GetChild(0).GetComponent<Animator>().SetBool("rotateWeapons", true);
            } 
    }
    */
}

/*
public class BossUFO                             // this works
{
    public int cale = 10;
    public int healthBoss = 1000;
    public int idBoss = 100;

    public void TakeDamageBoss(int damageTaken)
    {
        healthBoss -= damageTaken;
        if (healthBoss <= 0)
        {

        }
    }
}
*/
