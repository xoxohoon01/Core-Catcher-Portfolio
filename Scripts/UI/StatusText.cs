using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusText : UIBase
{
    public TMP_Text maxHP;
    public TMP_Text damage;
    public TMP_Text armor;
    public TMP_Text moveSpeed;
    public TMP_Text attackSpeed;
    public TMP_Text cooldown;
    public TMP_Text skillRange;
    public TMP_Text skillSpeed;

    private void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        Status status = PlayerManager.Instance.GetPlayer()?.status;

        if (status == null)
            return;

        maxHP.text = status.maxHP.ToString();
        damage.text = status.damage.ToString();
        armor.text = status.armor.ToString();
        moveSpeed.text = status.moveSpeed.ToString();
        attackSpeed.text = status.attackSpeed.ToString();
        cooldown.text = status.decreaseCooldown.ToString();
        skillRange.text = status.skillRange.ToString();
        skillSpeed.text = status.skillSpeed.ToString();
    }
}
