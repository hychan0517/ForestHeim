using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	public GameObject _player;

	/// <summary> 플레이어와 카메라 사이의 거리 </summary>
	private Vector3 _originCameraPosition;
	private Vector3 _targetPosition;
	private Vector3 _startMovePosition;
	private Vector3 _cameraMoveDirection;

	private const float UPDATE_DURATION = 0.3f;
	private readonly WaitForSeconds UPDATE_DURATION_YIELD = new WaitForSeconds(0.3f);
	private const float HEIGHT_RATE = 0.3f;

	private float _deltaTime;
	private Vector3 _lastCameraPosition;
	private float _moveSpeed;
	

	private void Awake()
	{
		_originCameraPosition = transform.position - _player.transform.position;
		_startMovePosition = transform.position;
		_lastCameraPosition = transform.position;
		StartCoroutine(Co_UpdatePlayerPosition());
	}

	private void Update()
	{
		_deltaTime += Time.deltaTime;
		float rate = _deltaTime * (1 / UPDATE_DURATION);
		transform.position = new Vector3(_startMovePosition.x + (_cameraMoveDirection.x * rate), _startMovePosition.y + (_cameraMoveDirection.y * rate), _startMovePosition.z + (_cameraMoveDirection.z * rate));
		_moveSpeed = transform.position.x - _lastCameraPosition.x;
		_lastCameraPosition = transform.position;
	}

	private IEnumerator Co_UpdatePlayerPosition()
	{
		while (true)
		{
			yield return UPDATE_DURATION_YIELD;
			_startMovePosition = transform.position;
			_targetPosition = _player.transform.position + _originCameraPosition;

			if (_targetPosition.x <= -50)
			{
				_targetPosition.x = 0;
			}

			_cameraMoveDirection = new Vector3(_targetPosition.x - _startMovePosition.x, (_targetPosition.y - _startMovePosition.y) * HEIGHT_RATE, _targetPosition.z - _startMovePosition.z);
			_deltaTime = 0;
		}
	}

	public float GetPlayerSpeed()
	{
		return _moveSpeed;
	}
}
