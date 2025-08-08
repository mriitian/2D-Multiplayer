using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
    
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBar;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;
        health.currentHealth.OnValueChanged += HandleHealthChange;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        health.currentHealth.OnValueChanged -= HandleHealthChange;
        HandleHealthChange(0, health.currentHealth.Value);
    }
    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        healthBar.fillAmount = (float)newHealth / health.MaxHealth;
    }
}
