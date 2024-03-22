using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickingRightArrow : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerManager._instance._nowPickCharacter < DefineHelper.eCharacterName.Count - 1)
        {
            GalaxyManager._instance.SpawnPlayer(PlayerManager._instance._nowPickCharacter+1);
        }
    }
}
