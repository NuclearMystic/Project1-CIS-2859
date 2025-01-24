using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        HandleMovement();
        HandleRunningToggle();
    }

    private void HandleRunningToggle()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isRunning = !isRunning;
        }
    }

    private void HandleMovement()
    {
        throw new NotImplementedException();
    }
}
