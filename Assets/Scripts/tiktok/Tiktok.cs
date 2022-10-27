/// Basic profile picture handling example.
/// - @sebheron 2022

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Models;
using TikTokLiveSharp.Events;
using TMPro;


public class Tiktok : MonoBehaviour
{
    /// <summary>
    /// The uniqueId for the stream.
    /// </summary>
    public string id;

    /// <summary>
    /// The TikTokLiveClient, initalised on Start.
    /// </summary>
    private TikTokLiveClient _client;

    /// <summary>
    /// Queue of profile pictures
    /// </summary>
    public Queue<userdata> _profilePictures;

    /// <summary>
    /// bool check connection
    /// </summary>
    public bool startchat = false;

    /// <summary>
    /// bool usertat connection
    /// </summary>
    public List<userstat> userstats = new List<userstat>();

    /// <summary>
    /// Delay Next 
    /// </summary>
    public bool Next = true;

    /// <summary>
    /// current userstats index
    /// </summary>
    public short index = 0;

    /// <summary>
    /// spin object
    /// </summary>
    public GameObject SpingGameObject;



    /// <summary>
    /// Start method. Initalises 
    /// </summary>
    void Start()
    {

        _client = new TikTokLiveClient(id);
        _profilePictures = new Queue<userdata>();
        _client.OnConnected += OnConnected;
        _client.OnCommentRecieved += Client_OnCommentRecieved;
        try
        {
    
            _client.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    void OnConnected(object sender, ConnectionEventArgs eventArgs)
    {
        Debug.Log(eventArgs.IsConnected.ToString());
        startchat = eventArgs.IsConnected;
        gamemanager.instance.menü.connectionscreen.SetActive(false);
        gamemanager.instance.menü.loginscreen.SetActive(false);
        StartCoroutine(CoutDown());

    }


    void OnDestroy()
    {
        _client.Stop();
        _client.OnCommentRecieved -= Client_OnCommentRecieved;
    }

    void Client_OnCommentRecieved(object sender, WebcastChatMessage e)
    {
        if (!startchat) return;
        Debug.Log(e.Comment + " USER  ->> " + e.User.Nickname);

        lock (e)
        {
            var url = e.User.profilePicture.Urls;
            if (url != null)
            {

                userdata data = new userdata()
                {
                    Username = e.User.Nickname,
                    photoadress = url,
                    Commentset = e.Comment,
                };
                _profilePictures.Enqueue(data);
            }
        }

    }

    void Update()
    {

        if (_profilePictures.Count > 0)
        {
            if (!Next) return;
            Next = false;
            // Dequeue the data
            userdata data = _profilePictures.Dequeue();
            // Add a new data
            StartCoroutine(AddNewData(data));
        }

    }

    /// <summary>
    /// Enumerator method called from Update.
    /// This method adds a new Data with the players
    /// </summary>
    IEnumerator AddNewData(userdata data)
    {
        if (data.Commentset != "me")
        {
            Next=true;
            yield break;
        }

        if (index == 8) yield break;
        var url = data.photoadress.FirstOrDefault(x => x.Contains(".jpeg"));
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        var tex = DownloadHandlerTexture.GetContent(request);
        int texturelength = tex.width;
        //add list
        userstats[index].UserName = data.Username;
        userstats[index].sprite.sprite = Sprite.Create(tex,
        new Rect(0, 0, texturelength, texturelength),
        new Vector2(0.5f, 0.5f), 100);
        userstats[index].photoadress = url;

        userstats[index].userparent.SetActive(true);
        yield return new WaitForSeconds(1f);
        iTween.RotateAdd(SpingGameObject, iTween.Hash("z", -45f, "looptype", iTween.LoopType.none));

        index++;
        Next = true;
    }

    public IEnumerator CoutDown()
    {
        TextMeshProUGUI coutdown = gamemanager.instance.menü.CoutDown;
        for (int i = 15; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            coutdown.text = i.ToString();
        }
        startchat = false;

        gamemanager.instance.StartSpin(index);
    }
}