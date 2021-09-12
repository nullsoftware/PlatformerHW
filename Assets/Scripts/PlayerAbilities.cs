using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private float _invisibilityCooldown = 15f;
    [SerializeField] private float _invisibilityDuration = 7f;

    private SpriteRenderer _sprite;
    private float _invisibilityTimeStamp;
    private bool _isInvisible;

    public bool IsInvisible => _isInvisible;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (CanActivateInvisibility() && Input.GetKey(KeyCode.E))
            ActivateInvisibility();
    }

    private bool CanActivateInvisibility()
    {
        return _invisibilityTimeStamp <= Time.time;
    }

    private void ActivateInvisibility()
    {
        SetInvisibility(true);
        _invisibilityTimeStamp = Time.time + _invisibilityCooldown;

        RunWithDelay(() => SetInvisibility(false), _invisibilityDuration);
    }

    private void SetInvisibility(bool value)
    {
        _isInvisible = value;
        _sprite.material.color = new Color(1f, 1f, 1f, value ? .5f : 1f);
    }

    private async Task RunWithDelay(Action action, float secondsDelay)
    {
        await Task.Delay(TimeSpan.FromSeconds(secondsDelay));

        action();
    }
}
