﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float _jumpHeight = 8.0f;
    [SerializeField] private float _gravity = 20.0f;
    private CharacterController _characterController;
    private float _horizontalInput, _verticalInput;
    private bool _sprintInput, _jumpInput;
    private Vector3 _direction;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (_characterController == null) Debug.LogError("CharacterController is NULL");
    }

    void Update()
    {
        GetInput();
        CalculateMovement();
    }

    void GetInput() 
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        _sprintInput = Input.GetKey(KeyCode.LeftShift);
         _jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    void CalculateMovement() 
    {
        float directionY = _direction.y;
        _direction = new Vector3(_horizontalInput, 0.0f, _verticalInput).normalized;

        if (_sprintInput && _verticalInput > 0.0f)
        {
            _direction.x = runningSpeed * _direction.x;
            _direction.z = runningSpeed * _direction.z;
        }
        else
        {
            _direction.x = walkingSpeed * _direction.x;
            _direction.z = walkingSpeed * _direction.z;
        }

        if (_jumpInput && _characterController.isGrounded)
        {
            _direction.y = _jumpHeight;
        }
        else
        {
            _direction.y = directionY;
        }

        if (!_characterController.isGrounded)
        {
            _direction.y -= _gravity * Time.deltaTime;
        }

        _direction = transform.TransformDirection(_direction);
        _characterController.Move(_direction * Time.deltaTime);
    }
}
