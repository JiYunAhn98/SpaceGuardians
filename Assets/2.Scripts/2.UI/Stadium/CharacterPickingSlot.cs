using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineHelper;
public class CharacterPickingSlot : MonoBehaviour, IPointerClickHandler
{
    Image _characterIcon;
    eCharacterName _name;

    public void OnPointerClick(PointerEventData eventData)
    {
        StadiumManager._instance.ChangeCharacter(_name);
    }
    public void SetIcon(eCharacterName name)
    {
        _characterIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _characterIcon.sprite = Resources.Load<Sprite>("CharacterThumbnail/" + name.ToString());
        _name = name;
    }
    public void PickThisSlot()
    {
        _characterIcon.color = Color.gray;
    }
    public void PickOtherSlot()
    {
        _characterIcon.color = Color.white;
    }
}
