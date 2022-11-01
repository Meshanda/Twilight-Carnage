using ScriptableObjects.Variables;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : NetworkBehaviour
{
#region Movement
	[Space(5)]
	[Header("Movement Field")]

	[Tooltip("Max Walk speed of our character")]
	[SerializeField] private FloatVariable _walkSpeed;

	private Vector3 _horizontalAxis;

	private float _speed;
#endregion

#region Rotation
	[Space(5)]
	[Header("Rotation field")]

	[Range(0.0f, 0.3f)]
	[Tooltip("How fast the character face the direction")]
	[SerializeField] private float _rotateSpeed;


	private float _targetRotation;
	private float _rotationVelocity;
#endregion
	
	private CharacterController _controller;
	private GameObject _mainCamera;
	
	// Start is called before the first frame update
	void Start()
	{
		if (Camera.main != null) _mainCamera = Camera.main.gameObject;
		_controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!IsOwner)
			return;
		
		Move();
	}

	private void Move()
	{
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float targetSpeed = _walkSpeed.value;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (_horizontalAxis == Vector3.zero) targetSpeed = 0.0f;

		_speed = targetSpeed;
		
		
		//normalise input direction
		Vector3 inputDirection = _horizontalAxis.normalized;

		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		if (_horizontalAxis != Vector3.zero)
		{
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotateSpeed);

			// rotate to face input direction relative to camera position
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}

		Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		//for controller to slow down when the stick is not at x,y = 1
		targetDirection.x *= Mathf.Abs(_horizontalAxis.x);
		
		// move the player
		_controller.Move(targetDirection * (_speed  * Time.deltaTime));
	}

	public void OnMove(InputValue value)
	{
		Vector2 move = value.Get<Vector2>();
		
		_horizontalAxis = new Vector3(move.x, 0, move.y);
	}
}

