using DamageNumbersPro;

public class DamageNumberManager : MonoSingleton<DamageNumberManager>
{
    public DamageNumber shieldHitPrefab;
    public DamageNumber healthHitPrefab;
    public DamageNumber normalCritHitPrefab;
    public DamageNumber superCritHitPrefab;
    public DamageNumber ultraCritHitPrefab;
}
