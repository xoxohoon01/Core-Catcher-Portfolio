using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationEventListener : MonoBehaviour
{
    public void AttackEvent(int number)
    {
        GetComponent<PlayerController>().BasicAttackInitialize(number);
    }

    public void SkillEvent(int number)
    {
        GetComponent<PlayerController>().OnSkillAnimationEvent(number);
    }

    public void Skill1Event(int number)
    {
        GetComponent<PlayerController>().Skill1Initialize(number);
    }

    public void Skill2Event(int number)
    {
        GetComponent<PlayerController>().Skill2Initialize(number);
    }

    public void Skill3Event(int number)
    {
        GetComponent<PlayerController>().Skill3Initialize(number);
    }

    public void Skill4Event(int number)
    {
        GetComponent<PlayerController>().Skill4Initialize(number);
    }
}
