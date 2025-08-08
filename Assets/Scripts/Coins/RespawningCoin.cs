using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> onCollected;
    private Vector3 previousPos;
    private void Update()
    {
        if(previousPos != transform.position)
        {
            Show(true);
        }

        previousPos = transform.position;
    }
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false); return 0;
        }
        if(AlreadyCollected) return 0;

        AlreadyCollected = true;

        onCollected?.Invoke(this);

        return CoinValue;
    }

    public void Reset()
    {
        AlreadyCollected = false;
    }
}
