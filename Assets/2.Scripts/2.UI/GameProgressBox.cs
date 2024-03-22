using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameProgressBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _remainDistanceText;

    public void SetDistance(int distance)
    {
        _remainDistanceText.text = distance.ToString();
    }

}
