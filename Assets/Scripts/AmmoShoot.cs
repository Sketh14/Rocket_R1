using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoShoot : MonoBehaviour
{
    /*
    private Rigidbody2D rb;
    public EnemyMovement aboutEnemyClones;
    private GameObject g;
    private EnemyMovement enemyWeaponDetail;
    private Rigidbody2D rbUFO;
    private CapsuleCollider2D bulletCollider;
    */

    private int damageOutput;
    private Vector3 lastPosition;                  // to record instantiation point
    private bool getDestroyed = false;

    public AudioClip enemySound;

    void Start()
    {
        lastPosition = transform.position;
        //bulletCollider = GetComponent<CapsuleCollider2D>();
        //rbUFO = GetComponent<Rigidbody2D>();

        damageOutput = gameObject.CompareTag("enemyAmmo") ? 20 : 35;

        {
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
        if (gameObject.CompareTag("enemyAmmo"))
        {
            //rbUFO.AddForce(new Vector2(0f, -0.05f));

            transform.Translate(0f, -.05f, 0f);   //UFO  //make enemy shot a little slower, too fast 0.15f
        }

        //take into account the speed of the ship
        transform.Translate(0f, 0.13f, 0f);        //Player  //1f is a little fast, .05f looks good, may change later
    }

    private void Update()
    {
        if(Mathf.Abs(lastPosition.y - transform.position.y) >= 10)
        {
            Destroy(gameObject);                  // as the bckgrnd is moving so, can destroy both at the same distance
        }

        {   //was deleting both separately //old method
            /*
            if (gameObject.CompareTag("enemyAmmo") && Mathf.Abs(lastPosition.y - transform.position.y) >= 10) // destroy ammo if it gets too far,UFO
            {
                //Debug.Log("Destroy Bullet At Update");
                //Debug.Log("Lst p : " + lastPosition.y + " trns p : " + transform.position.y);
                Destroy(gameObject);
            }
            else if (!gameObject.CompareTag("enemyAmmo") && Mathf.Abs(lastPosition.y - transform.position.y) >= 10)     //Player
            {
                //Debug.Log("Lst p : " + lastPosition.y + " trns p : " + transform.position.y);
                Destroy(gameObject);
            }
            */
        }
    } 

    // the ammo doesn't know at what it is being shot, only works with rigidbody2d
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!getDestroyed)
        {
            //Debug.Log("Hit Something : " + collision.name);
            if (collision.CompareTag("Player") && gameObject.CompareTag("enemyAmmo"))
            {
                getDestroyed = true;
                RocketController tempRC = collision.GetComponentInParent<RocketController>();
                if (tempRC != null)
                {
                    //Debug.Log("Hit Player");
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                    tempRC.TakeDamagePlayer(damageOutput);
                    Destroy(gameObject);
                    //return;     // no need       // early return so as to not check further if statement
                }
            }

            else if (collision.CompareTag("Enemy") && !gameObject.CompareTag("enemyAmmo"))
            {
                getDestroyed = true;
                var tempUC = collision.GetComponentInParent<UFOController>();
                if (tempUC != null)
                {
                    //Debug.Log("Hit Enemy" + collision.tag);
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                    tempUC.TakeDamageEnemy(damageOutput);
                    Destroy(gameObject);
                }
            }

            else if (collision.CompareTag("UfoBoss") && !gameObject.CompareTag("enemyAmmo"))
            {
                getDestroyed = true;
                BossScript tempUC = collision.GetComponentInParent<BossScript>();
                if (tempUC != null)
                {
                    //Debug.Log("Hit Enemy" + collision.tag);
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                    tempUC.TakeDamageEnemy(damageOutput);
                    Destroy(gameObject);
                }
            }
            //Destroy(gameObject);
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMovement enemy = collision.GetComponent<EnemyMovement>();
        if (enemy != null && PlayerManager.Instance.MovePlayer.indexOW != 10)
        {
            //Physics2D.IgnoreCollision(enemy.enemyCollider,bulletCollider);
            enemy.takeDamage(PlayerManager.Instance.MovePlayer.SelectedWeapon.weapons[PlayerManager.Instance.MovePlayer.indexOW].damageSize);
            if (enemy.health <= 0)
            {
                PlayerManager.Instance.killCounter();
            }
            //Debug.Log("damage : "+ weaponDamage.SelectedWeapon.weapons[weaponDamage.indexOW].damageSize);
        }

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            //Debug.Log(enemyWeaponDetail.randomizerOfEnemyWeapon);
            player.takeDamageByEnemy(PlayerManager.Instance.MovePlayer.SelectedWeapon.weapons[enemyWeaponDetail.randomizerOfEnemyWeapon].damageSize);
        }

        if (collision.tag != "Weapon" && collision.tag != "HealthRefill" && collision.tag != "AmmoRefill")
        {
            if (collision.name != "LvlControl" && collision.name != "LvlControl (1)")
                Destroy(gameObject);
        }
    }

    */
}
