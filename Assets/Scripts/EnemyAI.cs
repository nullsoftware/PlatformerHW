using System.Collections;
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
    [SerializeField] private float _attackCooldown = 2.5f;
    [SerializeField] private EntityStats _target;

    private Collider2D _ownCollider;
    private Collider2D _targetCollider;
    private PlayerAbilities _targetAbilities;
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
        _targetCollider = _target.GetComponent<Collider2D>();
        _targetAbilities = _target.GetComponent<PlayerAbilities>();

        RefreshObservePoint();
    }

    private void FixedUpdate()
    {
        if (_isAttackMode && _target != null)
        {
            if (_ownCollider.IsTouching(_targetCollider))
            {
                TryAttack();

                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            }
            else
            {
                _rigidbody.velocity = (_target.transform.position - transform.position).normalized * _moveSpeed;
            }

            if (_targetAbilities.IsInvisible)
            {
                _isAttackMode = false;
            }
        }
        else
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

            // searching for target in observe radius
            Collider2D[] hits = Physics2D.OverlapCircleAll(_ownCollider.bounds.center, _observeRadius);

            if (_target != null && _targetAbilities.IsInvisible == false && hits.Any(t => t.gameObject == _target.gameObject))
            {
                _isAttackMode = true;
            }
        }

        if ((_rigidbody.velocity.x > 0 && !_isRotated) || (_rigidbody.velocity.x <= 0 && _isRotated))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            _isRotated = !_isRotated;
        }
    }

    private void RefreshObservePoint()
    {
        _leftPoint = new Vector3(transform.position.x - _observeRadius, transform.position.y);
        _rightPoint = new Vector3(transform.position.x + _observeRadius, transform.position.y);
    }

    private void TryAttack()
    {
        if (_attackTimeStamp <= Time.time)
        {
            int dmg = Random.Range(_minDamage, _maxDamage);
            _target.ApplyDamage(dmg);
            _attackTimeStamp = Time.time + _attackCooldown;

            Debug.Log($"Damege: {dmg}");
        }
    }
}
