using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject boxObj, vaseObj, barrelObj, barrierObj, coneObj, targetDummyObj, plankObj, balloonObj, goldenBalloonObj;
    public BoxCollider boxCollider;
    public int boxNum, vaseNum, barrelNum, barrierNum, coneNum, targetDummyNum, plankNum, balloonNum;

    public GameManager gameManager;

    public GameObject spawnedObjectParent;

    public MovingWalls movingWalls1, movingWalls2;

    public bool firstTimeSpawn = false;

    private void Update()
    {
        if (gameManager.isSpawning == false && firstTimeSpawn == false && gameManager.isWin == false)
        {
            firstTimeSpawn = true;
            gameManager.isSpawning = true;
        }
        else if (gameManager.isSpawning == true)
        {
            firstTimeSpawn = false;
        }

        if (gameManager.gameStarted == true)
        {
            if (firstTimeSpawn == true)
            {
                spawnObject(boxObj, boxNum);
                spawnObject(vaseObj, vaseNum);
                spawnObject(barrelObj, barrelNum);
                spawnObject(barrelObj, barrierNum);
                spawnObject(coneObj, coneNum);
                spawnObject(targetDummyObj, targetDummyNum);
                spawnObject(plankObj, plankNum);
                spawnGoldenBalloon();

                spawnedObjectParent.SetActive(false);

                firstTimeSpawn = false;
            }

            spawnedObjectParent.SetActive(true);

            if (gameManager.spawnCounter == 5 && gameManager.isSpawning == true)
            {
                spawnGoldenBalloon();
                addObject();
            }
        }
    }

    void spawnObject(GameObject go, int spawnNumber)
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
            0.5f, Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z));

            Quaternion randomRotation = new Quaternion(Random.rotation.x, UnityEngine.Random.rotation.y, transform.rotation.z, 1);

            GameObject gi = GameObject.Instantiate(go, spawnPosition, randomRotation);
            gi.name = go.name;
            gi.transform.SetParent(spawnedObjectParent.transform);
        }
    }

    void addObject()
    {
        GameObject[] objs = new GameObject[7] { boxObj, vaseObj, barrelObj, barrierObj, coneObj, targetDummyObj, plankObj };

        GameObject pickObjs = objs[Random.Range(0, objs.Length + 1)];

        spawnObject(pickObjs, 1);
    }

    void spawnGoldenBalloon()
    {
        gameManager.spawnCounter = 0;

        int chances = Random.Range(1, 101);
        int lastNumber = chances % 10;

        movingWalls1.speed += 0.005f;
        movingWalls2.speed += 0.005f;

        if (lastNumber == 9)
        {
            spawnObject(goldenBalloonObj, 1);
            spawnObject(balloonObj, 4);
        }
        else
        {
            spawnObject(balloonObj, balloonNum);
        }
    }
}
