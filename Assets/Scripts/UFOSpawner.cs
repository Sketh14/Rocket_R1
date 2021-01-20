using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SocialPlatforms.Impl;
//using UnityEngine.UI;

public class UFOSpawner : MonoBehaviour
{
    //[SerializeField]
    private bool[] bufferLocations = new bool[30];
    private bool spawnUFOBoss = false;
    private bool doneOnce = true;
    private int changeNoOfUFOSpawned = 6;
    private readonly int ufoSpawnedForBoss = 5;
    private int ufoOccurenceFrequencyForBoss;
    private readonly int[] toggleTwoPlusOrMinus = { -2, 0, 2 }; 
    private bool changeEnemyWaveValue = false;           //change the enemyWave counter  
    private GameObject UFOBoss = null;

    public int enemyWave = 0;                           //how many waves of enemy has appeared 
    public bool possibleToSpawnUfO = false;             //turned off
    public bool spawnWhileBossRegenerates = true;
    public int totalUFOSpawned = -1;
    public Transform[] spawnLocationsUFO;
    public Transform[] spawnUnderBossUFO;            // one will do        //for Boss spawn locations
    public GameObject UFOPrefab;
    public GameObject UFOBossPrefab;
    public GameObject[] PowerUpPrefab;
    public int ufoScoreHolder = 10;
    public int ufoFRHolder = 80;
    public int highScore = 0;
    //public bool openedOnce = false;

    private void FixedUpdate()
    {
        //Progress System
        {
            //update after every 6th wave of enemy
            if (changeEnemyWaveValue && (enemyWave % 6 == 0))       //enemy
            {
                Debug.Log("Updating System");

                changeEnemyWaveValue = false;
                ufoScoreHolder += 15;
                if (ufoFRHolder >= 35)
                {
                    ufoFRHolder -= 5;
                }
                //spawn powerup
                Instantiate(PowerUpPrefab[Random.Range(0,2)], new Vector3(Random.Range(-2, 3), 5f, 0f), Quaternion.identity);
            }
        }

        //Spawn Boss
        if (enemyWave != 0 && (enemyWave % 8) == 0 && totalUFOSpawned == 0 && UFOBoss == null)  //boss
        {
            if (changeNoOfUFOSpawned != 14)           //Keep in check that over 15 UFO is not possible  //works
            {
                changeNoOfUFOSpawned += 2;            //Keep increasing the maximum possible no. of UFO spawned
            }
            spawnWhileBossRegenerates = false;
            spawnUFOBoss = true;
            //Debug.Log("Spawn Boss " + possibleToSpawnUfO);
            //enemyWave++;
        } 
        else
        {
            if (spawnWhileBossRegenerates)                 //inverse for normal spawning
            {
                if (totalUFOSpawned == ((UFOBoss != null) ? ufoSpawnedForBoss : changeNoOfUFOSpawned))
                {
                    doneOnce = true;

                    {
                        /*
                        if (changeEnemyWaveValue)
                        {
                            Debug.Log("Incrementing WaveValue");
                            enemyWave++;
                            changeEnemyWaveValue = false;
                        }
                        */
                    } 
                    possibleToSpawnUfO = false;             // can't spawn more 
                    //totalUFOSpawned = 0;                    //(wrong) reset counter for UFO for next spawn
                }
                else if (totalUFOSpawned == 0)
                {
                    //Reset the Buffer bufferLocations
                    { 
                        for(short i =0; i<30; i++)
                        {
                            bufferLocations[i] = false;
                        }
                    }

                    //check if ufo spawned during Boss are destroyed or not
                    {
                        if (UFOBoss != null)
                        {
                            //Debug.Log("Increase Boss wave" + ufoOccurenceFrequencyForBoss);
                            ufoOccurenceFrequencyForBoss++;                 //If boss spawned, then increment frequency
                        }

                        if (ufoOccurenceFrequencyForBoss == 3)               //Only spawn 2 waves
                        {
                            //Debug.Log("Reset Boss wave" + ufoOccurenceFrequencyForBoss);
                            //GameManager.Instance.bossAttacks = true;
                            ufoOccurenceFrequencyForBoss = 0;
                            spawnWhileBossRegenerates = false;
                            doneOnce = false;
                        }
                    }

                    //Debug.Log("Reached possible to spawn");           // prints too much
                    if (doneOnce)
                    {
                        //Debug.Log("Initiating Spawn");
                        doneOnce = false;
                        if(UFOBoss != null)
                        { 
                            possibleToSpawnUfO = true; 
                        }
                        else
                        {
                            StartCoroutine(WaitForSeconds2());
                        }
                    }
                }
            }
        }

        //  Spawn UFO/UFOBoss Code
        if (spawnUFOBoss)                   // Spawn UFO Boss
        {
            //Debug.Log("Spawning Boss");
            UFOBoss = Instantiate(UFOBossPrefab, new Vector3(Random.Range(-2, 3), 5f, 0f), Quaternion.identity);
            UFOBoss.name += GameManager.Instance.bossDefeated.ToString();
            spawnUFOBoss = false;
        }

        //check if Boss has spawned and set the spawn location accordingly
        {
            int tempIndex;
            if (UFOBoss != null)               //works
            {
                tempIndex = Random.Range(0, 7);
                //Debug.Log("Boss Spawned!");
            }
            else                               //normal spawn locations
            {
                tempIndex = Random.Range(0, 15);
            }

            if (possibleToSpawnUfO)            // if possible, spawn UFO
            {
                //Debug.Log("Spawning UFO");
                SpawnIfPossible(tempIndex);
            }
        }

            //spawnUFO(tempIndex);
        
    }

