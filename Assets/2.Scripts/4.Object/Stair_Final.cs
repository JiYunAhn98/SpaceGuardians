using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair_Final : Block
{
    public override void Init()
    {
        _collider = gameObject.GetComponent<EdgeCollider2D>();
        _collider.isTrigger = true;
    }

    public override void WhenInArea()
    {
        _collider.isTrigger = false;
        StartCoroutine(OnMyEvent());
    }

    public override void WhenOutArea()
    {
        _collider.isTrigger = true;
    }

    public IEnumerator OnMyEvent()
    {
        SpriteRenderer block = GetComponent<SpriteRenderer>();

        while (block.color.a < 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color += Color.black * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
