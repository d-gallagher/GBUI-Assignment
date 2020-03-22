using UnityEngine;

/// <summary>
/// Gun Controller Script
/// </summary>
public class GunController : MonoBehaviour
{
    #region Public Variables
    /// <summary>
    /// Weapon mount position.
    /// </summary>
    public Transform weaponMountPosition;
    /// <summary>
    /// Gun to equip on start.
    /// </summary>
    public Gun startingGun;
    #endregion

    #region Private Variables
    /// <summary>
    /// Currently equipped gun.
    /// </summary>
    private Gun _equippedGun;
    #endregion

    #region Unity Methods
    private void Start()
    {
        if (startingGun != null) EquipGun(startingGun);
    }
    #endregion

    public void EquipGun(Gun gunToEquip)
    {
        if (_equippedGun != null)
        {
            Destroy(_equippedGun.gameObject);
        }
        _equippedGun = Instantiate(gunToEquip, weaponMountPosition.position, weaponMountPosition.rotation) as Gun;
        _equippedGun.transform.parent = weaponMountPosition;
    }

    public void Shoot()
    {
        if (_equippedGun != null)
        {
            _equippedGun.Shoot();
        }
    }
}
