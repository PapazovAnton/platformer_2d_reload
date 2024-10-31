using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(CheckCollision))]

public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerMover _playerMover;
    private PlayerAnimator _playerAnimator;
    private CheckCollision _checkCollision;

    private float _horizontalDirection;

    private void OnEnable()
    {
        _playerMover.OnLanding += HandleLanding;
    }

    private void OnDisable()
    {
        _playerMover.OnLanding -= HandleLanding;
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMover = GetComponent<PlayerMover>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _checkCollision = GetComponent<CheckCollision>();
    }

    private void Update()
    {
        _horizontalDirection = _playerInput.HorizontalDirection;

        _playerAnimator.Run(_horizontalDirection);

        _playerMover.SetView(_playerInput.ViewDirection);

        if (_playerInput.TryJump)
            _playerAnimator.Jump();
    }

    private void FixedUpdate()
    {
        _playerMover.Move(_horizontalDirection);

        if (_playerInput.TryJump)
            _playerMover.TryJump();
    }

    private void HandleLanding()
    {
        _playerAnimator.Landing();
    }
}
