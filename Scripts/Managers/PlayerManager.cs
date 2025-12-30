using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private PlayerController currentPlayer;

    public void SetPlayer(PlayerController player)
    {
        currentPlayer = player;
    }

    public void GetExp(float exp)
    {
        currentPlayer.status.exp += exp;
        CheckLevelUp();
    }

    public PlayerController GetPlayer()
    {
        return currentPlayer;
    }

    public void CheckLevelUp()
    {
        if (currentPlayer == null)
            return;

        if (currentPlayer.status.exp >= currentPlayer.status.maxExp)
        {
            currentPlayer.status.exp -= currentPlayer.status.maxExp;
            currentPlayer.status.level += 1;
            currentPlayer.status.maxExp *= 1.2f;
            BattleManager.Instance.isStop = true;
            Time.timeScale = 0;
            UIManager.Instance.Show<LevelUpCardFrame>("FloatingUI").Initialize();
        }
    }

    private void Update()
    {
        CheckLevelUp();
    }
}
