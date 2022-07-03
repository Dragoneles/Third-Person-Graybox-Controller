// Author:  Joseph Crump
// Date:    06/03/22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior that processes the player's inputs and moves the player camera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [Header("Pivot Points")]
    [SerializeField]
    private Transform _root;
    [SerializeField]
    private Transform _yaw;
    [SerializeField]
    private Transform _pitch;

    private PlayerInputHandler _input;

    private void Awake()
    {
        _input = transform.root.GetComponentInChildren<PlayerInputHandler>();
    }

    private void OnValidate()
    {
        if (_camera == null)
            _camera = GetComponentInChildren<Camera>();
    }
}
