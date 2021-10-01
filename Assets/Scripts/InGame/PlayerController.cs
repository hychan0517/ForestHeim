using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Jump
    private Rigidbody _rigidbody;
    private RaycastHit[] hits = new RaycastHit[10];
    private bool _isGround;
    private float _playerSpeed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isGround = true;
    }
    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * 0.05f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * 0.05f;
            _playerSpeed = 
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && IsGround())
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(new Vector3(0, 300, 0));
            StartCoroutine(Co_JumpDelay());
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
