using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	private Camera _camera;
	public PlayerController _player;

	/// <summary> �÷��̾�� ī�޶� ������ �Ÿ� </summary>
	private Vector3 _originCameraPosition;
	private Vector3 _targetPosition;
	private Vector3 _startMovePosition;
	private Vector3 _cameraMoveDirection;
	private Vector3 _lastCameraPosition;
	private Vector3 _lastPlayerPosition;
	private float _deltaTime;
	private float _cameraMoveSpeed;
	private float _playerMoveSpeed;
	private bool _isAuto;
	private float _zoomInOutValue;
	

	private const float UPDATE_DURATION = 0.4f;
	private readonly WaitForSeconds UPDATE_DURATION_YIELD = new WaitForSeconds(0.4f);
	private const float HEIGHT_RATE = 1f;
	//TODO : �ӵ��� ���� ī�޶� ����� �����
	//���� �Ÿ� �̻� �߾��������� �������ٸ� �ִϸ��̼��� ��������
	//Ư�� ��ġ���� originPos�� �����, ī�޶� �浹�̾ƴ� �÷��̾� �浹�� �����ؾ���
	//Ư�� ��ġ���� min,max xy���� �����
	

	private void Awake()
	{
		_camera = GetComponent<Camera>();
		_originCameraPosition = transform.position - _player.transform.position;
		_lastPlayerPosition = _player.transform.position;
		_startMovePosition = transform.position;
		_lastCameraPosition = transform.position;
		StartCoroutine(Co_UpdatePlayerPosition());
		StartCoroutine(Co_ZoomInOut());
	}

	private void Update()
	{
		_deltaTime += Time.deltaTime;
		float rate = _deltaTime * (1 / UPDATE_DURATION);

		if (Mathf.Abs(transform.position.y - _player.transform.position.y) >= 4 && _player._isGround == false)
		{
			if (_isAuto == false)
				Debug.Log("AUTO");
			_isAuto = true;
			transform.position = new Vector3(_startMovePosition.x + (_cameraMoveDirection.x * rate), _player.transform.position.y + 4, _startMovePosition.z + (_cameraMoveDirection.z * rate));
		}
		else
		{
			if (_isAuto)
			{
				Debug.Log("AUTO FALSE");
				_isAuto = false;
				UpdateCameraPosition();
			}
			else
			{
				transform.position = new Vector3(_startMovePosition.x + (_cameraMoveDirection.x * rate), _startMovePosition.y + (_cameraMoveDirection.y * rate / 2), _startMovePosition.z + (_cameraMoveDirection.z * rate));
			}
		}

		//transform.position = new Vector3(_startMovePosition.x + (_cameraMoveDirection.x * rate), _startMovePosition.y + (_cameraMoveDirection.y * rate), _startMovePosition.z + (_cameraMoveDirection.z * rate));
		_cameraMoveSpeed = transform.position.x - _lastCameraPosition.x;
		_lastCameraPosition = transform.position;
		_playerMoveSpeed = Mathf.Abs(_player.transform.position.x - _lastPlayerPosition.x);
		_lastPlayerPosition = _player.transform.position;
	}

	private IEnumerator Co_UpdatePlayerPosition()
	{
		while (true)
		{
			yield return UPDATE_DURATION_YIELD;
			UpdateCameraPosition();
		}
	}

	private void UpdateCameraPosition()
	{
		_startMovePosition = transform.position;
		_targetPosition = _player.transform.position + _originCameraPosition + Vector3.forward * _zoomInOutValue;

		_cameraMoveDirection = new Vector3(_targetPosition.x - _startMovePosition.x, (_targetPosition.y - _startMovePosition.y) * HEIGHT_RATE, _targetPosition.z - _startMovePosition.z);
		_deltaTime = 0;
	}

	private IEnumerator Co_ZoomInOut()
	{
		while(true)
		{
			yield return null;
			if(_playerMoveSpeed > 0)
			{
				if (_zoomInOutValue > -15)
					_zoomInOutValue -= Time.deltaTime * 7.5f;
				else
					_zoomInOutValue = -15f;
			}
			else
			{
				if (_zoomInOutValue < 0)
					_zoomInOutValue += Time.deltaTime * 7.5f;
				else
					_zoomInOutValue = 0;
			}
		}
	}

	public float GetPlayerSpeed()
	{
		return _cameraMoveSpeed;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("CameraChanger"))
		{
			_originCameraPosition = other.GetComponent<CameraChangePosition>()._newOriginPos;
			LogManager.Instance.PrintLog(LogManager.eLogType.Normal, "New Origin Camera Position");
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
