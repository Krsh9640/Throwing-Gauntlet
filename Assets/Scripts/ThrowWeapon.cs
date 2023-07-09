using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public GameManager GameManager;

    private void Update()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Buildings")
        {
            GameManager.resetCombo();
        }
    }
}
