using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Jump
    private Rigidbody _rigidbody;
    private RaycastHit[] hits = new RaycastHit[10];

    [SerializeField]
    private bool _isGround;
    private bool _isJump = true;
    [SerializeField]
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
    }


	private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (_isLeftWall == false)
            {
                transform.position += Vector3.left * 5f * Time.deltaTime;
                //CheckLeftUpGround();
            }
            if (IsRight())
            {
                StopRotationCor();
                _rotationgCor = StartCoroutine(Co_RotationLeft());
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_isRightWall)
            {
                transform.position += Vector3.right * 5f * Time.deltaTime;
            }
            if (IsLeft())
            {
                StopRotationCor();
                _rotationgCor = StartCoroutine(Co_RotationRight());
            }
        }

        if (_isJump)
            CheckDownGround();

        if (Input.GetKey(KeyCode.Space))
        {
            if (_isJump && _isGround)
            {
                StartCoroutine(Co_JumpDelay());
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(new Vector3(0, 800, 0));
                
            }
            else if(_isGround == false && _isJump && _isSecondsJump == false)
			{
                StartCoroutine(Co_JumpDelay());
                _isSecondsJump = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(new Vector3(0, 800, 0));
            }
		}
    }

    private IEnumerator Co_RotationRight()
	{
        while (IsLeft())
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - ROTATIONG_RATE, transform.eulerAngles.z);
            yield return null;
        }
        _rotationgCor = null;
    }

    private IEnumerator Co_RotationLeft()
	{
        while (IsRight())
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + ROTATIONG_RATE, transform.eulerAngles.z);
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
        _rigidbody.isKinematic = false;
        _isGround = false;
        yield return new WaitForSeconds(0.3f);
        _isJump = true;
    }

    private void CheckDownGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        hits = new RaycastHit[10];
        Physics.RaycastNonAlloc(ray, hits, 1f);
        foreach (RaycastHit i in hits)
        {
            if (i.collider && (i.collider.CompareTag("Ground") || i.collider.CompareTag("MoveGround")))
            {
                float distance = transform.position.y - i.point.y;
                if (distance >= 0f && distance <= 0.2f)
                {
                    transform.position = new Vector3(transform.position.x, i.point.y + 0.1f, transform.position.z);
                    _isGround = true;
                    _isSecondsJump = false;
                    _rigidbody.isKinematic = true;
                    return;
                }
            }
        }

        _isGround = false;
        _rigidbody.isKinematic = false;
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
            transform.parent = collision.gameObject.transform;
		}
    }

	private void OnCollisionExit(Collision collision)
	{
        if (collision.collider.CompareTag("MoveGround") && collision.transform == transform.parent)
        {
            transform.parent = null;
        }
    }
}
