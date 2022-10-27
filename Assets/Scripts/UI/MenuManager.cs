using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject connectionscreen, loginbutton,loginscreen;

    public TextMeshProUGUI CoutDown;

    public void texediting(TMP_InputField inputField)
    {
        if (inputField.text.Length > 1)
        {
            loginbutton.SetActive(true);

        }
        else
        {
            loginbutton.SetActive(false);
        }
    }

    public void setstartchat(TMP_InputField inputField)
    {
        gamemanager.instance.reader.id=inputField.text;
        gamemanager.instance.reader.enabled = true;
        loginbutton.SetActive(false);
        
    }
}
