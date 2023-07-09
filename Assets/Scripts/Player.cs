using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public GameObject[] projectileToDestroy;
    public GameObject weaponObj;
    public GameObject[] projectiles;
    public Mesh[] meshes;

    public GameManager gameManager;

    public TMP_Text textThrowLeft;

    [Header("Settings")]
    public int totalThrow;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    public bool readyToThrow = false;

    // [Header("Movement")]

    // public float moveSpeed;
    // public float groundDrag;
    // public Transform orientation;

    // float horizontalInput;
    // float verticalInput;

    // Vector3 moveDirection;

    // public Rigidbody rb;

    // [Header("Ground Check")]
    // public float playerHeight;
    // public LayerMask whatIsGround;
    // bool grounded;

    private void Start()
    {
        // rb.freezeRotation = true;
    }

    private void Update()
    {
        // grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // MoveInput();
        // SpeedControl();

        // if (grounded)
        // {
        //     rb.drag = groundDrag;
        // }
        // else
        // {
        //     rb.drag = 0;
        // }

        textThrowLeft.GetComponent<TMP_Text>().text = totalThrow.ToString();

        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrow > 0)
        {
            Throw();
            randomizeWeapon();
        }

        projectileToDestroy = GameObject.FindGameObjectsWithTag("Weapon");

        if (projectileToDestroy != null)
        {
            foreach (GameObject go in projectileToDestroy)
            {
                float distance = Vector3.Distance(this.transform.position, go.transform.position);

                if (distance > 100)
                {
                    Destroy(go);
                }
                else
                {
                    Destroy(go, 5);
                }
            }
        }
    }

    void Throw()
    {
        readyToThrow = false;

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
        projectile.tag = "Weapon";

        ThrowWeapon throwWeapon = projectile.AddComponent<ThrowWeapon>() as ThrowWeapon;

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrow--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    void ResetThrow()
    {
        readyToThrow = true;
    }

    // private void FixedUpdate()
    // {
    //     MovePlayer();
    // }

    // void MoveInput()
    // {
    //     horizontalInput = Input.GetAxisRaw("Horizontal");
    //     verticalInput = Input.GetAxisRaw("Vertical");
    // }

    // void MovePlayer()
    // {
    //     moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

    //     rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    // }

    // void SpeedControl()
    // {
    //     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //     if (flatVel.magnitude > moveSpeed)
    //     {
    //         Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //         rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    //     }
    // }

    public void randomizeWeapon()
    {
        GameObject pickObj = projectiles[Random.Range(0, projectiles.Length)];

        objectToThrow = pickObj;

        foreach (Mesh mesh in meshes)
        {
            if (mesh.name == pickObj.name)
            {
                weaponObj.GetComponent<MeshFilter>().mesh = mesh;
            }
        }
    }
}