    void SpawnIfPossible(int indexOfLocation)                     //check if another UFO is present at this location, if not the spawn
    {
        Vector3 tempPointToSpawn;                                 //where UFO will spawn
        int plusTwoInY;                                           //spawn +2 in y-axis or not

        //plusTwoInY = Random.Range(1, 2);                        //worng code, alternate between 0 and 2
        plusTwoInY = Random.Range(0, 2) * 2;                      //(fixed) isn't working properly, only output 0, in Int version 2nd value is exclusive

        {
            //plusTwoInY = 2;              // turn off 0.9,3.0
            //indexOfLocation = 11;        // turn off 
        }

        if (UFOBoss != null)               // check if have to spawn under Boss
        {
            //Debug.Log("Boss is here");
            tempPointToSpawn = spawnUnderBossUFO[indexOfLocation].position;
        }
        else
        {
            //adding spawns higher than intended so subtract
            tempPointToSpawn = spawnLocationsUFO[indexOfLocation].position;
        }
        tempPointToSpawn -= new Vector3(0f, plusTwoInY, 0f);

        int tempToggleTwoOrFourInY = 2;        // if enemy spawn in upper 2 layers

        if (plusTwoInY == 2)                     //check if +2 is added to y-axis
        {
            // 15 because the total capacity of spawn location is 15, so any more will automatically become index + 15
            indexOfLocation += 15;
            tempToggleTwoOrFourInY = 4;        // if enemy spawns in lower 2 layers 
            //Debug.Log("plus two!!");
        }

        if (!bufferLocations[indexOfLocation])                   //another UFO is not present
        {
            bufferLocations[indexOfLocation] = true;            // fill buffer, if location is available here

            if (plusTwoInY == 2)
            {
                {   //wrong, got indexing wrong
                    /*
                    if(indexOfLocation - 16 < 0)          //wrong          //check if index is 0 
                    {
                        if(bufferLocations[15])                     // if 0 check index 15
                        {
                            return;
                        }
                    }

                    else if (bufferLocations[indexOfLocation - 16])      // check if upper part conatins UFO
                    {
                        Debug.Log("Present : " + indexOfLocation);
                        return;                                     //if it contains one, then return, else spawn
                    }
                    */
                }
                if (bufferLocations[indexOfLocation - 15])      // check if upper part conatins UFO
                {
                    //Debug.Log("Present : " + indexOfLocation);
                    return;                                     //if it contains one, then return, else spawn
                }

            }
            else
            {
                if (bufferLocations[indexOfLocation + 15])      //check if lower position conatin UFO
                {
                    //Debug.Log("Present : " + indexOfLocation);
                    return;
                }
            }

            //Debug.Log("Spawn Index : " + indexOfLocation);

            {
                //+2f is for right incoming, -2f would be for left incoming, 0f would be for the middle. //toggleTwoPlusOrMinus[tempToggleHolder]
                int tempToggleHolder = Random.Range(0, 3);

                //Debug.Log(tempPointToSpawn + new Vector3(toggleTwoPlusOrMinus[tempToggleHolder], tempToggleTwoOrFourInY, 0f));
                var cloneName = Instantiate(UFOPrefab, tempPointToSpawn + new Vector3(toggleTwoPlusOrMinus[tempToggleHolder], tempToggleTwoOrFourInY, 0f), Quaternion.identity);
                
                cloneName.name = "CloneUFO_" + indexOfLocation;
                cloneName.GetComponent<UFOController>().idUFO = indexOfLocation;
                cloneName.GetComponent<UFOController>().RealLocationSet(false, tempPointToSpawn, toggleTwoPlusOrMinus[tempToggleHolder]);
                ++totalUFOSpawned;
                {
                    /*
                    if (cloneName.transform.position != tempPointToSpawn)
                    {
                        if(cloneName.transform.position.x == tempPointToSpawn.x)
                            cloneName.transform.Translate(0f, 0.1f, 0f); 
                        else if(cloneName.transform.position.y == tempPointToSpawn.y)
                            cloneName.transform.Translate(0.1f, 0f, 0f);
                        else
                            cloneName.transform.Translate(0.1f, 0.1f, 0f);
                    }
                    */
                }
            }

            {
                //var teleportClone = Instantiate(GameManager.Instance.teleportPrefab, tempPointToSpawn, Quaternion.identity);
                //StartCoroutine(WaitForSeconds(tempPointToSpawn, indexOfLocation, teleportClone));
            }

            //spawnUFO(tempPointToSpawn);             //old method
        }
    }


