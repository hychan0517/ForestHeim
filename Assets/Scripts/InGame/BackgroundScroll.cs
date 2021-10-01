using GlobalDefine;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
	private MeshRenderer _renderer;
    [SerializeField] private float SPEED_WEIGHT = 0.0001f;
	private void Start()
	{
		_renderer = GetComponent<MeshRenderer>();
	}
	private void LateUpdate()
	{
		Vector2 offset = _renderer.material.GetTextureOffset(Define.TEXTURE_NAME);
		_renderer.material.SetTextureOffset(Define.TEXTURE_NAME, new Vector2(offset.x + AppInGame.Ins.GetPlayerSpeed() * SPEED_WEIGHT, 0));
	}
}
