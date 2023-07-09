using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject PnelsOption;
    public GameObject cup;
    public GameObject gameplayScene, mainMenuScene;
    public GameManager gameManager;

    public Camera menuCam, gameplayCam;

    public Vector3 menuCamOriginPos, gameplayCamOriginPos;

    public bool playPressed = false;

    private void Start()
    {
        menuCamOriginPos = menuCam.gameObject.transform.position;
        gameplayCamOriginPos = gameplayCam.gameObject.transform.position;
    }

    private void Update()
    {
        buttonPressed();

        if (playPressed == true)
        {
            cameraMove();
        }

        if (gameManager.lineIndex == 2 && gameManager.dialogueBox.activeInHierarchy == true)
        {
            gameManager.lineIndex = 3;
            StopAllCoroutines();

            gameManager.dialogue.text = gameManager.lines[gameManager.lineIndex];
            gameManager.dialogueBox.SetActive(false);

            gameManager.isSpawning = false;
            gameManager.gameStarted = true;
        }

        if (gameManager.isWin == true)
        {
            cup.SetActive(true);
        }

        if (gameplayCam.gameObject.activeInHierarchy == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void buttonPressed()
    {
        if (mainMenuScene.activeInHierarchy == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 200f))
                {
                    if (hit.transform != null)
                    {
                        if (hit.transform.name == "BtnPlay")
                        {
                            playPressed = true;
                        }
                        else if (hit.transform.name == "BtnClose")
                        {
                            PnelsOption.SetActive(false);
                        }
                        else if (hit.transform.name == "BtnExit")
                        {
                            Application.Quit();
                        }
                    }
                }
            }
        }
    }

    void cameraMove()
    {
        gameplayScene.SetActive(true);

        if (playPressed == true)
        {
            gameManager.startDialogue(0);
            gameManager.isSpawning = true;

            playPressed = false;
            
            gameplayCam.enabled = true;
            menuCam.enabled = false;


            mainMenuScene.SetActive(false);
        }
    }

    public void backToMenu()
    {
        mainMenuScene.SetActive(true);

        gameManager.reset();

        if (gameManager.isLose == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            menuCam.enabled = true;
            gameplayCam.enabled = false;

            gameManager.gameOverPanel.SetActive(false);

            playPressed = false;
            gameManager.isLose = false;
            gameManager.gameStarted = false;

            gameplayScene.SetActive(false);
        }
    }
}
