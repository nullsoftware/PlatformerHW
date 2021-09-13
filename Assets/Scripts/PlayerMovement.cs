using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 4f;
    [SerializeField] private float _jumpForce = 14f;
    [SerializeField] private LayerMask _platformLayerMask;

    private Animator _playerAnimator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;

    private bool IsRunning
    {
        get => _playerAnimator.GetBool(nameof(IsRunning));
        set => _playerAnimator.SetBool(nameof(IsRunning), value); 
    }

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        bool isJumpRequested = Input.GetButton("Jump") && IsGrounded();

        _rigidbody.velocity = new Vector2(movX * _runSpeed, isJumpRequested ? _jumpForce : _rigidbody.velocity.y);

        IsRunning = movX != 0;

        if ((movX < 0 && transform.localScale.x > 0) || (movX > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);   
    }

    private bool IsGrounded()
    {
        Collider2D result = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, .1f).collider;

        return result != null && !result.isTrigger;
    }
}
