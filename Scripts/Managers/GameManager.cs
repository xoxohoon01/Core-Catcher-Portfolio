using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public float time = 0;
    public string characterName;

    public List<GameObject> characterObjects = new List<GameObject>();

    [Range(0.0f, 30.0f)]
    public float timeScale = 1.0f;

    private void Start()
    {
        RefreshCharacter();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (Input.GetButtonDown("Menu") && SceneManager.GetActiveScene().name == "MainScene")
        {
            MenuButtons buttons = UIManager.Instance.Get<MenuButtons>();
            if (buttons == null || (buttons != null && !buttons.gameObject.activeInHierarchy))
            {
                UIManager.Instance.Show<MenuButtons>();
            }
            else
            {
                UIManager.Instance.Get<MenuButtons>().Continue();
            }
        }

        if (!BattleManager.Instance.isStop)
            Time.timeScale = timeScale;
    }

    public void AddCharacterObject(GameObject character)
    {
        characterObjects.Add(character);
    }

    public void SelectCharacter(string characterName)
    {
        this.characterName = characterName;

        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
            foreach (GameObject characterObject in characterObjects)
            {
                if (characterObject.name == characterName)
                    characterObject.SetActive(true);
                else
                    characterObject.SetActive(false);
            }

            UIManager.Instance.Get<CharacterSelect>().RefreshText(characterName);
        }
    }

    public void RefreshCharacter()
    {
        foreach (GameObject characterObject in characterObjects)
        {
            if (characterObject.name == characterName)
            {
                characterObject.SetActive(true);
                if (SceneManager.GetActiveScene().name == "MainScene")
                {
                    PlayerManager.Instance.SetPlayer(characterObject.GetComponent<PlayerController>());
                }
            }
            else
            {
                characterObject.SetActive(false);
            }
        }
    }

}
