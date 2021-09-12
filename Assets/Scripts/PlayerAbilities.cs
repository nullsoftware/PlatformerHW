using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private float _invisibilityCooldown = 15f;
    [SerializeField] private float _invisibilityDuration = 5f;

    private SpriteRenderer _sprite;
    private float _invisibilityCooldownTimeStamp;
    private float _invisibilityDurationTimeStamp;
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

        if (_isInvisible && _invisibilityDurationTimeStamp <= Time.time)
            SetInvisibility(false);
    }

    private bool CanActivateInvisibility()
    {
        return _invisibilityCooldownTimeStamp <= Time.time;
    }

    private void ActivateInvisibility()
    {
        SetInvisibility(true);
        _invisibilityCooldownTimeStamp = Time.time + _invisibilityCooldown;
        _invisibilityDurationTimeStamp = Time.time + _invisibilityDuration;
    }

    private void SetInvisibility(bool value)
    {
        _isInvisible = value;
        _sprite.material.color = new Color(1f, 1f, 1f, value ? .5f : 1f);
    }

}