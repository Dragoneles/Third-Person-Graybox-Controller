// Author:  Joseph Crump
// Date:    07/03/22

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior that processes the player's inputs and moves the player avatar.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    public enum MoveMode
    {
        Default,
        Flight
    }

    [Header("References")]
    [SerializeField]
    private PlayerInputHandler _input;

    [SerializeField]
    private Transform _cameraTarget;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("The initial move state for the controller.")]
    private MoveMode _moveMode = MoveMode.Default;

    [SerializeField]
    [Tooltip("How quickly the player moves in any direction.")]
    private float _speed = 5f;

    [SerializeField]
    [Tooltip("Maximum pathing angle. Slopes steeper than this will prevent the player from moving up them.")]
    private float _maxClimbAngle = 30f;

    [SerializeField]
    private float _minCameraPitch = -80f, _maxCameraPitch = 80f;

    [SerializeField, Range(0f, 1f)]
    private float _pitchInterpolant = 0.2f;

    [SerializeField, Min(0f)]
    private float _pitchSensitivity = 0.5f, _yawSensitivity = 0.8f;

    [SerializeField]
    private bool _invertYaw = false, _invertPitch = false;

    private float _targetPitch = 0f;
    private float TargetPitch
    {
        get => _targetPitch;
        set
        {
            _targetPitch = Mathf.Clamp(value, _minCameraPitch, _maxCameraPitch);
        }
    }

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnValidate()
    {
        if (_input == null)
            _input = GetComponentInChildren<PlayerInputHandler>();
    }

    private void Update()
    {
        if (Application.isFocused == false)
            return;

        Move();
        Look();
    }

    private void OnEnable()
    {
        ShowMouseCursor(false);

        _input.Jump.Pressed += Jump;
        _input.ToggleMoveState.Pressed += ToggleMoveMode;
    }

    private void OnDisable()
    {
        ShowMouseCursor(true);

        _input.Jump.Pressed -= Jump;
        _input.ToggleMoveState.Pressed -= ToggleMoveMode;
    }

    private void OnApplicationFocus(bool focus)
    {
        ShowMouseCursor(!focus);
    }

    private void Move()
    {
        float strafe = _input.Move.Horizontal;
        float forward = _input.Move.Vertical;

        var direction = new Vector3(strafe, 0f, forward).normalized;
        direction = transform.TransformVector(direction);
        _rigidbody.velocity = direction * _speed;
    }

    private void Look()
    {
        float yawDelta = _input.Look.Horizontal * _yawSensitivity;
        yawDelta = (_invertYaw) ? -yawDelta : yawDelta;
        transform.Rotate(0f, yawDelta, 0f);

        float pitchDelta = _input.Look.Vertical * _pitchSensitivity;
        pitchDelta = (_invertPitch) ? -pitchDelta : pitchDelta;

        TargetPitch += pitchDelta;
        var currentRotation = _cameraTarget.localRotation;
        var newRotation = Quaternion.Euler(TargetPitch, currentRotation.y, currentRotation.z);
        _cameraTarget.localRotation = Quaternion.Slerp(currentRotation, newRotation, _pitchInterpolant);
    }

    private void Jump()
    {

    }

    private void ToggleMoveMode()
    {

    }

    private void ShowMouseCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
