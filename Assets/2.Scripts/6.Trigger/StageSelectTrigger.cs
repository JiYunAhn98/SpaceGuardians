using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class StageSelectTrigger : StageSlotLoader
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager._instance.PlayEffect(eEffectSound.WindowOpen);
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager._instance.PlayEffect(eEffectSound.WindowClose);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
