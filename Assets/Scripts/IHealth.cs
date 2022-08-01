using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void OnDamage(int amount);
    public void OnHealth(int amount);
}
