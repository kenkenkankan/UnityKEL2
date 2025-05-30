using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scriptable object class sfor character and NPC
/// </summary>
[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterData : BaseStatData //May need Editor for differentiate between NPC & Hostile NPC/Enemies & MC
{
    public Image dialogueImage;
    public List<RuntimeAnimatorController> animControllers;
    public string type;

    [Serializable]
    public class Stats : BaseStats
    {
        [Range(0, 1)] public float movementSpeed;
        [Range(0.001f, 1)] public float Sanity;
        public float fearDamage;

        public static Stats operator +(Stats s1, Stats s2)
        {
            s1.movementSpeed += s2.movementSpeed;
            s1.Sanity += s2.Sanity;
            // s1.sanityRecoveryRate += s2.sanityRecoveryRate;
            // s1.fearResistance += s2.fearResistance;
            s1.fearDamage += s2.fearDamage;

            return s1;
        }
    }

    public Stats stats;
}
