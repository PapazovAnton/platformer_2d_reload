using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class PlayerAnimator : MonoBehaviour
{
    private const string SpeedHashName = "Speed";
    private const string GroundHashName = "Grounded";
    private const string JumpHashName = "Jump";

    private Animator _animator;

    private int _speedHash;
    private int _groundedHash;
    private int _jumpHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _speedHash = Animator.StringToHash(SpeedHashName);
        _groundedHash = Animator.StringToHash(GroundHashName);
        _jumpHash = Animator.StringToHash(JumpHashName);
    }

    public void Run(float axisHorizontal)
    {
        _animator.SetFloat(_speedHash, Math.Abs(axisHorizontal));
    }

    public void Jump()
    {
        _animator.SetBool(_groundedHash, false);
        _animator.SetBool(_jumpHash, true);
    }

    public void Landing()
    {
        _animator.SetBool(_groundedHash, true);
        _animator.SetBool(_jumpHash, false);
    }
}
