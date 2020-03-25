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

    public void Aim(Vector3 aimPoint)
    {
        if (_equippedGun != null) _equippedGun.Aim(aimPoint);
    }

    public void Reload()
    {
        if (_equippedGun != null) _equippedGun.Reload();
    }

    public void OnTriggerHold()
    {
        if (_equippedGun != null)_equippedGun.OnTriggerHold();
    }

    public void OnTriggerrelease()
    {
        if (_equippedGun != null)_equippedGun.OnTriggerRelease();
    }

    public float GunHeight => weaponMountPosition.position.y;
}
