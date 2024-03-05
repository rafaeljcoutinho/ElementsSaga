using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetHolder : MonoBehaviour
{
    private void OnMouseDown() {
        TargetEventHandler.OnTarget(gameObject.GetComponentInParent<PlayerStats>());
    }
}


public class TargetEventArgs : EventArgs
{
    public PlayerStats PlayerStats { get; private set; }

    public TargetEventArgs(PlayerStats player)
    {
        PlayerStats = player;
    }
}

public static class TargetEventHandler  {

    public static event EventHandler<TargetEventArgs> OnTargetClick;
    public static void OnTarget(PlayerStats playerStats){
        OnTargetClick?.Invoke(playerStats.gameObject, new TargetEventArgs(playerStats));
    }
}
