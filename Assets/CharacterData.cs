using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public string chara_name;
    public int max_hp;
    public int attack_damage;
    public int max_sp_point;
    public string element_mark;
    public int special_id;
}
