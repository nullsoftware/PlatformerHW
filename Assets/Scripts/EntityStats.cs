using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class EntityStats : MonoBehaviour
{
    private const string DeathFieldTagName = "DeathField";
    private const string DamageAppliedTriggerName = "DamageApplied";

    private Animator _animator;    
    [SerializeField, Range(1, int.MaxValue)] private int _maxHealth = 100;
    [SerializeField] private int _health = 100;

    public UnityEvent<int> HealthChangedEvent;
    public UnityEvent EntityDiedEvent;

    public bool IsDead => _health == 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _health = Mathf.Min(_maxHealth, _health);

        HealthChangedEvent.Invoke(_health);
    }

    public void ApplyDamage(int damage)
    {
        if (damage <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(damage));

        _health = Mathf.Max(0, _health - damage);
        _animator.SetTrigger(DamageAppliedTriggerName);
        HealthChangedEvent.Invoke(_health);

        if (_health == 0)
        {
            EntityDiedEvent.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == DeathFieldTagName)
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
