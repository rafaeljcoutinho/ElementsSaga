using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase : ScriptableObject
{
    public string amount;
    public int clock;
}


public interface IBuff{
    public void PerformBuff(PlayerStats player);
}