using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerAbilities))]
public class PlayerInfo : MonoBehaviour
{
    public EntityStats Stats { get; private set; }
    public Collider2D Collider { get; private set; }
    public PlayerAbilities Abilities { get; private set; }


    private void Start()
    {
        Stats = GetComponent<EntityStats>();
        Collider = GetComponent<Collider2D>();
        Abilities = GetComponent<PlayerAbilities>();
    }
}
