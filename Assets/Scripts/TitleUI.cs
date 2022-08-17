using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleUI : MonoBehaviour
{
    public TMP_InputField usernameInput;

    public void SetUsername(string newName)
    {
        MainManager.Instance.currentUsername = newName;
    }
    void Start()
    {
        usernameInput.onValueChanged.AddListener(delegate { SetUsername(usernameInput.text); });
        if (MainManager.Instance.currentUsername != null)
        {
            usernameInput.GetComponent<TMP_InputField>().text = MainManager.Instance.currentUsername;
        }
    }

}
