using UnityEngine;

public class RadialBeltController : MonoBehaviour, IFireable
{

    public Transform beltMount;
    public RadialBelt _equippedBelt;



    //public void EquipBelt() => EquipBelt(_equippedBelt);
    //public void EquipBelt(RadialBelt radialBelt)
    //{

    //    if (_equippedBelt != null)
    //    {
    //        Destroy(_equippedBelt.gameObject);
    //    }
    //    _equippedBelt = Instantiate(radialBelt, beltMount.position, beltMount.rotation) as RadialBelt;
    //    _equippedBelt.transform.parent = beltMount;

    //}

    public void OnTriggerHold()
    {
        if (_equippedBelt != null) _equippedBelt.OnTriggerHold();
    }
    public void OnTriggerRelease()
    {
        if (_equippedBelt != null) _equippedBelt.OnTriggerRelease();
    }

}
