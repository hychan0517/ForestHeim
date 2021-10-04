using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Jump
    private Rigidbody _rigidbody;
    private RaycastHit[] hits = new RaycastHit[10];
    private bool _isGround;
    //private bool _isRight;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isGround = true;
    }
	private void FixedUpdate()
	{
		
	}

	private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * 0.075f;
            //_isRight = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * 0.075f;
            //_isRight = true;
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (IsGround())
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(new Vector3(0, 400, 0));
                StartCoroutine(Co_JumpDelay());
            }
    //        else if (_isGround)
    //        {
    //            if (_isRight)
    //            {
    //                _rigidbody.velocity = Vector3.zero;
    //                _rigidbody.AddForce(new Vector3(200, 300, 0));
    //            }
    //            else
				//{
    //                _rigidbody.velocity = Vector3.zero;
    //                _rigidbody.AddForce(new Vector3(-200, 300, 0));

    //            }
    //            StartCoroutine(Co_JumpDelay());
    //        }
        }
    }
    
    private IEnumerator Co_JumpDelay()
    {
        _isGround = false;
        yield return new WaitForSeconds(0.3f);
        _isGround = true;
    }

    private bool IsGround()
    {
        if (_isGround == false)
        {
            return false;
        }
        else
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            hits = new RaycastHit[10];
            //TODO : 레이어마스크와 Distance
            Physics.RaycastNonAlloc(ray, hits, 0.3f);
            foreach(RaycastHit i in hits)
            {
                if(i.collider && i.collider.CompareTag("Ground"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
