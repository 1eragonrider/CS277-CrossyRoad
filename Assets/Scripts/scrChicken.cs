using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrChicken : MonoBehaviour
{

    public GameObject strip1;
    public GameObject strip2;
    public GameObject strip3;
    public GameObject strip4;
    public GameObject strip5;
    public GameObject strip6;
    public GameObject strip7;
    public GameObject strip8;
    public GameObject strip9;

    // Street is index 0    Rocks is index 1,2     Tree is index 3,4    Plain is index 5,69

    bool isJumping = false;
    public GameObject[] poolOfStripsPrefabs;
    const int maxNumRoads = 2;
    const int maxNumGrass = 2;
    int indexCurrentStrip;
    
    int indexEndOfStreet = 0;
    
    int indexStartOfRockOccupied = 1;
    int indexEndOfRockOccupied = 2;

    int indexStartOfTreeOccupied = 3;
    int indexEndOfTreeOccupied = 4;

    int indexStartOfPlainGrass = 5;

    bool isplayingDeathAnimation = false;
    static int indexOfPreviousGrassStrip;
    static bool isAGrassStripToBeDisplayed = false; // otherwise road strip to be diplayed
    static int numOfRoadStripsToDisplay;
    static int numOfGrassStripsToDisplay;

    private List<GameObject> strips;
    public Vector3 jumpTargetLocation;
    public float movingSpeed = 50.0f;
    public float jumpHeightIncrement = 5.0f;
    private float midwayPointZ;

    // Start is called before the first frame update
    void Start()
    {
        strips = new List<GameObject>();
        strips.Add(strip1);
        strips.Add(strip2);
        strips.Add(strip3);
        strips.Add(strip4);
        strips.Add(strip5);
        strips.Add(strip6);
        strips.Add(strip7);
        strips.Add(strip8);
        strips.Add(strip9);

        indexCurrentStrip = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //mouse click jump
        if(Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Mouse Down");
            if (!isJumping)
            {   
                isJumping = true;
                jump();
            }
        }

        if (isJumping)
        {
            // jump up and move forward
            if(this.transform.position.z < midwayPointZ)
            {
                this.transform.position = new Vector3(this.transform.position.x, 
                    this.transform.position.y + jumpHeightIncrement*Time.deltaTime, 
                    this.transform.position.z + movingSpeed*Time.deltaTime);
            }
            // falling down still moving forwards
            else if (this.transform.position.z <= jumpTargetLocation.z)
            {
                this.transform.position = new Vector3(this.transform.position.x,
                   this.transform.position.y - jumpHeightIncrement * Time.deltaTime,
                   this.transform.position.z + movingSpeed * Time.deltaTime);
            }
            // make the chicken flat on ground and at jumpTargetLocation
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, 
                    strips[indexCurrentStrip].transform.position.y,
                    jumpTargetLocation.z);

                isJumping = false;
            }
        }
    }

    private void jump()
    {
        // we want to move the chicken to the next strip
        // Get location of next strip --> move chicken to that location
        // this.transform.position = new Vector3(transform.position.x, transform.position.y,strip2.transform.position.z);


        // increment the strips current index by 1
        indexCurrentStrip += 1;

        // get the strip at the index within the strips list
        GameObject nextStrip = strips[indexCurrentStrip] as GameObject;
        
        // get the z-pos of the new strip and apply to the chicken
        jumpTargetLocation = new Vector3(transform.position.x, transform.position.y, nextStrip.transform.position.z);

        midwayPointZ = (this.transform.position.z + jumpTargetLocation.z) / 2;
    }


    void spawnNewStrip()
    {
        // we take a strip from the pool of prefab strips randomly
        int stripsPrefabCount = poolOfStripsPrefabs.Length;
        int randomNumber = Random.Range(0, stripsPrefabCount);

        // alternate between road(s) and grass strip(s). with continuation
        if(isAGrassStripToBeDisplayed)
        {
            // alternate between random selection from plain to occupied
            if(indexOfPreviousGrassStrip >= indexStartOfPlainGrass) // since plain grass is index 5 and 6
            {
                randomNumber = Random.Range(indexEndOfStreet, indexEndOfTreeOccupied); // all the other strips
            }
            else //insert a bare strip
            {
                randomNumber = Random.Range(indexStartOfPlainGrass, stripsPrefabCount);
            }
            
            indexOfPreviousGrassStrip = randomNumber;
            numOfGrassStripsToDisplay -= 1;

            if (numOfGrassStripsToDisplay == 0)
            {
                isAGrassStripToBeDisplayed = false; // next strip to be displayed must be a road strip
                numOfGrassStripsToDisplay = Random.Range(1, maxNumGrass);
            }
            else
            {
                isAGrassStripToBeDisplayed = true;
            }
        }
        else // insert road strip
        {
            // this is where my code differs fundementaly from Dr. Solis
            // https://www.youtube.com/watch?v=5dxyJRb1TD0 and https://www.youtube.com/watch?v=rsppyiUT7N4

            randomNumber = 0;  // not really random, but there is only 1 road strip at index 0
            numOfRoadStripsToDisplay -= 1;
            if (numOfRoadStripsToDisplay == 0)
            {
                isAGrassStripToBeDisplayed = true;
                numOfRoadStripsToDisplay = Random.Range(1, maxNumRoads);
            }
            else
            {
                isAGrassStripToBeDisplayed = false;

            }
        }

    }
}
