using System;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    #region Variables

    public static Action DiceThrown;
    
    private Camera _mainCamera;
    private Rigidbody _selectedRigidbody;
    private readonly List<Vector3> _dragTracking = new List<Vector3>();
    
    [SerializeField] private float throwMultiplier = 50000;
    [SerializeField] private float torqueMultiplier = 50000;
    [SerializeField] private float wallOffset = 2.0f;
    [SerializeField] private Transform[] walls;// 0-left | 1-back | 2-right | 3-front
    [SerializeField] private LayerMask rayPanelLayerMask;
    [SerializeField] private LayerMask diceLayerMask;
    
    #endregion
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) PickUp();
        if (Input.GetMouseButtonUp(0)) Throw();
        Drag();
    }

    #region Private Methods

    private void PickUp()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, diceLayerMask))
        {
            if (hit.collider == null) return;
            _selectedRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
        }
    }
    
    private void Drag()
    {
        if (_selectedRigidbody == null) return;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        _selectedRigidbody.useGravity = false;
        _selectedRigidbody.freezeRotation = true;
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, rayPanelLayerMask))
        {
            float xPos = Mathf.Clamp(hit.point.x,
                walls[0].position.x + wallOffset, walls[2].position.x - wallOffset);
            float zPos = Mathf.Clamp(hit.point.z,
                walls[3].position.z + wallOffset, walls[1].position.z - wallOffset);
            
            // The dice can move only above the table, so that the player does not release the dice outside the table
            _selectedRigidbody.transform.position = new Vector3(xPos, 3, zPos);

            // The history of mouse movement over the last 15 frames,
            // which can then be used to give the throw force and direction and torque
            if (_dragTracking.Count > 15) _dragTracking.RemoveAt(0);
            _dragTracking.Add(new Vector3(hit.point.x, 3, hit.point.z));
        }
    }
    
    private void Throw()
    {
        if (_selectedRigidbody == null) return;

        _selectedRigidbody.useGravity = true;
        _selectedRigidbody.freezeRotation = false;
        
        Vector3 forceDirection = _dragTracking[^1] - _dragTracking[0];
        Vector3 torque = new Vector3(forceDirection.z, forceDirection.y, -forceDirection.x);
        _selectedRigidbody.AddForce(forceDirection * throwMultiplier);
        _selectedRigidbody.AddTorque(torque * torqueMultiplier);
        
        _selectedRigidbody = null;
        DiceThrown?.Invoke();
    }

    #endregion
}
