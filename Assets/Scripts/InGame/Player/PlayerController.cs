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

    //Up
    private GameObject _ladderObject;
    private bool _isLadder = false;


	private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


	private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && _isLadder == false)
        {
            if (_isLeftWall == false)
            {
                transform.position += Vector3.left * 5f * Time.deltaTime;
            }
            if (IsRight())
            {
                StopRotationCor();
                _rotationgCor = StartCoroutine(Co_RotationLeft());
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) && _isLadder == false)
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

        if (_isJump && _isLadder == false)
            CheckDownGround();

        if (Input.GetKey(KeyCode.Space) && _isLadder == false)
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


        //Ladder
        if(Input.GetKey(KeyCode.UpArrow) && _isLadder == false && _ladderObject != null)
        {
            _isLadder = true;
            transform.position = new Vector3(_ladderObject.transform.position.x, transform.position.y, transform.position.z);
            SetGroundState();
        }
        if(Input.GetKey(KeyCode.UpArrow) && _isLadder && _ladderObject)
        {
            transform.position += Vector3.up * 5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) && _isLadder && _ladderObject)
        {
            transform.position += Vector3.down * 5f * Time.deltaTime;
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
                    SetGroundState();
                    return;
                }
            }
        }

        _isGround = false;
        _rigidbody.isKinematic = false;
    }

    private void SetGroundState()
    {
        _isGround = true;
        _isSecondsJump = false;
        _rigidbody.isKinematic = true;
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
        else if (other.CompareTag("Ladder"))
        {
            if (_ladderObject == null)
                _ladderObject = other.gameObject;
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
        else if (other.CompareTag("Ladder") && other.gameObject == _ladderObject)
        {
            _ladderObject = null;
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
        if (collision.collider.CompareTag("MoveGround") && collision.transform == transform.parent)
        {
            transform.parent = null;
        }
    }
}
