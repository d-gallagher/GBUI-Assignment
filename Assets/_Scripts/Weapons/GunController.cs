using UnityEngine;

/// <summary>
/// Gun Controller Script
/// </summary>
public class GunController : MonoBehaviour
{
    #region Public Variables
    public Transform weaponMountPosition;
    public Gun[] allGuns;
    public float GunHeight => weaponMountPosition.position.y;
    #endregion

    #region Private Variables
    private Gun _equippedGun;
    #endregion

    #region Unity Methods
    private void Start() { }
    #endregion

    #region Implementation of IFireable
    public void OnTriggerHold()
    {
        if (_equippedGun != null) _equippedGun.OnTriggerHold();
    }

    public void OnTriggerRelease()
    {
        if (_equippedGun != null) _equippedGun.OnTriggerRelease();
    }
    #endregion

    #region Public Methods
    public void EquipGun(int weaponIndex) => EquipGun(allGuns[weaponIndex]);

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
    #endregion
}
