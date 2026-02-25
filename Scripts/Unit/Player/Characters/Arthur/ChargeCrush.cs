using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Arthur/ChargeCrush")]
public class ChargeCrush : SkillData
{
    public override void OnAnimationEvent(UnitController user, int number)
    {
        ArthurController arthur = user as ArthurController;
        if (arthur == null) return;

        Quaternion rotation = Quaternion.LookRotation(arthur.transform.forward);
        Vector3 spawnPos = arthur.transform.position + arthur.transform.forward + (Vector3.up * 2f);

        // 자기 자신의 baseSpan 사용 (배열 참조 제거)
        float totalTime =
            (baseSpan /
            (1f + ((arthur.status.attackSpeed / arthur.characterData.attackSpeed) * 0.1f)))
            / arthur.status.skillSpeed;

        float activeTime = totalTime * 0.5f;

        float range = 8f + ((arthur.status.skillRange - 1f) * 1f);
        float height = 8f + ((arthur.status.skillRange - 1f) * 0.5f);

        Vector3 size = new Vector3(range, height, range);

        // 스킬 오브젝트 생성
        GameObject skillObj = ObjectPoolManager.Instance
            .Spawn("ArthurSkill3", spawnPos, rotation);

        ArthurSkill3Controller controller = skillObj.GetComponent<ArthurSkill3Controller>();
        if (controller == null)
        {
            Debug.LogError("ArthurSkill3Controller 없음");
            return;
        }

        controller.Initialize(
            (arthur.status.damage * 2f) * arthur.status.skillDamage,
            0f,
            0.5f,
            0f,
            activeTime,
            0,
            arthur.faction,
            size
        );

        // owner 지정 (중요)
        controller.SetOwner(arthur.transform);

        if (arthur.dashDelay <= 1f)
            arthur.dashDelay = 1f;

        ObjectPoolManager.Instance
            .Spawn("AudioObject", arthur.transform.position, Quaternion.identity)
            .GetComponent<AudioObject>()
            .PlayAudio("Sword/ChargeCrush", "Weapon", 1);
    }
}