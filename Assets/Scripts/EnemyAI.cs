﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private int _minDamage = 5;
    [SerializeField] private int _maxDamage = 20;
    [SerializeField] private float _observeRadius = 5;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private PlayerInfo _target;

    private Collider2D _ownCollider;
    private Rigidbody2D _rigidbody;
    private bool _isAttackMode;
    private Vector3 _leftPoint;
    private Vector3 _rightPoint;
    private Vector3 _currentDirection;
    private float _attackTimeStamp;
    private bool _isRotated;

    private void Start()
    {
        _ownCollider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        RefreshObservePoint();
    }

    private void FixedUpdate()
    {
        if (_isAttackMode && _target != null)
        {
            AttackTarget();
        }
        else
        {
            MoveBeetwenPoints();
            SearchForTarget();
        }

        RotateIfNeeded();
    }

    private void RefreshObservePoint()
    {
        _leftPoint = new Vector3(transform.position.x - _observeRadius, transform.position.y);
        _rightPoint = new Vector3(transform.position.x + _observeRadius, transform.position.y);
    }

    private void TryDamage()
    {
        if (_attackTimeStamp <= Time.time)
        {
            int dmg = Random.Range(_minDamage, _maxDamage);
            _target.Stats.ApplyDamage(dmg);
            _attackTimeStamp = Time.time + _attackCooldown;

            Debug.Log($"Damege: {dmg}");
        }
    }

    private void AttackTarget()
    {
        if (_ownCollider.IsTouching(_target.Collider))
        {
            TryDamage();

            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }
        else
        {
            _rigidbody.velocity = (_target.Stats.transform.position - transform.position).normalized * _moveSpeed;
        }
    }

    private void MoveBeetwenPoints()
    {
        if (_currentDirection == Vector3.zero)
        {
            _currentDirection = _leftPoint;
        }

        if (Vector2.Distance(transform.position, _currentDirection) < 1)
        {
            _currentDirection = (_currentDirection == _rightPoint) ? _leftPoint : _rightPoint;
        }

        _rigidbody.velocity = (_currentDirection - transform.position).normalized * _moveSpeed;
    }

    private void SearchForTarget()
    {
        // searching for target in observe radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(_ownCollider.bounds.center, _observeRadius);

        if (_target != null && _target.Abilities.IsInvisible == false && hits.Any(t => t.gameObject == _target.gameObject))
        {
            _isAttackMode = true;
        }
    }

    private void RotateIfNeeded()
    {
        if ((_rigidbody.velocity.x > 0 && !_isRotated) || (_rigidbody.velocity.x <= 0 && _isRotated))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _isRotated = !_isRotated;
        }
    }
}
