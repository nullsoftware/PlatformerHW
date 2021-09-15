using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _observeRadius = 5;

    private EnemyAI _enemyAI;
    private Collider2D _ownCollider;
    private Rigidbody2D _rigidbody;
    private Vector3 _leftPoint;
    private Vector3 _rightPoint;
    private Vector3 _currentDirection;
    private bool _isRotated;

    private void Start()
    {
        _ownCollider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();

        RefreshObservePoint();
    }

    private void FixedUpdate()
    {
        if (_enemyAI.IsAttackMode && _enemyAI.Target != null)
        {
            GoToTarget();
        }
        else
        {
            MoveBeetwenPoints();
            SearchForTarget();
        }

        RotateIfNeeded();
    }

    private void GoToTarget()
    {
        if (_ownCollider.IsTouching(_enemyAI.Target.Collider))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }
        else
        {
            _rigidbody.velocity = (_enemyAI.Target.Stats.transform.position - transform.position).normalized * _moveSpeed;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(_ownCollider.bounds.center, _observeRadius);

        if (_enemyAI.Target != null && _enemyAI.Target.Abilities.IsInvisible == false && hits.Any(t => t.gameObject == _enemyAI.Target.gameObject))
        {
            _enemyAI.IsAttackMode = true;
        }
    }

    private void RefreshObservePoint()
    {
        _leftPoint = new Vector3(transform.position.x - _observeRadius, transform.position.y);
        _rightPoint = new Vector3(transform.position.x + _observeRadius, transform.position.y);
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