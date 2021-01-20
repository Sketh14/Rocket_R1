using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private Vector3 lastPosition;                  // to record instantiation point
    //private bool getDestroyed = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, -0.03f, 0f);

        if (Mathf.Abs(lastPosition.y - transform.position.y) >= 10)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("Health"))              //increase Health
        {
            RocketController tempRC = collision.GetComponentInParent<RocketController>();
            if (tempRC != null)
            {
                //Debug.Log("Health Up");
                //gameObject.GetComponent<BoxCollider2D>().enabled = false;            //no need, because of OnTriggerExit
                if (tempRC.healthOfPlayer != 100)
                {
                    tempRC.healthOfPlayer += 15;
                }

                Destroy(gameObject); 
            }
        }

        else if (collision.CompareTag("Player") && gameObject.CompareTag("Weapon"))        //increase Fire Rate
        { 
            RocketController tempRC = collision.GetComponentInParent<RocketController>();
            if (tempRC != null)
            {
                //Debug.Log("FR Increase"); 
                if (tempRC.fireRatePlayer != 20)
                {
                    tempRC.fireRatePlayer -= 10;
                }

                Destroy(gameObject); 
            }
        }

        //collision.GetComponentInParent<RocketController>().PlaySound();
        collision.GetComponentInParent<RocketController>().PlayAudioForPlayer(3);
    }
}
