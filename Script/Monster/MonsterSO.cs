
using UnityEngine;

public enum PlayerUnitType
{
    DRAGON,
    HUMANOID,
    MAGICAL
}

[CreateAssetMenu(fileName = "MonsterSO", menuName = "Monster", order = 1)]
public class MonsterSO : ScriptableObject
{
    public string Name;
    public float health;
    public float speed;
    public PlayerUnitType size;
    public RuntimeAnimatorController animatorController;
}