    //wait for teleport animation to be over, then spawn a UFO //old function
    /*
    IEnumerator WaitForSeconds(Vector3 tempLocation, int tempIndex, GameObject tempTeleport)
    {
        yield return new WaitForSeconds(1.1f);
        //here
        var cloneName = Instantiate(UFOPrefab, tempLocation, Quaternion.identity);
        cloneName.name = "CloneUFO_" + tempIndex;
        cloneName.GetComponent<UFOController>().idUFO = tempIndex;
        Destroy(tempTeleport);
        //Debug.Log("Destroyed");
    }
    */



    IEnumerator WaitForSeconds2()                  // wait for 2 seconds if all UFO are destroyed
    {
        if (UFOBoss == null)
        {
            enemyWave++;
            //Debug.Log("Increasing Wave : " + enemyWave);
        }  
        yield return new WaitForSeconds(2); 

        if (UFOBoss == null)
        {
            possibleToSpawnUfO = true;
            //Debug.Log("Spawn Ufo : " + possibleToSpawnUfO);
        }
        changeEnemyWaveValue = true;
    }

    public void SetBufferAndUpdateScore(int tempID, int tempScore)              //set false to points, if object is destroyed
    {
        //Debug.Log(tempIndex);
        if (tempID != -1)
        {
            bufferLocations[tempID] = false;
        }
        highScore += tempScore;
        GameManager.Instance.highScoreContainer.text = highScore.ToString();
        if (highScore > 9000 && highScore < 9100)
        {
            GameManager.Instance.highScoreContainer.text = "Its over 9000!!!";
        }
        else if (highScore >= 100000 && highScore < 101000)
        {
            GameManager.Instance.highScoreContainer.text = "Too Op!!!";
        }
        else if (highScore >= 1000000 && highScore < 1010000)
        {
            GameManager.Instance.highScoreContainer.text = "Masaka!!!";
        }
    }

    public void GameOver()
    {
        SaveScript.SaveHighScore(highScore);
    }

    /*
    public void StartSpawningUFO()           //all animation has ended and start spawning UFO
    {
        possibleToSpawnUfO = true;
    }
    */
}
