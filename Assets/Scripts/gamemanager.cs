using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;


    public GameObject winnerScren;
    public Tiktok reader;
    public MenuManager menü;

    public GameObject confeti;

    [SerializeField] Rigidbody spinrb;
    [SerializeField] float power;

    public AudioSource source;
    int maxindex = 0;

    int randomuser = 0;

    public float rotateaxix;

    bool startspin;
    void Start()
    {
        Application.targetFrameRate = 144;
        if (instance == null) instance = this;
        else if (instance != null) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // spinrb.AddTorque(Vector3.forward * power * 0.8f, ForceMode.Impulse);
            spinrb.transform.DOLocalRotate(new Vector3(0, 0, rotateaxix) * 15, 10, RotateMode.Fast).SetRelative(true).SetEase(Ease.OutCubic, 5f);
        }
        if (startspin)
        {
            startspin = false;
            // Debug.Log("hello");
            // spinrb.AddTorque(Vector3.forward * power * 0.8f, ForceMode.Impulse);

            if (maxindex == 0)
            {
                resetGame();
                return;
            }
            source.Play();

            menü.CoutDown.gameObject.SetActive(false);
            randomuser = Random.Range(0, maxindex);
            reader.SpingGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            Debug.Log(randomuser + "RANDOM USER ");
            power += randomuser;
            //(Random.Range((randomuser * 45) - 20, (randomuser * 45) + 20)) (randomuser*45)
            float axis = 360 +(randomuser*45);
            Debug.Log(axis);
            //spinrb.AddTorque(Vector3.forward * power * 0.8f, ForceMode.Impulse);
            spinrb.transform.DOLocalRotate(new Vector3(0, 0, axis) * 15, 12, RotateMode.Fast).SetRelative(true).SetEase(Ease.OutCubic, 5f).OnStepComplete(()
            => StartCoroutine(winnerset()));
            // StartCoroutine(winnerset());

        }
    }


    public void StartSpin(short max_index)
    {
        maxindex = max_index;
        startspin = true;
    }

    IEnumerator winnerset()
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(reader.userstats[randomuser].photoadress);
        yield return www.SendWebRequest();
        Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        int myTexturesize = myTexture.width;
        reader.userstats[8].sprite.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexturesize, myTexturesize), new Vector2(0.5f, 0.5f), 100);
        reader.userstats[8].UserName = reader.userstats[randomuser].name;
        reader.userstats[8].userparent.SetActive(true);
        reader.userstats[8].userparent.transform.DOLocalMove(new Vector3(0f, 0.368f, 1.251f), 0.7f);
        reader.userstats[8].userparent.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.7f);
        winnerScren.SetActive(true);
        iTween.ShakeRotation(winnerScren, Vector3.forward * 25, 2.6f);
        confeti.SetActive(true);
        yield return new WaitForSeconds(3f);
        winnerScren.SetActive(false);
        confeti.SetActive(false);
        yield return new WaitForSeconds(10f);
        resetGame();

    }

    public void resetGame()
    {
        power = 48;
        foreach (userstat r in reader.userstats)
        {
            r.userparent.SetActive(false);
            r.UserName = "";
        }

        //
        reader.userstats[8].userparent.transform.localPosition = new Vector3(0, 1.039966f, 0);
        reader.userstats[8].userparent.transform.localScale = Vector3.one;
        //
        reader._profilePictures.Clear();
        reader.index = 0;
        reader.startchat = true;
        reader.Next = true;
        reader.SpingGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        menü.CoutDown.text = 15.ToString();
        menü.CoutDown.gameObject.SetActive(true);
        StartCoroutine(reader.CoutDown());

    }
}
