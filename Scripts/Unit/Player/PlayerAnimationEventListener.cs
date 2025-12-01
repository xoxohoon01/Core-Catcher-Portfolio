using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationEventListener : MonoBehaviour
{
    public void Attack(int number)
    {
        transform.parent.GetComponent<PlayerController>().BasicAttackInitialize(number);
    }

    public void Skiill1(int number)
    {
        transform.parent.GetComponent<PlayerController>().Skill1Initialize(number);
    }

    public void Skiill2(int number)
    {
        transform.parent.GetComponent<PlayerController>().Skill2Initialize(number);
    }

    public void Skiill3(int number)
    {
        transform.parent.GetComponent<PlayerController>().Skill3Initialize(number);
    }

    public void Skiill4(int number)
    {
        transform.parent.GetComponent<PlayerController>().Skill4Initialize(number);
    }
}
