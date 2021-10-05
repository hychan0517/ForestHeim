using DG.Tweening;
using GlobalDefine;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
	[SerializeField]
	private eMoveGroundType _moveGroundType = eMoveGroundType.None;
	[SerializeField]
	private Ease _easeType = Ease.Linear;
	[SerializeField]
	private float _moveDistance = 0;
	[SerializeField]
	private float _moveSpeed = 0;

	private Vector3 _originPosition;

	private void Awake()
	{
		switch (_moveGroundType)
		{
			case eMoveGroundType.Left:
				_originPosition = new Vector3(transform.position.x - _moveDistance / 2, transform.position.y, transform.position.z);
				MoveLeft();
				break;
			case eMoveGroundType.Right:
				_originPosition = new Vector3(transform.position.x + _moveDistance / 2, transform.position.y, transform.position.z);
				MoveRight();
				break;
			case eMoveGroundType.Up:
				_originPosition = new Vector3(transform.position.x, transform.position.y + _moveDistance / 2, transform.position.z);
				MoveUp();
				break;
			case eMoveGroundType.Down:
				_originPosition = new Vector3(transform.position.x, transform.position.y - _moveDistance / 2, transform.position.z);
				MoveDown();
				break;
		}
		
	}

	private void MoveLeft()
	{
		transform.DOMoveX(_originPosition.x - _moveDistance / 2, _moveSpeed).SetEase(_easeType).OnComplete(() => { MoveRight(); });
	}

	private void MoveRight()
	{
		transform.DOMoveX(_originPosition.x + _moveDistance / 2, _moveSpeed).SetEase(_easeType).OnComplete(() => { MoveLeft(); });
	}

	private void MoveUp()
	{
		transform.DOMoveY(_originPosition.y + _moveDistance / 2, _moveSpeed).SetEase(_easeType).OnComplete(() => { MoveDown(); });
	}

	private void MoveDown()
	{
		transform.DOMoveY(_originPosition.y - _moveDistance / 2, _moveSpeed).SetEase(_easeType).OnComplete(() => { MoveUp(); });
	}
}
