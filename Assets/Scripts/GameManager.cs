using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int comboPoints;
    public int spawnCounter = 0;

    public int balloonCounter = 0;

    public bool isSpawning = false;

    public GameObject combosText;

    public GameObject dialogueBox;

    public GameObject gameOverPanel;
    public GameObject spawnedObjectParent;

    public MainMenu mainMenu;

    public TMP_Text dialogue;
    public string[] lines;
    public float textSpeed;

    public int lineIndex;

    public GameObject player;
    public GameObject[] targets;

    public MovingWalls movingWalls1, movingWalls2;
    GameObject movingWalls1Obj, movingWalls2Obj;
    public BoxCollider spawnerCollider;

    public bool gameStarted;
    public bool isLose = false;
    public bool isWin = false;


    private void Start()
    {
        gameStarted = false;

        movingWalls1Obj = movingWalls1.gameObject;
        movingWalls2Obj = movingWalls2.gameObject;
    }

    private void Update()
    {
        StartCoroutine(checkLose());

        combosText.GetComponent<TMP_Text>().text = comboPoints.ToString();

        targets = GameObject.FindGameObjectsWithTag("Target");

        countBalloons();

        if (dialogueBox.activeInHierarchy == true)
        {
            player.GetComponent<Player>().readyToThrow = false;

            if (Input.GetMouseButtonDown(0))
            {
                if (dialogue.text == lines[lineIndex])
                {
                    nextLine();
                }
                else
                {
                    StopAllCoroutines();
                    dialogue.text = lines[lineIndex];
                }
            }
        }
        else
        {
            player.GetComponent<Player>().readyToThrow = true;
        }

        if (isWin == true)
        {
            mainMenu.backToMenu();
        }

        if ((movingWalls1Obj.transform.localPosition - movingWalls1.originalPos).magnitude < 1f)
        {
            movingWalls1Obj.transform.localPosition = movingWalls1.originalPos;
        }
        if ((movingWalls2Obj.transform.localPosition - movingWalls1.originalPos).magnitude > 30f)
        {
            movingWalls2Obj.transform.localPosition = movingWalls2.originalPos;
        }
    }

    public void addCombo()
    {
        comboPoints++;
    }

    public void resetCombo()
    {
        if (comboPoints > 0)
        {
            comboPoints = 0;
        }
    }

    public void addThrowNumber(int number)
    {
        player.GetComponent<Player>().totalThrow += number;
    }

    void countBalloons()
    {
        if (balloonCounter == 5)
        {
            Debug.Log("win");
            isSpawning = false;
            balloonCounter = 0;

            startDialogue(3);
            player.GetComponent<Player>().readyToThrow = true;

            isWin = true;
        }
    }

    public void resetWalls()
    {
        movingWalls1.gameObject.transform.localPosition = movingWalls1.originalPos;
        movingWalls2.gameObject.transform.localPosition = movingWalls2.originalPos;

        movingWalls1.spawnCollider.size = movingWalls1.originalSize;
    }

    public void startDialogue(int index)
    {
        resetWalls();

        dialogueBox.SetActive(true);
        lineIndex = index;

        StartCoroutine(TypeLine());
    }

    public IEnumerator TypeLine()
    {
        foreach (char c in lines[lineIndex].ToCharArray())
        {
            dialogue.text += c;

            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void nextLine()
    {
        if (lineIndex < lines.Length - 1)
        {
            lineIndex++;
            dialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            player.GetComponent<Player>().readyToThrow = true;
        }
    }

    public IEnumerator checkLose()
    {
        if (player.GetComponent<Player>().totalThrow == 0 || isLose == true)
        {
            foreach (Transform child in spawnedObjectParent.transform)
            {
                Destroy(child.gameObject);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            gameOverPanel.SetActive(true);

            yield return new WaitForSeconds(3);

            isLose = true;
            mainMenu.backToMenu();
        }
    }

    public void reset()
    {
        Player playerComp = player.GetComponent<Player>();

        playerComp.totalThrow = 10;
        comboPoints = 0;
        balloonCounter = 0;
    }

    public void pushWalls()
    {
        spawnerCollider.size = new Vector3(spawnerCollider.size.x, spawnerCollider.size.y, 
        spawnerCollider.size.z + 0.01f);

        movingWalls1Obj.transform.localPosition = new Vector3(movingWalls1Obj.transform.localPosition.x,
            movingWalls1Obj.transform.localPosition.y, movingWalls1Obj.transform.localPosition.z - 1);

        movingWalls2Obj.transform.localPosition = new Vector3(movingWalls2Obj.transform.localPosition.x,
            movingWalls2Obj.transform.localPosition.y, movingWalls2Obj.transform.localPosition.z + 1);
    }
}
