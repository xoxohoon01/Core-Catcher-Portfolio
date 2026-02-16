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
        characterName = CharacterManager.Instance.characterData.lastSelectedCharacterId;
        RefreshCharacter();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (Input.GetButtonDown("Menu") && (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "MainLabScene"))
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
        bool selected = false;
        foreach (GameObject characterObject in characterObjects)
        {
            if (characterObject.name == characterName)
            {
                characterObject.SetActive(true);
                selected = true;

                if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "MainLabScene")
                {
                    characterObject.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
                    Camera.main.transform.position = characterObject.transform.position + new Vector3(0, 14, -9.5f);
                    PlayerManager.Instance.SetPlayer(characterObject.GetComponent<PlayerController>());
                }
            }
            else
            {
                characterObject.SetActive(false);
            }
        }

        if (selected == false)
        {
            characterName = "Raven";
            RefreshCharacter();
        }
    }

}
