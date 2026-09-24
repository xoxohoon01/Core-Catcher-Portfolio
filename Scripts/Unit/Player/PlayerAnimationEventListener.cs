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

    // --- 예전 이름 호환용. Raven 애니메이션 클립(RavenAttack1~3, RavenSkill1~4)이
    // 리네임 이전 이름을 그대로 부르고 있어서, 클립을 건드리지 않고 여기서 받는다.
    // Skiill1~4는 SkillNInitialize(현재는 빈 스텁)가 아니라, SkillData.OnAnimationEvent를
    // 타는 SkillEvent와 동일하게 연결한다 — 실제 발사체 스폰은 그쪽 경로에서 일어난다.
    public void Attack(int number) => AttackEvent(number);
    public void Skiill1(int number) => SkillEvent(number);
    public void Skiill2(int number) => SkillEvent(number);
    public void Skiill3(int number) => SkillEvent(number);
    public void Skiill4(int number) => SkillEvent(number);
}
