using System;
using Code.Controllers;
using UnityEngine;

namespace Code.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public Int32 Ammo;
        public Int32 Magazine;
        public Int32 Damage;
        
        protected Int32 MaxAmmo;
        protected Int32 MaxMagazineAmmo;

        public Vector3? Shoot(Transform firePoint, Vector3 direction)
        {
            RaycastHit hit;
            Ray ray = new Ray(firePoint.position, direction); 
            
            if (Physics.Raycast(ray, out hit, Int32.MaxValue))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerController>().GetDamage(Damage);
                }
                return hit.point;
            }

            return null;
        }

        public void Reload()
        {
            Int32 ammoToReload = MaxAmmo - Ammo;
            Int32 bulletCountAfterReload = Magazine - ammoToReload;
            if (bulletCountAfterReload > 0)
            {
                Magazine = bulletCountAfterReload;
                Ammo += ammoToReload;
            }
            else
            {
                Ammo += Magazine;
                Magazine = 0;
            }
        }
    }
}
