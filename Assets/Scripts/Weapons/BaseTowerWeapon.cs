using System.Collections.Generic;
using System.Linq;
using Managers;
using Models;
using Monsters;
using Projectiles;
using Towers;
using UnityEngine;
using static Models.GameSettings;

namespace Weapons {
    public abstract class BaseTowerWeapon: MonoBehaviour, IWeapon {
        public BaseProjectile m_weaponProjectile;
        public Transform m_shootPoint;
        
        protected BaseTowerWeaponSettings m_weaponSettings; 
        protected GamePool<BaseProjectile> m_projectilePool;
        
        public IDamageable m_shootTarget;
        protected IMovable m_movableTarget;
        
        protected float m_lastShotTime;
        public float SqrShootDistance => m_weaponSettings.m_range * m_weaponSettings.m_range;
        
        protected abstract WeaponType WeaponType { get; }
        
        public abstract void Shoot();

        public virtual void Init() {
            m_projectilePool = PoolManager.GetOrCreatePool<BaseProjectile>();
            m_weaponSettings = ModelsProvider.GameSettings.GetWeaponTowerSettings(WeaponType);
            m_lastShotTime = -m_weaponSettings.m_shootInterval;
        }

        public bool ShootAvailable() {
            return m_lastShotTime + m_weaponSettings.m_shootInterval > Time.time == false || m_shootTarget == null;
        }

        private BaseProjectile CreateProjectile() {
            var projectile = Instantiate(m_weaponProjectile, m_projectilePool.m_poolContainer.transform);
            projectile.transform.position = m_shootPoint.position;
            projectile.gameObject.SetActive(false);
            m_projectilePool.Add(projectile);
            return projectile;
        }

        public void SetTarget(IDamageable target) {
            m_shootTarget = target;
            m_movableTarget = target as IMovable;
        }

        protected BaseProjectile GetOrCreateProjectile() {
            var projectile = m_projectilePool.Get().FirstOrDefault(p => p.IsActive == false && p.ProjectileType == m_weaponProjectile.ProjectileType);
            if (projectile == null) {
                projectile = CreateProjectile();
            }
            return projectile;
        }
    }
}