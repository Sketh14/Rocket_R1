//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite[] backgroundSprites;
    public RocketController rCtrlr;
    public BackGroundScroller bGSclr;
    public UFOSpawner ufoSpwn;
    public GameObject dialoguePanel;
    public GameObject[] ammoContainer;
    public GameObject teleportPrefab;
    public Text highScoreContainer;
    public GameObject healthSliderBoss;
    public GameObject mainRocket;
    public GameObject missilePrefab;
    public bool toggleSound = true;
    public int bossDefeated;                         //set this in inspector
    //public GameObject UFOPrefab;
    //public GameObject mainBackGround;
    //public bool toggleMusic = true;
    //public bool musicController = true;
    //public bool soundEffectController = true;
    //public bool bossAttacks;                         //get out of regenerate mode

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            {
                /*
                if (_instance == null)
                {
                    Debug.Log("GameManager Created");
                    GameObject obj = new GameObject("PlayerManager");
                    obj.AddComponent<GameManager>();
                }
                */
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        } 
    }
}
