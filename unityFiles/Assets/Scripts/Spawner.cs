using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    List<GameObject> tower; //list of block gameObjects
    public bool gameRunning;
    public static Spawner instance;

    [SerializeField]
    GameObject startButton, _camera, block, beginningBlock, backgroundController, score;

    //The very first function called when this script starts
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameRunning = false;
        tower = new List<GameObject>();
        tower.Add(beginningBlock);
    }

    // Update is called once per frame
    void Update()
    {
        //if game not operating yet, leave
        if (!gameRunning) return;

        //game is currently operating;
        //everytime the user clicks, cut the block if it is placed incorrectly.
        //then move the background and update score
        if (Input.GetMouseButtonDown(0))
        {
            if (tower.Count > 1)
                tower[tower.Count - 1].GetComponent<Block>().CutBlock();
            if (!gameRunning) return;
            StartCoroutine(_camera.GetComponent<CameraController>().MoveCamera());
            StartCoroutine(backgroundController.GetComponent<Background>().moveMountains());
            StartCoroutine(backgroundController.GetComponent<Background>().transitionToSpace());
            score.GetComponent<TMP_Text>().text = (tower.Count - 1).ToString();

            //create a new block, and the loop starts again
            CreateBlock();
        }
    }

    //start the game
    public void Begin()
    {             
        gameRunning = true;
        CreateBlock();
    }

    //end the game
    public void End()
    {
        gameRunning = false;
        startButton.SetActive(true);
    }

    //reload the game (after player loses)
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //create a new block with the same dimensions and position as the block under it
    void CreateBlock()
    {
        GameObject activeBlock = Instantiate(block);
        GameObject previousBlock = tower[tower.Count - 1];

        tower.Add(activeBlock);

        if (tower.Count > 2)
            activeBlock.transform.localScale = previousBlock.transform.localScale;

        float yPosition = previousBlock.transform.position.y + previousBlock.transform.localScale.y;
        activeBlock.transform.position = new Vector3(previousBlock.transform.position.x, yPosition , previousBlock.transform.position.z);

        //alternate between x and z axis movement for different blocks
        activeBlock.GetComponent<Block>().xAxis = tower.Count % 2 == 0;
    }
}