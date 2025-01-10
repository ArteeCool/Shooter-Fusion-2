using System;
using System.Collections.Generic;
using Code.Weapons;
using TMPro;
using UnityEngine;

namespace Code.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        
        [SerializeField] private GameObject _decalPrefab;
        
        [SerializeField] private Weapon _currentWeapon;

        [SerializeField] private TMP_Text _bullets;

        private List<GameObject> _decals;
        
        private List<Weapon> _weapons;
        
        private Boolean _isReloading;
        
        private void Start()
        {
            var pistol = gameObject.AddComponent<Pistol>();
            _decals = new List<GameObject>();
            _weapons = new List<Weapon> { pistol };
            _currentWeapon = _weapons[0];
            UpdateBulletsCount();
        }

        private void Update()
        {
            if (_isReloading) return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (_currentWeapon == null) return;
            if (_currentWeapon.Ammo <= 0) return;
            
            _currentWeapon.Ammo--;
            UpdateBulletsCount();
            Vector3? position = _currentWeapon.Shoot(_firePoint, _firePoint.forward);
            
            if (position != null) 
                _decals.Add(Instantiate(_decalPrefab, position.Value, _firePoint.rotation));
        }

        private void Reload()
        {
            if (_currentWeapon == null) return;
            _currentWeapon.Reload();
            UpdateBulletsCount();
        }

        private void UpdateBulletsCount()
        {
            _bullets.text = $"{_currentWeapon.Magazine}/{_currentWeapon.Ammo}";
        }
    }
}
