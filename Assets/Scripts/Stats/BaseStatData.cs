using System;
using UnityEngine;

/// <summary>
/// Base Scriptable Object for Stats
/// </summary>
public class BaseStatData : ScriptableObject
{
    public string Name;

    [Serializable]
    public class BaseStats
    {
        public float sanityRecoveryRate;
        public float fearResistance;

        public static BaseStats operator +(BaseStats s1, BaseStats s2)
        {
            s1.sanityRecoveryRate += s2.sanityRecoveryRate;
            s1.fearResistance += s2.fearResistance;
            
            return s1;
        }
    }
}
