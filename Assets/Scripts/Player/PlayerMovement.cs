using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;

    public float walkSpeed;
    public float runSpeed;
    public float sprintSpeed;

    bool isRunning;
    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            HandleMovement(runSpeed);
            Debug.Log("Speed is run speed.");
        }
        else if (isSprinting)
        {
            HandleMovement(sprintSpeed);
            Debug.Log("Speed is sprint speed.");

        }
        else 
        { 
            HandleMovement(walkSpeed);
            Debug.Log("Speed is walk speed.");

        }

        HandleRunningToggle();
    }

    private void HandleRunningToggle()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isRunning = !isRunning;
            Debug.Log("Walk/Run toggled.");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            Debug.Log("Sprinting");
        }
        else
        {
            isSprinting = false;
        }
    }

    private void HandleMovement(float currentSpeed)
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveVector = transform.right * x + transform.forward * y * currentSpeed;
        moveVector = moveVector.normalized * currentSpeed;

        Debug.Log("Current speed is " + currentSpeed);
        characterController.Move(moveVector * Time.deltaTime);

    }
}
