using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusText : UIBase
{
    [Header("Left")]
    public TMP_Text maxHP;
    public TMP_Text damage;
    public TMP_Text armor;
    public TMP_Text moveSpeed;
    public TMP_Text attackSpeed;
    public TMP_Text cooldown;
    public TMP_Text skillRange;
    public TMP_Text skillSpeed;

    [Header("Right")]
    public TMP_Text critChance;
    public TMP_Text critDamage;
    public TMP_Text skillDamage;
    public TMP_Text drain;
    public TMP_Text dashCooldown;

    private void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        Status status = PlayerManager.Instance.GetPlayer()?.status;

        if (status == null)
            return;

        maxHP.text = $"{status.maxHP:0.##}";
        damage.text = $"{status.damage:0.##}";
        armor.text = $"{status.armor:0.##}";
        moveSpeed.text = $"{status.moveSpeed * 10f:0.##}";
        attackSpeed.text = $"{status.attackSpeed:0.##}";
        cooldown.text = $"{status.cooldownReduction * 100:0.##}%";
        skillRange.text = $"{status.skillRange * 100:0.##}%";
        skillSpeed.text = $"{status.skillSpeed * 100:0.##}%";

        critChance.text = $"{status.critChance * 100:0.##}%";
        critDamage.text = $"x{status.critDamage:0.##}%";
        skillDamage.text = $"{status.skillDamage * 100:0.##}%";
        drain.text = $"{status.drain * 100:0.##}%";
        dashCooldown.text = $"{status.dashCooldown:0.##}";
    }
}
