using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private float _invisibilityCooldown = 10f;
    [SerializeField] private float _invisibilityDuration = 3f;
    [SerializeField] private TMP_Text _abilityInfoTextMesh;
    [SerializeField] private string _abilityInfoTextFormat = "Cooldown: {0} sec";

    private SpriteRenderer _sprite;
    private float _invisibilityCooldownTimeStamp;
    private float _invisibilityDurationTimeStamp;
    private bool _isInvisible;
    private string _defaultText;

    public bool IsInvisible => _isInvisible;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _defaultText = _abilityInfoTextMesh.text;
    }

    private void Update()
    {
        if (_invisibilityCooldownTimeStamp <= Time.time && Input.GetButtonDown(InputConstants.AbilityButtonName))
            ActivateInvisibility();

        if (_isInvisible && _invisibilityDurationTimeStamp <= Time.time)
            SetInvisibility(false);
    }

    private void FixedUpdate()
    {
        _abilityInfoTextMesh.text = GetInfoText();
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

    private string GetInfoText()
    {
        float secondsLeft = _invisibilityCooldownTimeStamp - Time.time;

        if (secondsLeft >= 0)
            return string.Format(_abilityInfoTextFormat, secondsLeft.ToString("F2"));
        else
            return _defaultText;
    }
}