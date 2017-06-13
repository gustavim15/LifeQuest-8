using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Facebook.MiniJSON;

public class FacebookController : MonoBehaviour
{
    [SerializeField] private GameObject imagePerfil = null;
    [SerializeField] private GameObject txtOla = null;
    [SerializeField] private GameObject btnContinuar = null;
    [SerializeField] private ControllerGeral controllerGeral = null;
    [SerializeField] private QuestController questController = null;
    [SerializeField] private FilhoController filhoController = null;
    private Dictionary<string, string> profile = null;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            FB.LogOut();
        }
    }

    public void FBlogin()
    {
        FB.LogInWithReadPermissions(callback: AuthCallback);
    }

    void AuthCallback(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB login worked!");
            ScreenOrder(true);
        }
        else
        {
            Debug.Log("FB Login fail");
            ScreenOrder(false);
        }
    }

    void ScreenOrder(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API(Util.GetPictureURL("me", 128, 128), HttpMethod.GET, ProfilePictureCallback);
            FB.API("/me?fields=id,first_name", HttpMethod.GET, faceUserNameCallback);

        }
    }

    void ProfilePictureCallback(IGraphResult result)
    {
        imagePerfil.SetActive(true);
        if (result.Error != null)
        {
            Debug.Log("problem with getting profile picture");

            FB.API(Util.GetPictureURL("me", 128, 128), HttpMethod.GET, ProfilePictureCallback);
            return;
        }

        Image UserAvatar = imagePerfil.GetComponent<Image>();
        UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));

    }

    void faceUserNameCallback(IResult result)
    {
        txtOla.SetActive(true);
        if (result.Error != null)
        {
            Debug.Log("problem with getting profile picture");
			
            FB.API("/me?fields=id,first_name", HttpMethod.GET, faceUserNameCallback);
            return;
        }

        profile = Util.DeserializeJSONProfile(result.RawResult);

        Text UserMsg = txtOla.GetComponent<Text>();

        ControllerGeral.instance.UserID = profile["id"];
        UserMsg.text = "Bem vindo, " + profile["first_name"];
        controllerGeral.enabled = true;
        questController.enabled = true;
        filhoController.enabled = true;
        btnContinuar.SetActive(true);
    }

    public void ShareWithFriends()
    {
//        FB.ShareLink(
//            new Uri("http://gustavopequeno2.wixsite.com/gustavopequeno/mdulo-v"),
//            callback: ShareCallback
//        );
        FB.ShareLink(contentTitle: "Jogue LifeQuest!", contentURL: new System.Uri("http://gustavopequeno2.wixsite.com/gustavopequeno/mdulo-v"), contentDescription: "Download do jogo LifeQuest", callback: ShareCallback);
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || result.Error != null)
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (result.PostId != null)
        {
            Debug.Log(result.PostId);
        }
        else
        {
            Debug.Log("ShareLink success!");
        }
    }
}


