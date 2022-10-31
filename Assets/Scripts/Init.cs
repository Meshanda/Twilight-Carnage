using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    private const string MAIN_SCENE = "SampleScene";
    async void Start()
    {
        await UnityServices.InitializeAsync();

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
