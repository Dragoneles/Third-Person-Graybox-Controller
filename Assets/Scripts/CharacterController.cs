// Author:  Joseph Crump
// Date:    06/03/22

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
    [Header("References")]
    [SerializeField]
    private PlayerInputHandler _input;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("How quickly the player moves in any direction.")]
    private float speed = 5f;

    [SerializeField]
    [Tooltip("Maximum pathing angle. Slopes steeper than this will prevent the player from moving up them.")]
    private float maxClimbAngle = 30f;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Interpolant used to angle the character in the camera's direction.")]
    private float turnInterpolant = 0.2f;

    private Rigidbody _rigidbody;
    private Camera _camera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        var cameraController = FindObjectOfType<CameraController>();
        _camera = cameraController.Camera;
    }

    private void OnValidate()
    {
        if (_input == null)
            _input = GetComponentInChildren<PlayerInputHandler>();
    }

    private void Update()
    {
        LookTowardsCamera();
        Move();
    }

    private void Move()
    {
        float strafe = _input.Move.Horizontal;
        float forward = _input.Move.Vertical;

        var direction = new Vector3(strafe, 0f, forward).normalized;
        direction = transform.TransformVector(direction);
        _rigidbody.velocity = direction * speed;
    }

    private void LookTowardsCamera()
    {
        Vector3 forward = GetPlanarForward();
        Vector3 cameraForward = GetCameraPlanarDirection();

        transform.forward = Vector3.Slerp(forward, cameraForward, turnInterpolant);
    }

    private Vector3 GetCameraDirection()
    {
        return _camera.transform.forward;
    }

    // Removes Y elevation from the camera direction
    private Vector3 GetCameraPlanarDirection()
    {
        Vector3 direction = GetCameraDirection();
        direction.y = 0f;
        direction.Normalize();
        return direction;
    }

    // Removed Y elevation from player forward
    private Vector3 GetPlanarForward()
    {
        Vector3 direction = transform.forward;
        direction.y = 0f;
        direction.Normalize();
        return direction;
    }
}
