using System.Collections;
using System.Collections.Generic;
using ParrelSync;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    private const string MAIN_SCENE = "MainMenu";
    async void Start()
    {
        var options = new InitializationOptions();
        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "MasterProfile");

        await UnityServices.InitializeAsync(options);

        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            AuthenticationService.Instance.SignedIn += OnSignedIn;

            if (AuthenticationService.Instance.IsSignedIn)
            {
                var username = PlayerPrefs.GetString("username");
                if (username == "")
                {
                    username = "Player";
                    PlayerPrefs.SetString("username", username);
                }
            }

            SceneManager.LoadSceneAsync(MAIN_SCENE);
        }
    }

    private void OnSignedIn()
    {
        Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
        Debug.Log($"Token: {AuthenticationService.Instance.AccessToken}");
        
    }
}
