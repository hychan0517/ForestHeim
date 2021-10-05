using GlobalDefine;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
	private MeshRenderer _renderer;
	private float _originPosY;
	private Vector3 _originDirection;

    [SerializeField] private float SPEED_WEIGHT = 0.0001f;
	private void Start()
	{
		_originDirection = transform.position - Camera.main.transform.position;
		_renderer = GetComponent<MeshRenderer>();
		_originPosY = transform.position.y;
	}
	private void LateUpdate()
	{
		Vector2 offset = _renderer.material.GetTextureOffset(Define.TEXTURE_NAME);
		_renderer.material.SetTextureOffset(Define.TEXTURE_NAME, new Vector2(offset.x + AppInGame.Ins.GetPlayerSpeed() * SPEED_WEIGHT, 0));
		//transform.position = new Vector3(transform.position.x, _originPosY, transform.position.z);
		//transform.position = new Vector3(Camera.main.transform.position.x + _originDirection.x, _originPosY, Camera.main.transform.position.z + _originDirection.z);
	}
}
