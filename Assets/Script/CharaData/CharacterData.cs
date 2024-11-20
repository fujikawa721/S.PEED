using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int characterId;
    public int maxHp;
    public int baseDamage;
    public int maxSpPoint;
    public string elementMark;

    public string characterType;
    public string spName;
    public string spInfo;
    public Sprite faceImage;
    public Sprite normalCutInImage;
    public Sprite specialCutInImage;
}
