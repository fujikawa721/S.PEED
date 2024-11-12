using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public string character_name;
    public int character_id;
    public int max_hp;
    public int base_damage;
    public int max_sp_point;
    public string element_mark;

    public string character_type;
    public string sp_name;
    public string sp_info;
}
