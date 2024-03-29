using Models;
using UnityEngine;

namespace Weapons {
    public class SimpleTowerTowerWeapon : BaseTowerWeapon {
        protected override WeaponType WeaponType => WeaponType.SimpleWeapon;
        
        public override void Shoot() {
            if (m_shootTarget.IsAlive == false) {
                return;
            }
            var projectile = GetOrCreateProjectile();
            projectile.Initialize(m_shootTarget, m_shootPoint);
            m_lastShotTime = Time.time;
        }
    }
}