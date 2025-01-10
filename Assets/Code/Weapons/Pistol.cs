namespace Code.Weapons
{
    public class Pistol : Weapon
    {
        private void Awake()
        {
            MaxAmmo = 12;
            Ammo = MaxAmmo;
            MaxMagazineAmmo = 24;
            Magazine = MaxMagazineAmmo;
            Damage = 24;
        }
    }
}
