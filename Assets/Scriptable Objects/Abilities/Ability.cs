using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ability", menuName = "Ability", order = 0)]
public class Ability : ScriptableObject {
    
    [SerializeField] public int damage;
    [SerializeField] public Sprite abilitySprite;
    [SerializeField] public int heal;
    [SerializeField] public float cooldownTime;
    [SerializeField] public float animationTime;
    [SerializeField] public int levelRequired;
    [SerializeField] public bool isEnabled;
    [SerializeField] public int cost;
    [SerializeField] public bool localCast;
    [SerializeField] public string description;

}
