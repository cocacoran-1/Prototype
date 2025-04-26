namespace Game.Weapon
{
    public interface IWeaponSkill
    {
        void Setup(WeaponBase context);
        void Execute();
    }
}
