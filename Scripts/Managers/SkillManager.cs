using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public int id;
    public bool isUnlocked;
}

public class SkillManager : MonoSingleton<SkillManager>
{
    public Dictionary<string, List<Skill>> Skill = new Dictionary<string, List<Skill>>();

    protected override void Awake()
    {
        base.Awake();

        LoadSkill();
    }

    public void SaveSkill(string characterName)
    {
        DataManager.Save($"{characterName}Skill", Skill[characterName]);
    }

    public void LoadSkill()
    {
        CharacterScriptableObject[] characters = Resources.LoadAll<CharacterScriptableObject>("CharacterSO/");
        foreach (var character in characters)
        {
            if (DataManager.Exists($"{character.characterName}Skill"))
            {
                List<SkillNode> skills = Resources.Load<SkillTreeData>($"SkillTreeSO/{character.characterName}").skillNodes;
                List<Skill> skillData = DataManager.Load<List<Skill>>($"{character.characterName}Skill");

                foreach (var skillNode in skills)
                {
                    bool exists = skillData.Exists(s => s.id == skillNode.id);
                    if (!exists)
                    {
                        Skill newSkill = new Skill();
                        newSkill.id = skillNode.id;
                        newSkill.isUnlocked = false;
                        skillData.Add(newSkill);
                    }
                }

                skillData.Sort((a, b) => a.id.CompareTo(b.id));
                Skill[character.characterName] = skillData;
                SaveSkill(character.characterName);
            }
            else
            {
                List<SkillNode> skills = Resources.Load<SkillTreeData>($"SkillTreeSO/{character.characterName}")?.skillNodes;
                List<Skill> createdSkills = new List<Skill>();
                if (skills != null && skills.Count > 0)
                {
                    foreach (var skill in skills)
                    {
                        Skill createdSkill = new Skill();
                        createdSkill.id = skill.id;
                        createdSkill.isUnlocked = false;

                        createdSkills.Add(createdSkill);
                    }
                    Skill[character.characterName] = createdSkills;
                    SaveSkill(character.characterName);
                }
            }
        }
    }

    public bool CheckSkillUnlocked(string characterName, int id)
    {
        foreach (Skill skill in Skill[characterName])
        {
            if (skill.id == id)
            {
                if (skill.isUnlocked)
                    return true;
                else
                    return false;
            }
        }

        return false;
    }
}
