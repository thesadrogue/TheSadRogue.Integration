
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;

namespace ExampleGame.Components
{
    /// <summary>
    /// The Component given to RogueLikeEntities who can engage in combat
    /// </summary>
    public class CombatComponent : RogueLikeComponentBase
    {
        public int BaseDamage { get; }
        public int WeaponDamage { get; private set; }
        public int HitPoints { get; private set; }
        public int BaseDamageResistance { get; }
        public int ItemDamageResistance { get; private set; }
        public int DamageResistance => BaseDamageResistance + ItemDamageResistance;
        public int Damage => BaseDamage + WeaponDamage;
        
        public CombatComponent(int hp, int dr, int baseDamage) : base(false, false, false, false, 1)
        {
            HitPoints = hp;
            BaseDamageResistance = dr;
            BaseDamage = baseDamage;
        }

        public void TakeDamage(int damage) 
            => HitPoints += DamageResistance >= damage ? 0 : DamageResistance - damage;

        public void DealDamage(RogueLikeEntity victim) 
            => victim.GetComponent<CombatComponent>().TakeDamage(Damage);
    }
}