using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("References")]
    public GameObject destroyEffect;
    public GameManager GameManager;

    private void Update()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            GameObject effect = Instantiate(destroyEffect, spawnPosition, this.transform.rotation);

            Destroy(other.gameObject, 0.2f);
            Destroy(effect, 1.5f);
            Destroy(this.gameObject, 1.5f);

            GameManager.addCombo();
            GameManager.spawnCounter++;
            GameManager.balloonCounter++;

            GameManager.pushWalls();

            this.gameObject.GetComponent<SphereCollider>().enabled = false;

            if (this.gameObject.name == "Balloon")
            {
                GameManager.addThrowNumber(1);
            }
            else if (this.gameObject.name == "Golden Balloon")
            {
                GameManager.addThrowNumber(5);
            }
        }
    }
}

