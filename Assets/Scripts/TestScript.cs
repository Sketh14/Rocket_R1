﻿using UnityEngine;
using System.Collections;
//using UnityEngine.iOS;

// Input.GetTouch example.
//
// Attach to an origin based cube.
// A screen touch moves the cube on an iPhone or iPad.
// A second screen touch reduces the cube size.

public class TestScript: MonoBehaviour
{
    private Vector3 position;
    private float width;
    private float height;

    public int testIncrement;
    public bool testToggle;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        Debug.Log(width);
        height = (float)Screen.height / 2.0f;
        Debug.Log(height);

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2"));
    }

    private void FixedUpdate()
    {
        testIncrement++;

        if(testIncrement == 100)
        {
            TestFunction();
            testIncrement = 0;
        }
    }

    void TestFunction()
    {
        if(testToggle)
        {
            Debug.Log("Returning Early");
            return;
        }

        Debug.Log("Run Till Last"); 
    }

    /*
    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                Debug.Log(pos.x + "   " + pos.y);
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(-pos.x, pos.y, 0.0f);

                // Position the cube.
                transform.position = position;
            }

            if (Input.touchCount == 2)
            {
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    // Halve the size of the cube.
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Restore the regular size of the cube.
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
        }
    }
    */
}