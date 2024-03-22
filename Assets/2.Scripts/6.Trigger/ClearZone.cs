using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<StageBaseManager>().ProgEnd(true);
            if (SceneControlManager._instance._currentScene == DefineHelper.eSceneName.JumpClimb)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                StartCoroutine(OnMyEvent());
            }
        }
    }
    public IEnumerator OnMyEvent()
    {
        SpriteRenderer block = transform.GetChild(0).GetComponent<SpriteRenderer>();

        while (block.color.a < 1)
        {
            block.color += Color.black * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
