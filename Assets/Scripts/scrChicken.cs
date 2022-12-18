using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class scrChicken : MonoBehaviour
{
    bool isJumping = false;
    bool isJumpingForward = false;
    bool isJumpingBackward = false;
    bool isJumpingLeft = false;
    bool isJumpingRight = false;

    public GameObject strip1;
    public GameObject strip2;
    public GameObject strip3;

    public GameObject strip4;
    public GameObject strip5;
    public GameObject strip6;

    public GameObject strip7;
    public GameObject strip8;
    public GameObject strip9;

    public GameObject strip10;
    public GameObject strip11;
    public GameObject strip12;

    public GameObject strip13;

    public GameObject chickenDefault;

    // Street is index 0    Rocks is index 1,2     Tree is index 3,4    Plain is index 5,6
    
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

    float deltaSidewaysDistance = 0.0f;
    float deltaDistance = 0.0f;
    float deltaMidwayDistance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        isJumping = false; // double duty

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
        strips.Add(strip10);
        strips.Add(strip11);
        strips.Add(strip12);
        strips.Add(strip13);



        indexCurrentStrip = 0;

        // Later create new roads and auto generate as needed
        // https://www.youtube.com/watch?v=75zrva79Gmw

        // determine the distance from one strip to the next --> sideways distance
        // forward and backwards

        deltaSidewaysDistance = strips[1].transform.position.z - strips[0].transform.position.z/2.0f;
        deltaDistance = strips[1].transform.position.z - strips[0].transform.position.z/2.0f;
        deltaMidwayDistance = deltaDistance / 2.0f;

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

        // keyboard entry movement

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isJumping)
        {
            Debug.Log("Left Arrow Key");

            if(!isJumpingLeft)
            {
                isJumpingLeft = true;
                isJumping = true;
                jumpLeftSetup();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            Debug.Log("Up Arrow Key");

            if (!isJumpingForward)
            {
                isJumpingForward = true;
                isJumping = true;
                jumpForwardSetup();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isJumping)
        {
            Debug.Log("Right Arrow Key");

            if (!isJumpingRight)
            {
                isJumpingRight = true;
                isJumping = true;
                jumpRightSetup();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isJumping)
        {
            Debug.Log("Down Arrow Key");

            if (!isJumpingBackward && indexCurrentStrip != 0)
            {
                isJumpingBackward = true;
                isJumping = true;
                jumpBackwardSetup();
            }
        }

        if (isJumpingForward)
        {
            // rotate chicken to point forward   ----  rotation in y axis 0 is forward 180 is backward
            chickenDefault.transform.localEulerAngles = new Vector3(0.0f,0.0f,0.0f);

            if(this.transform.position.z < deltaMidwayDistance)
            {
                // move forward and up
                this.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y + jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z + (movingSpeed * Time.deltaTime));
            }
            else if (this.transform.position.z <= jumpTargetLocation.z)
            {
                // falling down but moving forward
                this.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y - jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z + (movingSpeed * Time.deltaTime));
            }
            else
            {
                // bring chicken to ground at jumpTargetLocation
                this.transform.position = jumpTargetLocation;
                isJumpingForward = false;
                isJumping = isJumpingBackward && isJumpingForward && isJumpingLeft && isJumpingRight; // basically only turns isJumping false if no direction is true

            }
        } // <\isJumpingForward>
        else if (isJumpingBackward)
        {
            // rotate chicken to point backward   ----  rotation in y axis 0 is forward 180 is backward
            chickenDefault.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

            if (this.transform.position.z > deltaMidwayDistance)
            {
                // move Backward and up
                this.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y + jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z - (movingSpeed * Time.deltaTime));
            }
            else if (this.transform.position.z >= jumpTargetLocation.z)
            {
                // falling down but moving left
                this.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y - jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z - (movingSpeed * Time.deltaTime));
            }
            else
            {
                // bring chicken to ground at jumpTargetLocation
                this.transform.position = jumpTargetLocation;
                isJumpingBackward = false;
                isJumping = isJumpingBackward && isJumpingForward && isJumpingLeft && isJumpingRight; // basically only turns isJumping false if no direction is true

            }
        } // <\isJumpingBackward>
        else if (isJumpingLeft)
        {
            // rotate chicken to point Left   ----  rotation in y axis 0 is forward 180 is backward
            chickenDefault.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);

            if (this.transform.position.x > deltaMidwayDistance)
            {
                // move left and up
                this.transform.position = new Vector3(this.transform.position.x - (movingSpeed * Time.deltaTime),
                    this.transform.position.y + jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z);
            }
            else if (this.transform.position.x >= jumpTargetLocation.x)
            {
                // falling down but moving left
                this.transform.position = new Vector3(this.transform.position.x - (movingSpeed * Time.deltaTime),
                    this.transform.position.y - jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z);
            }
            else
            {
                // bring chicken to ground at jumpTargetLocation
                this.transform.position = jumpTargetLocation;
                isJumpingLeft = false;
                isJumping = isJumpingBackward && isJumpingForward && isJumpingLeft && isJumpingRight; // basically only turns isJumping false if no direction is true
            }
        } // <\isJumpingLeft>
        else if (isJumpingRight)
        {
            // rotate chicken to point right   ----  rotation in y axis 0 is forward 180 is backward
            chickenDefault.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);

            if (this.transform.position.x < deltaMidwayDistance)
            {
                // move left and up
                this.transform.position = new Vector3(this.transform.position.x + (movingSpeed * Time.deltaTime),
                    this.transform.position.y + jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z);
            }
            else if (this.transform.position.x <= jumpTargetLocation.x)
            {
                // falling down but moving left
                this.transform.position = new Vector3(this.transform.position.x + (movingSpeed * Time.deltaTime),
                    this.transform.position.y - jumpHeightIncrement * Time.deltaTime,
                    this.transform.position.z);
            }
            else
            {
                // bring chicken to ground at jumpTargetLocation
                this.transform.position = jumpTargetLocation;
                isJumpingRight = false;
                isJumping = isJumpingBackward && isJumpingForward && isJumpingLeft && isJumpingRight; // basically only turns isJumping false if no direction is true
            }
        } // <\isJumpingRight>

       /* if (isJumping)
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
        }*/
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

    private void jumpLeftSetup()
    {
        // calculate the destination of the landing point in the left direction (uses -)
        jumpTargetLocation = new Vector3(this.transform.position.x - deltaDistance, strips[indexCurrentStrip].transform.position.y, this.transform.position.z);

        // calculate the halfway point of the chicken for moving up and down
        deltaMidwayDistance = (this.transform.position.x + jumpTargetLocation.x) / 2.0f;
    }

    private void jumpRightSetup()
    {
        // calculate the destination of the landing point in the left direction (uses +)
        jumpTargetLocation = new Vector3(this.transform.position.x + deltaDistance, strips[indexCurrentStrip].transform.position.y, this.transform.position.z);

        // calculate the halfway point of the chicken for moving up and down
        deltaMidwayDistance = (this.transform.position.x + jumpTargetLocation.x) / 2.0f;


    }

    void jumpForwardSetup()
    {
        // add strips current index
        indexCurrentStrip += 1;

        // get z position of new strip --> apply chicken
        jumpTargetLocation = new Vector3(this.transform.position.x, 
            strips[indexCurrentStrip + 1].transform.position.y, 
            this.transform.position.z + deltaDistance);
        deltaMidwayDistance = (this.transform.position.z + jumpTargetLocation.z) / 2.0f;

        // instantiate a new strip...can be done later
        // spawnNewStrip();
    }

    void jumpBackwardSetup()
    {
        // as we jump back we need previous strips height *except when we jump to the big green patch
        if(indexCurrentStrip > 1)
        {
            // decrement current index
            indexCurrentStrip -= 1;

            jumpTargetLocation = new Vector3(this.transform.position.x,
                strips[indexCurrentStrip - 1].transform.position.y,
                this.transform.position.z - deltaDistance);
            deltaMidwayDistance = (this.transform.position.z) - (deltaDistance / 2.0f);
        }
        else if (indexCurrentStrip == 1) // we cannot jump back onto the start patch
        {
            //decrement index
            indexCurrentStrip -= 1;

            jumpTargetLocation = new Vector3(this.transform.position.x,
                strips[0].transform.position.y,
                strips[0].transform.position.z);
            deltaMidwayDistance = this.transform.position.z - (deltaDistance / 2.0f);

        }

    }

    void spawnNewStrip()
    {
        // we take a strip from the pool of prefab strips randomly
        int stripsPrefabCount = poolOfStripsPrefabs.Length;
        int randomNumber = UnityEngine.Random.Range(0, stripsPrefabCount);

        // alternate between road(s) and grass strip(s). with continuation
        if(isAGrassStripToBeDisplayed)
        {
            // alternate between random selection from plain to occupied
            if(indexOfPreviousGrassStrip >= indexStartOfPlainGrass) // since plain grass is index 5 and 6
            {
                randomNumber = UnityEngine.Random.Range(indexEndOfStreet, indexEndOfTreeOccupied); // all the other strips
            }
            else //insert a bare strip
            {
                randomNumber = UnityEngine.Random.Range(indexStartOfPlainGrass, stripsPrefabCount);
            }
            
            indexOfPreviousGrassStrip = randomNumber;
            numOfGrassStripsToDisplay -= 1;

            if (numOfGrassStripsToDisplay == 0)
            {
                isAGrassStripToBeDisplayed = false; // next strip to be displayed must be a road strip
                numOfGrassStripsToDisplay = UnityEngine.Random.Range(1, maxNumGrass);
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
                numOfRoadStripsToDisplay = UnityEngine.Random.Range(1, maxNumRoads);
            }
            else
            {
                isAGrassStripToBeDisplayed = false;

            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision with 'Enemy tag.'");

            updateDeathAnimation();
        }
    }

    private void updateDeathAnimation()
    {
        transform.localScale = new Vector3(1.0f, 0.14f, 1.0f);
    }
}
