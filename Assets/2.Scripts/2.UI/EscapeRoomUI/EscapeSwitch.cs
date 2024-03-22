using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSwitch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<EscapeRoomManager>().ProgMazeSetting();
            Destroy(gameObject);
        }
    }
}
