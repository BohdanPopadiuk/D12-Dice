using System;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    #region Variables

    public static Action DiceSelected;
    public static Action<bool> DiceThrown;//false - Weak Throw | true - Normal Throw

    private readonly List<Vector3> _dragTracking = new List<Vector3>();
    private Vector3 _dragPoint;
    private Vector3 _diceOffset = new Vector3();
    
    private Rigidbody _selectedRigidbody;
    private Camera _mainCamera;

    [SerializeField] private float minThrowVelocity = 10000;
    [SerializeField] private float throwMultiplier = 40000;
    [SerializeField] private float torqueMultiplier = 40000;
    [SerializeField] private float wallOffset = 3.5f;
    [SerializeField] private float dicePosY = 3.0f;
    [SerializeField] private float dragSpeed = 8.0f;
    
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

    private void FixedUpdate()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, rayPanelLayerMask))
        // The history of mouse movement over the last 10 frames,
        // which can then be used to give the throw force and direction and torque
        if (_dragTracking.Count > 10) _dragTracking.RemoveAt(0);
        _dragTracking.Add(new Vector3(hit.point.x, dicePosY, hit.point.z));
    }

    #region Private Methods

    private void PickUp()
    {
        _dragTracking.Clear();
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit diceHit, float.MaxValue, diceLayerMask);
        
        if (diceHit.collider == null) return;
        
        Physics.Raycast(ray, out RaycastHit rayPanelHit, float.MaxValue, rayPanelLayerMask);
        _diceOffset = rayPanelHit.point - diceHit.collider.transform.position;
        
        _selectedRigidbody = diceHit.collider.gameObject.GetComponent<Rigidbody>();
        
        
        _selectedRigidbody.useGravity = false;
        _selectedRigidbody.freezeRotation = true;
        _selectedRigidbody.velocity = Vector3.zero;
        DiceSelected?.Invoke();
    }
    
    private void Drag()
    {
        if (_selectedRigidbody == null) return;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, rayPanelLayerMask))
        {
            float xPos = Mathf.Clamp(hit.point.x - _diceOffset.x,
                walls[0].position.x + wallOffset, walls[2].position.x - wallOffset);
            float zPos = Mathf.Clamp(hit.point.z - _diceOffset.z,
                walls[3].position.z + wallOffset, walls[1].position.z - wallOffset);
            
            Cursor.visible = Vector3.Distance(hit.point, new Vector3(xPos, hit.point.y, zPos)) > 4.0f;
            
            // The die can only move above the table and also has a delay so it doesn't accelerate to extreme speed
            Vector3 diceTargetPos = new Vector3(xPos, dicePosY, zPos);
            _selectedRigidbody.transform.position = Vector3.Lerp(_selectedRigidbody.transform.position, diceTargetPos,
                dragSpeed * Time.deltaTime);
        }
    }
    
    private void Throw()
    {
        if (_selectedRigidbody == null) return;
        Cursor.visible = true;

        _selectedRigidbody.useGravity = true;
        _selectedRigidbody.freezeRotation = false;
        
        //throw direction and force based on cursor history
        Vector3 forceDirection = _dragTracking[^1] - _dragTracking[0];
        //limiting the speed of the cube so that it does not fly out of orbit :)
        forceDirection = Vector3.ClampMagnitude(forceDirection, 4);
        
        Vector3 throwForce = forceDirection * throwMultiplier;
        Vector3 torque = new Vector3(forceDirection.z, forceDirection.y, -forceDirection.x) * torqueMultiplier;
        
        _selectedRigidbody.AddForce(throwForce * Time.deltaTime);
        _selectedRigidbody.AddTorque(torque * Time.deltaTime);
        
        _selectedRigidbody = null;
        
        DiceThrown?.Invoke(NormalThrow(throwForce));//send the throw status
    }

    private bool NormalThrow(Vector3 throwForce) //checking that the diсe has enough speed for the number to be random
    {
        return Math.Abs(throwForce.x) > minThrowVelocity ||
               Math.Abs(throwForce.y) > minThrowVelocity ||
               Math.Abs(throwForce.z) > minThrowVelocity;
    }
    
    #endregion
}
