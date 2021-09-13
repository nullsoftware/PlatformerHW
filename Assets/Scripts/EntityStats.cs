using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class EntityStats : MonoBehaviour
{
    private const string DamageAppliedTriggerName = "DamageApplied";

    [SerializeField, Range(1, 1000)] private int _maxHealth = 100;
    [SerializeField] private int _health = 100;

    private Animator _animator;

    public UnityEvent<int> HealthChanged;
    public UnityEvent EntityDied;

    public bool IsDead => _health == 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _health = Mathf.Min(_maxHealth, _health);

        HealthChanged.Invoke(_health);
    }

    public void ApplyDamage(int damage)
    {
        if (damage <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(damage));

        _health = Mathf.Max(0, _health - damage);
        _animator.SetTrigger(DamageAppliedTriggerName);
        HealthChanged?.Invoke(_health);

        if (_health == 0)
        {
            EntityDied?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DeathField>() != null)
        {
            ApplyDamage(_health);
        }
    }

    public static void ReloadLevel()
    {
        ReloadLevelInternal();
    }

    private static async Task ReloadLevelInternal()
    {
        await Task.Delay(1000);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
