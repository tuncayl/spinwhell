using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tiktokreader : MonoBehaviour
{


}

[System.Serializable]
public class userstat
{
    public GameObject userparent;

    [HideInInspector] public string name = "";
    public string UserName
    {
        get { return name; }
        set
        {
            short submax = (short)Mathf.Clamp(value.Length, value.Length, 6);
            UsernameText.text = value.Substring(0, submax);
            name = value;
        }
    }
    public TextMeshPro UsernameText;
    public SpriteRenderer sprite;

    [HideInInspector] public string photoadress = "";

}
public struct userdata
{
    public string Username;
    public List<string> photoadress;
    public string comment;
    public string Commentset
    {
        get { return comment; }
        set
        {
            comment=value.ToLower();
        }
    }
}
