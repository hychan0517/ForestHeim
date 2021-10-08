using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Jump
    private Rigidbody _rigidbody;
    private RaycastHit[] hits = new RaycastHit[10];

    private bool _isGround;
    private bool _isJump;
    private bool _isSecondsJump;

    //Move
	private bool _isLeftWall;
    private bool _isRightWall;

    //Rotation
    private Coroutine _rotationgCor;
    private const float ROTATIONG_RATE = 7.0f;

	private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isJump = true;
    }


	private void Update()
    {
        IsGround();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (_isLeftWall == false)
            {
                transform.position += Vector3.left * 5f * Time.deltaTime;
            }
            if(IsRight())
			{
                StopRotationCor();
                _rotationgCor = StartCoroutine(Co_RotationLeft());
			}
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_isRightWall == false)
            {
                transform.position += Vector3.right * 5f * Time.deltaTime;
            }
            if (IsLeft())
            {
                StopRotationCor();
                _rotationgCor = StartCoroutine(Co_RotationRight());
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (_isJump && IsGround())
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(new Vector3(0, 1150, 0));
                StartCoroutine(Co_JumpDelay());
            }
            else if(IsGround() == false && _isJump && _isSecondsJump == false)
			{
                _isSecondsJump = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(new Vector3(0, 1150, 0));
                StartCoroutine(Co_JumpDelay());
            }
		}
    }

    private IEnumerator Co_RotationRight()
	{
        while (IsLeft())
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - ROTATIONG_RATE);
            yield return null;
        }
        _rotationgCor = null;
    }

    private IEnumerator Co_RotationLeft()
	{
        while (IsRight())
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + ROTATIONG_RATE);
            yield return null;
        }
        _rotationgCor = null;
    }

    private void StopRotationCor()
    {
        if (_rotationgCor != null)
        {
            StopCoroutine(_rotationgCor);
            _rotationgCor = null;
        }
    }

    private bool IsLeft()
	{
        float targetDegree = transform.eulerAngles.y - ROTATIONG_RATE;
        if ((targetDegree < -90 && targetDegree > -270) || (targetDegree > 90 && targetDegree < 270))
            return true;
        else
            return false;
	}

    private bool IsRight()
	{
        float targetDegree = transform.eulerAngles.y + ROTATIONG_RATE;
        if ((targetDegree > 90 && targetDegree < 270) || (targetDegree < -90 && targetDegree > -270))
            return true;
        else
            return false;
    }

    
    private IEnumerator Co_JumpDelay()
    {
        _isJump = false;
        yield return new WaitForSeconds(0.3f);
        _isJump = true;
    }

    private bool IsGround()
    {
        return _isGround;
        Ray ray = new Ray(transform.position, Vector3.down);
        hits = new RaycastHit[10];
        //TODO : 레이어마스크와 Distance
        Physics.RaycastNonAlloc(ray, hits, 0.3f);
        foreach (RaycastHit i in hits)
        {
            if (i.collider && (i.collider.CompareTag("Ground") || i.collider.CompareTag("MoveGround")))
            {
                _isSecondsJump = false;
                return true;
            }
        }
        return false;
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("LeftWall"))
        {
            _isLeftWall = true;
        }
        else if (other.CompareTag("RightWall"))
        {
            _isRightWall = true;
        }
        else if (other.CompareTag("OutLine"))
        {
            LogManager.Instance.PrintLog(LogManager.eLogType.Normal, "Game End");
            UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
        }
    }

	private void OnTriggerExit(Collider other)
	{
        if (other.CompareTag("LeftWall"))
        {
            _isLeftWall = false;
        }
        else if (other.CompareTag("RightWall"))
        {
            _isRightWall = false;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.CompareTag("MoveGround"))
		{
            _isGround = true;
            transform.parent = collision.gameObject.transform;
		}
        else if(collision.collider.CompareTag("Ground"))
		{
            _isGround = true;
        }
    }

	private void OnCollisionExit(Collision collision)
	{
        if (collision.collider.CompareTag("MoveGround") && collision.transform == transform.parent)
        {
            _isGround = false;
            transform.parent = null;
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }
}
