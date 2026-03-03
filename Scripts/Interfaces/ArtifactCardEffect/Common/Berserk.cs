using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : IArtifactCardEffect
{
    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerController player = PlayerManager.Instance.GetPlayer();

        player.AddModifier(new ConditionalModifier(
            StatType.Damage,
            ModifierType.Multiply,
            () =>
            {
                if (player.status.maxHP <= 0)
                    return 0f;

                float ratio = player.status.hp / player.status.maxHP;

                // 80% 이상이면 효과 없음
                if (ratio >= 0.8f)
                    return 0f;

                // 80% → 0, 10% → 1
                float t = (0.8f - ratio) / 0.7f;
                t = Mathf.Clamp01(t);

                // 현재 카드 레벨 가져오기
                int level = CardManager.Instance
                    .artifactEffectLevel[card.effectName];

                if (level <= 0)
                    return 0f;

                // 레벨 기반 최대 증가량 계산
                float maxValue = card.baseAmount +
                                 (card.amountPerLevel * (level - 1)) +
                                 (level == 5 ? card.amountByMaxLevel : 0);

                // 0% → maxValue까지 선형 증가
                return maxValue * t;
            },
            () => true
            ));
    }
}
