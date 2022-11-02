using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerTarget : NetworkBehaviour
{
#region Rotation
    [Space(5)]
    [Header("Rotation field")]

    [Range(0.0f, 0.3f)]
    [Tooltip("How fast the character face the direction")]
    [SerializeField] private float _rotateSpeed;


    private Vector3 _playerLookAt;
    private float _targetRotation;
    private float _rotationVelocity;
#endregion

    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private Camera _camera;
    private GameObject _spawnedObj;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;

        Rotate();
    }

    private void Rotate()
    {
        _playerLookAt.Normalize();
        
        _targetRotation = Mathf.Atan2(_playerLookAt.x, _playerLookAt.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotateSpeed);
        
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
    
    protected void OnTargetMouse(InputValue value)
    {
        if (!IsOwner)
            return;
        
        Vector2 mousePosition = value.Get<Vector2>();

        Ray mouseRay = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit rayHit, float.PositiveInfinity, _terrainLayer))
        {
            _playerLookAt = rayHit.point - transform.position;
        }
    }
    
    protected void OnTargetGP(InputValue value)
    {
        if (!IsOwner)
            return;
        
        Vector2 gamePadLA = value.Get<Vector2>();

        _playerLookAt = new Vector3(gamePadLA.x, 0, gamePadLA.y);
    }

    protected void OnShoot()
    {
        
    }
}
