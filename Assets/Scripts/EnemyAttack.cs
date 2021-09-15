using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _minDamage = 5;
    [SerializeField] private int _maxDamage = 20;
    [SerializeField] private float _attackCooldown = 1.5f;

    private EnemyAI _enemyAI;
    private Collider2D _ownCollider;
    private float _attackTimeStamp;

    private void Start()
    {
        _ownCollider = GetComponent<Collider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void FixedUpdate()
    {
        if (_enemyAI.IsAttackMode && _enemyAI.Target != null && _ownCollider.IsTouching(_enemyAI.Target.Collider))
        {
            TryDamage();
        }
    }

    private void TryDamage()
    {
        if (_attackTimeStamp <= Time.time)
        {
            int dmg = Random.Range(_minDamage, _maxDamage);
            _enemyAI.Target.Stats.ApplyDamage(dmg);
            _attackTimeStamp = Time.time + _attackCooldown;

            Debug.Log($"Damege: {dmg}");
        }
    }
}