using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Properties")]
    public Text currentPlayerNameText;
    public Text currentMoneyText;
    public Text currentExperienceText;
    public InputField currentPlayerNameInputField;

    [Header("Player Profile")]
    private PlayerProfile playerProfile = new PlayerProfile();
    private string[] playerNames = { "Ruby", "Jane", "Lisa", "Jisoo", "Ryan" };

    [Header("Firebase Information")]
    private string idToken;
    private string databaseURL = "https://multifacetedsavingproject.firebaseio.com/users";
    private string AuthKey = "AIzaSyA4UIovzKrNwxMjcB0FiDtAMSqiO-C_hes";
    public InputField usernameTextInputField;
    public InputField passwordTextInputField;
    public InputField emailTextInputField;
    public static string localID; //Only for Firebase integration.
    public void AddMoney()
    {
        playerProfile.playerCurrency += 50;
        UpdateUI();
    }

    public void AddExperience()
    {
        playerProfile.playerExperience += 100;
        UpdateUI();
    }

    public void DrawRandomName()
    {
        playerProfile.playerName = playerNames[UnityEngine.Random.Range(0, playerNames.Length)];
        UpdateUI();
    }

    private void UpdateUI()
    {
        currentPlayerNameText.text = playerProfile.playerName;
        currentMoneyText.text = playerProfile.playerCurrency.ToString();
        currentExperienceText.text = playerProfile.playerExperience.ToString();
    }

    public void SavaData()
    {
        SaveData.current.profile = playerProfile;
        SerializationManager.Save("Save", SaveData.current);
    }

    public void LoadData()
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/Saves/Save.save");
        playerProfile = SaveData.current.profile;
        UpdateUI();
    }

    /// <summary>
    /// Firebase Integration Below
    /// </summary>
    /// 
    public void SaveToFirebase(bool emptyScore = false)
    {
        //RestClient.Put to avoid having the system generate random strings of characters. Otherwise, you may use RestClient.Post().
        if (emptyScore == true)
        {
            playerProfile.playerCurrency = 0;
            playerProfile.playerExperience = 0;
        }
        RestClient.Put(databaseURL + "/" + localID + ".json", playerProfile);
    }

    public void LoadFromFirebase()
    {
        RestClient.Get<PlayerProfile>(databaseURL + "/" + localID + ".json").Then(response =>
        {
            playerProfile = response;
            UpdateUI();
        });
    }

    public void SignUpUserButton()
    {
        SignUpUser(emailTextInputField.text, usernameTextInputField.text, passwordTextInputField.text);
    }

    public void SignInUserButton()
    {
        SignInUser(emailTextInputField.text, passwordTextInputField.text);
    }

    //Firebase Authentication System
    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey, userData).Then(response =>
        {
            idToken = response.idToken;
            localID = response.localID;
            playerProfile.playerName = username;
            playerProfile.localID = localID;
            SaveToFirebase(true);
        }).Catch(error =>
        {
            Debug.LogError("Unable to create new user due to: " + error);
        });
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey, userData).Then(response =>
        {
            idToken = response.idToken;
            localID = response.localID;
        }).Catch(error =>
        {
            Debug.LogError("Unable to login due to: " + error);
        });
    }
}

[Serializable]
public class SignResponse
{
    public string localID;
    public string idToken;
}
