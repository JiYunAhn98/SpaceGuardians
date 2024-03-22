using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class PlayerManager : MonoSingleton<PlayerManager>
{
    eCharacterName _character = eCharacterName.Burn;
    public eCharacterName _nowPickCharacter { get { return _character; } set { _character = value; } }


}
