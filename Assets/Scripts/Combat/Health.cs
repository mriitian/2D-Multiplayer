using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field : SerializeField] public int MaxHealth { get; private set; } = 100;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    private bool isDead = false;
    public Action<Health> onDie;
    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        currentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }
    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    private void ModifyHealth(int value)
    {
        if(isDead) return;
        int newHealth = currentHealth.Value + value;
        currentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
        if(currentHealth.Value == 0)
        {
            onDie?.Invoke(this);
            isDead = true;
        }
    }
}
