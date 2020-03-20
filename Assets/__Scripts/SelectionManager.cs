using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] public Transform raypointer;

    // Transform being raycasted at
    private Transform _raycastedObj;
    // Renderer on transform is used to change the material
    private Renderer _raycastedObjRenderer;
    private void Start()
    {
    }
    private void Update()
    {
        ShowRaycast();
    }

    private void ShowRaycast()
    {        
        if (_raycastedObj != null)
        {
            _raycastedObjRenderer = _raycastedObj.GetComponent<Renderer>();
            _raycastedObjRenderer.material = defaultMaterial;
            _raycastedObj = null;
        }

        // Retrieve info from raycast
        RaycastHit raycastHit;
        // used to raycast in the Forward direction from our pointer
        Vector3 fwd = raypointer.transform.TransformDirection(Vector3.forward);

        // true if raycast from transform -> forward, in range 15f
        if (Physics.Raycast(raypointer.transform.position, fwd, out raycastHit, 15f))
        {
            _raycastedObj = raycastHit.transform;
            if (_raycastedObj.CompareTag(selectableTag))
            {
                _raycastedObjRenderer = _raycastedObj.GetComponent<Renderer>();

                if (_raycastedObjRenderer != null)
                {
                    // highlight material on raycast
                    _raycastedObjRenderer.material = highlightMaterial;
                    Debug.DrawLine(_raycastedObj.transform.position, raypointer.position, Color.red);
                }
            }
        }

    }
}