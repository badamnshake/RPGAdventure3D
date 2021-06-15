using RPG.Atrributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] float weaponRange = 5f;
        [SerializeField] float weaponDamage = 0;
        [SerializeField] float percentageBonus = 5f;

        [SerializeField] bool isRightHanded = true;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon weaponPrefab = null;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";


        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                 weapon = Instantiate(weaponPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            // if already have an overirde ->> in the else statement
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {

                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }
        private static void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }



        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile,
                GetTransform(rightHand, leftHand).position,
                Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
        public bool HasProjectile() => projectile != null;

        public float GetDamage() => weaponDamage;

        public float GetRange() => weaponRange;

        private Transform GetTransform(
            Transform rightHand,
            Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public float GetPercentageBonus() => percentageBonus;

    }

}