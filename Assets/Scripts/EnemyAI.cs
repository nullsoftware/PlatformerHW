using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private PlayerInfo _target;

    public PlayerInfo Target => _target;
    public bool IsAttackMode { get; set; }
}