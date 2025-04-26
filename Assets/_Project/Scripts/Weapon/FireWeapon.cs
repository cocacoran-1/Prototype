using UnityEngine;

namespace Game.Weapon
{
    public class FireWeapon : WeaponBase
    {
        public enum FireSkillType { FireBall, FireZone, FireThrower, FirePillar }
        [SerializeField] private FireSkillType currentSkill;
        [SerializeField] private SkillData skillData;

        private IWeaponSkill skillBehavior;

        public override void Init()
        {
            base.Init();
            switch (currentSkill)
            {
                case FireSkillType.FireBall:
                    if (skillData is FireBallSkillData fireBallData)
                    {
                        Debug.Log("Creating FireBallSkill");
                        skillBehavior = new FireBallSkill(fireBallData);
                    }
                    else
                        Debug.LogError("SkillData is not FireBallSkillData!");
                    break;
                case FireSkillType.FireZone:
                    if (skillData is FireZoneSkillData fireZoneData)
                        skillBehavior = new FireZoneSkill(fireZoneData);
                    else
                        Debug.LogError("SkillData is not FireZoneSkillData!");
                    break;
                case FireSkillType.FireThrower:
                    if (skillData is FireThrowerSkillData fireThrowerData)
                        skillBehavior = new FireThrowerSkill(fireThrowerData);
                    else
                        Debug.LogError("SkillData is not FireThrowerSkillData!");
                    break;
                case FireSkillType.FirePillar:
                    if (skillData is FirePillarSkillData firePillarData)
                        skillBehavior = new FirePillarSkill(firePillarData);
                    else
                        Debug.LogError("SkillData is not FirePillarSkillData!");
                    break;
            }
            skillBehavior.Setup(this);
        }

        public override void FireSkill()
        {
            timer += Time.deltaTime;
            if (timer >= skillCooldown)
            {
                timer = 0f;
                skillBehavior?.Execute();
            }
        }
        private void Start()
        {
            Init();
        }
        private void Update()
        {
            FireSkill(); // Ensure FireSkill is called
        }
    }
}