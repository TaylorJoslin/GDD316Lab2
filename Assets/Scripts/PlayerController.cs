using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 InputKey;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;



    void Start()
    {
        //gets players rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //allows player to move with WASD
        InputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //how fast the player moves
        rb.velocity = InputKey * speed;

        if (InputKey.magnitude > 0.01f) // Check if the player is moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(InputKey.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

    }

    private void FixedUpdate()
    {
        
    }
}
