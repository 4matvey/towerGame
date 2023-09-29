using UnityEngine;

public class Block : MonoBehaviour
{
    public bool xAxis; //true when blocks moves along x axis; false when it is z axis
    bool moveAway; //controls wheather block is moving in positive or negative direction
    float maxMovement; //maximum distance block can move
    float movement;    //current distance block has moved
    float movementPerFrame; //distance block moves per frame

    [SerializeField]
    GameObject missedBlock; //portion of block that was not placed onto block under it

    // Start is called before the first frame update
    void Start()
    {
        movementPerFrame = Difficulty.instance.getDifficultySpeed(); //set speed block moves at
        moveAway = false;
        movement = maxMovement = 13f;
        if (xAxis)
            transform.Translate(movement, 0, 0); //x axis block movement
        else
            transform.Translate(0, 0, movement); //z axis block movement
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawner.instance.gameRunning)
            MoveBlock();
    }

    //move the current block foward until it reaches its max disatance,
    //then move it backward until it reaches -max distance, and repeat
    void MoveBlock()
    {
        if (moveAway)
        {
            if (movement < maxMovement)
            {
                if(xAxis)
                    transform.Translate(movementPerFrame, 0, 0);
                else
                    transform.Translate(0, 0, movementPerFrame);
                movement += movementPerFrame;
            }
            else
            {
                moveAway = false;
            }
        }
        else
        {
            if (movement > -maxMovement)
            {
                if (xAxis)
                    transform.Translate(-movementPerFrame, 0, 0);
                else //moveZ
                    transform.Translate(0, 0, -movementPerFrame);
                movement -= movementPerFrame;
            }
            else
            {
                moveAway = true;
            }
        }
    }
    
    //cuts the block based on its placement
    public void CutBlock()
    {
        //perfectly placed block;
        //no need to cut
        if (Mathf.Abs(movement) == 0f)
        {
            if (xAxis)
                transform.Translate(-movement, 0, 0);
            else
                transform.Translate(0, 0, -movement);
        }

        //non perfect block placement
        //need to cut the block (through use of positioning and scaling)
        else
        {
            float missedPortion = Mathf.Abs(movement); //amount of block that cut

            //if block is placed completely off the bottom block, then game over
            if ((xAxis ? transform.localScale.x : transform.localScale.z ) < missedPortion)
            {
                gameObject.AddComponent<Rigidbody>(); //make the block fall with gravity
                Destroy(gameObject, 10f);             //destroy after 10 seconds
                Spawner.instance.End();
                return;
            }

            //some of the block is cut, but not all
            else
            {
                GameObject myMissedBlock = Instantiate(missedBlock);
                float xPosition; //x position of missedBlock
                float zPosition; //

                //if block was moving along x axis
                if (xAxis)
                {
                    //position and scale the cut portion of the block
                    if (movement > 0)
                        xPosition = transform.position.x + (.5f * (transform.localScale.x - missedPortion));
                    else
                        xPosition = transform.position.x - (.5f * (transform.localScale.x - missedPortion));
                    myMissedBlock.transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
                    myMissedBlock.transform.localScale = new Vector3(missedPortion, transform.localScale.y, transform.localScale.z);

                    //position and scale the part of the block that was placed on top of bottom block
                    if (movement > 0)
                        xPosition = transform.position.x - (.5f * missedPortion);
                    else
                        xPosition = transform.position.x + (.5f * missedPortion);
                    this.transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
                    this.transform.localScale -= new Vector3(missedPortion, 0, 0);                   
                }

                //if block was moving along z axis
                else
                {
                    //position and scale the cut portion of the block
                    if (movement > 0)
                        zPosition = transform.position.z + (.5f * (transform.localScale.z - missedPortion));
                    else
                        zPosition = transform.position.z - (.5f * (transform.localScale.z - missedPortion));
                    myMissedBlock.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, missedPortion);
                    myMissedBlock.transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);

                    //position and scale the part of the block that was placed on top of bottom block
                    if (movement > 0)
                        zPosition = transform.position.z - (.5f * missedPortion);
                    else
                        zPosition = transform.position.z + (.5f * missedPortion);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
                    this.transform.localScale -= new Vector3(0, 0, missedPortion);
                }
            }   
        }
        Destroy(this);
    }
}