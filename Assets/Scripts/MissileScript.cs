using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    private readonly int damageOutput = 40;
    private Vector3 lastPosition;                  // to record instantiation point
    private bool passedRocket;
    private bool goDown = false;
    private readonly float missileSpeed = 5f;

    private Vector3 tempLastPosition;

    void Start()
    {
        tempLastPosition = lastPosition = transform.position;  //dummy value for tempLastPosition

        {
        //bulletCollider = GetComponent<CapsuleCollider2D>();
        //rbUFO = GetComponent<Rigidbody2D>();

        //damageOutput = gameObject.CompareTag("enemyAmmo") ? 20 : 35;
            /*
            g = this.gameObject.transform.root.gameObject;
            enemyWeaponDetail = g.GetComponent<EnemyMovement>();
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(transform.right * 800.0f);
            //Ammo();
            */
        }

    }

    private void FixedUpdate()
    {
        
        if (!goDown && transform.position.y <= 5f)     //move upwards off-screen, then rotate after reaching certain height
        {
            transform.Translate(0.3f ,0f , 0f);
        }
         
        else                      //rotate to face the player
        {
            goDown = true;
            Vector3 tempRocketPosition = GameManager.Instance.mainRocket.transform.position;
            if (!passedRocket && !((transform.position - tempRocketPosition).magnitude <= 2f))
            {
                transform.right = (tempRocketPosition - transform.position) ;

                // move missile untill it reaches the Max circle from the player
                transform.position = Vector2.MoveTowards(transform.position, tempRocketPosition, Time.fixedDeltaTime * missileSpeed);
                tempLastPosition = tempRocketPosition;
            }

            else
            {
                passedRocket = true;

                //move the missile after reaching the Max circle
                //transform.Translate(transform.up * Time.fixedDeltaTime * missileSpeed);
                transform.position = Vector2.MoveTowards(transform.position, tempLastPosition * 2, Time.fixedDeltaTime * missileSpeed);       //more accurate
            }
        }
    }

    private void Update()
    {
        if (Mathf.Abs(lastPosition.y - transform.position.y) >= 10)
        {
            Destroy(gameObject);                  // as the bckgrnd is moving so, can destroy both at the same distance
        }  
    }

    // the ammo doesn't know at what it is being shot, only works with rigidbody2d
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RocketController tempRC = collision.GetComponentInParent<RocketController>();
            if (tempRC != null)
            {
                Debug.Log("Hit Player");
                tempRC.TakeDamagePlayer(damageOutput);
                Destroy(gameObject);
            }
        }
        //Destroy(gameObject);
    }
}
