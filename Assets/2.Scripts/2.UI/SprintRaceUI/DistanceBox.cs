using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceBox : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _distance;

    public void SetDistance(int distance)
    {
        _distance.text = distance.ToString();
    }
}
