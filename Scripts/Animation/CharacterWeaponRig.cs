using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터별 전용 스크립트(RavenEventListener 등) 대신 쓰는 공용 무기 온오프 컴포넌트.
/// 인스펙터에서 무기 이름-오브젝트 쌍만 등록하면, 애니메이션 이벤트에서
/// ShowWeapon("DE50_Right") / HideWeapon("Shotgun") 식으로 이름으로 호출한다.
/// 새 캐릭터를 만들 때도 이 컴포넌트 하나로 처리하고, 새 스크립트를 만들 필요가 없다.
/// </summary>
public class CharacterWeaponRig : MonoBehaviour
{
    [Serializable]
    public class WeaponSlot
    {
        public string weaponName;
        public GameObject weaponObject;
    }

    [SerializeField] private List<WeaponSlot> weapons = new List<WeaponSlot>();

    private Dictionary<string, GameObject> lookup;

    private void Awake()
    {
        lookup = new Dictionary<string, GameObject>();
        foreach (var slot in weapons)
        {
            if (slot.weaponObject == null) continue;

            if (lookup.ContainsKey(slot.weaponName))
                Debug.LogWarning($"CharacterWeaponRig({name}): 무기 이름 '{slot.weaponName}' 중복 등록됨.");
            else
                lookup.Add(slot.weaponName, slot.weaponObject);
        }
    }

    public void ShowWeapon(string weaponName)
    {
        if (lookup.TryGetValue(weaponName, out var obj))
            obj.SetActive(true);
        else
            Debug.LogWarning($"CharacterWeaponRig({name}): '{weaponName}' 무기를 찾을 수 없습니다.");
    }

    public void HideWeapon(string weaponName)
    {
        if (lookup.TryGetValue(weaponName, out var obj))
            obj.SetActive(false);
        else
            Debug.LogWarning($"CharacterWeaponRig({name}): '{weaponName}' 무기를 찾을 수 없습니다.");
    }

    public void HideAllWeapons()
    {
        foreach (var slot in weapons)
        {
            if (slot.weaponObject != null)
                slot.weaponObject.SetActive(false);
        }
    }

    // --- Raven의 기존 애니메이션 클립(RavenAttack1~3, RavenIdle, RavenMove, RavenSkill1~4)이
    // 그대로 부르는 이벤트 이름 호환용. 애니메이션 클립은 건드리지 않고 여기서만 매핑한다.
    // 인스펙터에는 "LeftPistol", "RightPistol", "Shotgun", "SniperRifle" 이름으로 등록할 것.
    public void HideAll() => HideAllWeapons();
    public void HideLeft() { HideWeapon("LeftPistol"); HideWeapon("Shotgun"); }
    public void HideRight() { HideWeapon("RightPistol"); HideWeapon("SniperRifle"); }
    public void ShowLeftPistol() => ShowWeapon("LeftPistol");
    public void ShowRightPistol() => ShowWeapon("RightPistol");
    public void ShowShotgun() => ShowWeapon("Shotgun");
    public void ShowSniperRifle() => ShowWeapon("SniperRifle");
}
