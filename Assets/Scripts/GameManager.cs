using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Properties")]
    public Text currentPlayerNameText;
    public Text currentMoneyText;
    public Text currentExperienceText;

    [Header("Player Profile")]
    private PlayerProfile playerProfile = new PlayerProfile();
    private string[] playerNames = { "Ruby", "Jane", "Lisa", "Jisoo", "Ryan" };

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
        playerProfile.playerName = playerNames[Random.Range(0, playerNames.Length)];
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
}
