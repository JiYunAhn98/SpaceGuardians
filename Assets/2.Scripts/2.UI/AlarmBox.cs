using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmBox : MonoBehaviour
{
	// ���� ����
	[SerializeField] TextMeshProUGUI _txtMessage;

    public void OpenBox(string msg)
    {
        gameObject.SetActive(true);
        _txtMessage.text = msg;
    }
    public void CloseBox()
    {
        gameObject.SetActive(false);
    }
}
