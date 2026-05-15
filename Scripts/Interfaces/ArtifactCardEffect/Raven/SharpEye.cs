using UnityEngine;

// 크리티컬 발생 시 모든 스킬 쿨다운을 일정량 감소시킴
public class SharpEye : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action<int> critCallback;
    private int lastTriggerFrame = -1; // 같은 프레임 내 중복 발동 방지

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        critCallback = OnCritical;
        player.OnCritical += critCallback;
    }

    private void OnCritical(int critLevel)
    {
        // 범위 공격 등 같은 프레임에 여러 번 발동되지 않도록 제한
        if (Time.frameCount == lastTriggerFrame) return;
        lastTriggerFrame = Time.frameCount;

        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        if (level <= 0) return;

        // 크리 단계 관계없이 고정 감소량 적용
        float reduction = card.GetValue(AttributeType.amount, level);

        for (int i = 0; i < player.skillDelay.Length; i++)
        {
            player.skillDelay[i] = Mathf.Max(player.skillDelay[i] - reduction, 0f);
        }
    }

    public void Update() { }

    public void Remove()
    {
        if (critCallback != null)
        {
            player.OnCritical -= critCallback;
            critCallback = null;
        }
    }
}
