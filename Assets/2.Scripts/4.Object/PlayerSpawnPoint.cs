using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class PlayerSpawnPoint : MonoBehaviour
{
    public Player GeneratePlayer()
    {
        GameObject go = Instantiate(Resources.Load("PlayerCharacter/" + PlayerManager._instance._nowPickCharacter.ToString()), transform) as GameObject;
        return go.transform.GetComponentInChildren<Player>();
    }
    public Player GeneratePlayer(eCharacterName character)
    {
        GameObject go = Instantiate(Resources.Load("PlayerCharacter/" + character.ToString()), transform) as GameObject;
        return go.transform.GetComponentInChildren<Player>();
    }
}
