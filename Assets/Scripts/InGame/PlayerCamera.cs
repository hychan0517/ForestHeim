using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject _player;
	private float _originYPos;

	private void Awake()
	{
		_originYPos = transform.position.y;
	}
	private void Update()
	{
		transform.position = new Vector3(transform.position.x, _originYPos, transform.position.z);
	}
}
