using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWalls : MonoBehaviour
{
    public Transform playerPos;
    public BoxCollider spawnCollider;

    public GameManager gameManager;

    public Vector3 originalPos;
    public Vector3 originalSize;

    //public float targetX;
    public float targetZ;

    public float speed = 0.1f;

    public float step;

    private void Update()
    {
        step = speed * Time.deltaTime;

        if (gameManager.gameStarted == true)
        {
            if (gameManager.isSpawning == true)
            {
                moveWalls(targetZ);
            }

            resizeCollider(0);
        }
    }

    public void moveWalls(float zNumber)
    {
        Vector3 targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zNumber);

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
    }

    public void resizeCollider(float zNumber)
    {
        Vector3 targetSpawnerSize = new Vector3(spawnCollider.size.x, spawnCollider.size.y, zNumber);

        spawnCollider.size = Vector3.MoveTowards(spawnCollider.size, targetSpawnerSize, step / 10);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.isLose = true;
            StartCoroutine(gameManager.checkLose());
        }
    }
}