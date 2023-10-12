using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 0.5f;

    public Vector2 Velocity;
    public LayerMask LayerMask;

    protected Vector2 TargetVelocity;
    protected bool Grounded;
    protected Vector2 GroundNormal;
    protected Rigidbody2D Rb2d;
    protected ContactFilter2D ContactFilter;
    protected RaycastHit2D[] HitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> HitBufferList = new List<RaycastHit2D>(16);

    protected const float MinMoveDistance = 0.01f;
    protected const float ShellRadius = 0.01f;

    private Animator _animator;

    private void OnEnable()
    {
        Rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();

        ContactFilter.useTriggers = false;
        ContactFilter.SetLayerMask(LayerMask);
        ContactFilter.useLayerMask = true;
    }

    private void Update()
    {
        float axisHorizontal = Input.GetAxis("Horizontal");
        _animator.SetFloat("Speed", Math.Abs(axisHorizontal));
        TargetVelocity = new Vector2(axisHorizontal, 0);

        if (Input.GetKey(KeyCode.Space) && Grounded)
        {
            _animator.SetBool("Grounded", false);
            _animator.SetBool("Jump", true);
            Velocity.y = 5;
        }

        if (Input.GetKey(KeyCode.D))
            transform.localScale = new Vector3(1, 1, 1);

        if (Input.GetKey(KeyCode.A))
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        Velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        Velocity.x = TargetVelocity.x;

        Grounded = false;

        Vector2 deltaPosition = Velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    private void Movement(Vector2 move, bool yMovement)
    {
        float speed = (yMovement) ? _speed / 1.5f : _speed; 
        float distance = move.magnitude * speed;

        if (distance > MinMoveDistance)
        {
            int count = Rb2d.Cast(move, ContactFilter, HitBuffer, distance + ShellRadius);

            HitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                HitBufferList.Add(HitBuffer[i]);
            }

            for (int i = 0; i < HitBufferList.Count; i++)
            {
                Vector2 currentNormal = HitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    Grounded = true;
                    _animator.SetBool("Grounded", true);

                    if (yMovement)
                    {
                        GroundNormal = currentNormal;
                        currentNormal.x = 0;
                    } 
                }

                float projection = Vector2.Dot(Velocity, currentNormal);

                if (projection < 0)
                {
                    Velocity = Velocity - projection * currentNormal;
                }

                float modifiedDistance = HitBufferList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        Rb2d.position = Rb2d.position + move.normalized * distance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            coin.Destroy();
        }
    }
}