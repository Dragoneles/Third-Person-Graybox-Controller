// Author:  Joseph Crump
// Date:    06/03/22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior that processes the player's inputs and moves the player avatar.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputHandler _input;

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
        
    }
}
