using System;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Int32 _health;

        public void GetDamage(Int32 damage)
        {
            if (damage < 0) return;
            
            _health -= damage;

            if (_health < 0) Death();
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}
