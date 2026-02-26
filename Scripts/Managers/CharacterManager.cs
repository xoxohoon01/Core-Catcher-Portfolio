using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public List<string> unlockedCharacterIds = new List<string>(); // 해금된 캐릭터 ID 목록
    public string lastSelectedCharacterId = "Raven";        // 마지막 선택 캐릭터
}

public class CharacterManager : MonoSingleton<CharacterManager>
{
    public CharacterData characterData;

    protected override void Awake()
    {
        base.Awake();

        Load();
    }

    public void Load()
    {
        if (DataManager.Exists("CharacterData"))
        {
            characterData = DataManager.Load<CharacterData>("CharacterData");
        }
        else
        {
            characterData = new CharacterData(); // 기본 데이터 생성
            DataManager.Save("CharacterData", characterData);
        }
    }

    public void Save()
    {
        DataManager.Save("CharacterData", characterData);
    }

    public void UnlockCharacter(string id)
    {
        if (!characterData.unlockedCharacterIds.Contains(id))
        {
            characterData.unlockedCharacterIds.Add(id);
            Save(); // 해금 즉시 저장
        }
    }
}
