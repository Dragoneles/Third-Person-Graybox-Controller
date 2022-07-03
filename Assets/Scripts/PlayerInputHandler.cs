// Author:  Joseph Crump
// Date:    06/03/22

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Component that handles events dispatched by the <see cref="PlayerInput"/>.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    public class VirtualAxis
    {
        /// <summary>
        /// Current input value of the axis.
        /// </summary>
        public Vector2 Value { get; private set; }

        /// <summary>
        /// X-axis of the vector Value.
        /// </summary>
        public float Horizontal => Value.x;

        /// <summary>
        /// Y-axis of the vector value.
        /// </summary>
        public float Vertical => Value.y;

        public Action<InputAction.CallbackContext> InputCallback => OnInputChanged;

        private void OnInputChanged(InputAction.CallbackContext context)
        {
            Value = context.ReadValue<Vector2>();
        }
    }

    private class CallbackInfo
    {
        public string Action;
        public Action<InputAction.CallbackContext> Callback;
    }

    private PlayerInput _input;

    private readonly VirtualAxis _move = new VirtualAxis();
    private readonly VirtualAxis _look = new VirtualAxis();

    private readonly List<CallbackInfo> registeredCallbacks = new List<CallbackInfo>();

    /// <summary>
    /// The input axis used to move the player character.
    /// </summary>
    public VirtualAxis Move => _move;

    /// <summary>
    /// The input axis used to steer the camera.
    /// </summary>
    public VirtualAxis Look => _look;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        RegisterCallback("Move", Move.InputCallback);
        RegisterCallback("Look", Look.InputCallback);
    }

    private void OnDisable()
    {
        DeregisterAllCallbacks();
    }

    private void RegisterCallback(string action, Action<InputAction.CallbackContext> callback)
    {
        _input.actions[action].started += callback;
        _input.actions[action].performed += callback;
        _input.actions[action].canceled += callback;

        registeredCallbacks.Add(new CallbackInfo { Action = action, Callback = callback });
    }

    private void DeregisterAllCallbacks()
    {
        registeredCallbacks.ForEach(item => DeregisterCallback(item.Action, item.Callback));
    }

    private void DeregisterCallback(string action, Action<InputAction.CallbackContext> callback)
    {
        _input.actions[action].started -= callback;
        _input.actions[action].performed -= callback;
        _input.actions[action].canceled -= callback;
    }
}
