using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/SkillTreeData")]
public class SkillTreeData : ScriptableObject
{
    public string characterName;
    public List<SkillNode> skillNodes;
}
