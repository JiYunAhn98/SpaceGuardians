using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PickingLeftArrow : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerManager._instance._nowPickCharacter > 0)
        {
            GalaxyManager._instance.SpawnPlayer(PlayerManager._instance._nowPickCharacter-1);
        }
    }
}
