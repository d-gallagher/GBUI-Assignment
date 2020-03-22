using UnityEngine;

public class GunController : MonoBehaviour
{
    /// <summary>
    /// Weapon mount position.
    /// </summary>
    public Transform weaponHold;
    /// <summary>
    /// Gun to equip on start.
    /// </summary>
    public Gun startingGun;
    /// <summary>
    /// Currently equipped gun.
    /// </summary>
    private Gun _equippedGun;

    void Start()
    {
        if (startingGun != null) EquipGun(startingGun);
    }

    public void EquipGun(Gun gunToEquip)
    {
        if (_equippedGun != null)
        {
            Destroy(_equippedGun.gameObject);
        }
        _equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        _equippedGun.transform.parent = weaponHold;
    }

    public void Shoot()
    {
        if (_equippedGun != null)
        {
            _equippedGun.Shoot();
        }
    }
}
