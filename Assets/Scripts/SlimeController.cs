using UnityEngine;

public class SlimeController : GameManager
{
    private Rigidbody slimeRB;
    private Animator anim;

    private Vector3 forwardRotation = new(0, 90, 0);
    private Vector3 backwardRotation = new(0, 270, 0);

    public LayerMask groundLayer;
    public LayerMask enemy;

    public GameObject canvasGameObject;

    public bool isGrounded = false;
    public bool jump = false;

    private void Start()
    {
        slimeRB = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();

        anim.SetFloat("Speed", 0.0f);
        anim.SetBool("isGrounded", false);
        anim.SetBool("Jump", false);
    }
    private void Update()
    {
        isGrounded = IsGrounded();
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Jump", jump);

        slimeRB.AddForce(Physics.gravity * 5);
    }

    public void FixedUpdate()
    {
        Walk();
        Jump();
    }

    private void Walk()
    {
        float hAxis = Input.GetAxis("Horizontal");

        if (hAxis > 0.3f && isGrounded)
        {
            transform.eulerAngles = forwardRotation;
            anim.SetFloat("Speed", 1);
        }
        else if (hAxis < -0.3f && isGrounded)
        {
            transform.eulerAngles = backwardRotation;
            anim.SetFloat("Speed", 1);
        }
        else if (hAxis < 0.3f && hAxis > -0.3f)
        {
            anim.SetFloat("Speed", 0);
        }
        else if (!isGrounded && !jump)
        {
            anim.SetFloat("Speed", 0);
        }
    }

    private void Jump()
    {
        float vAxis = Input.GetAxisRaw("Vertical");
        if (vAxis == 1)
        {
            jump = true;
        }
        else if (isGrounded)
        {
            jump = false;
        }
    }

    private bool IsGrounded()
    {
        float distance = 1f;
        Vector3 direction = Vector3.down;
        Vector3 positionFront = transform.position + new Vector3(0.4f, 0.6f, 0);
        Vector3 positionMiddle = transform.position + new Vector3(0, 0.6f, 0);
        Vector3 positionBack = transform.position + new Vector3(-0.4f, 0.6f, 0);

        Debug.DrawRay(positionFront, direction, Color.red);
        Debug.DrawRay(positionMiddle, direction, Color.red);
        Debug.DrawRay(positionBack, direction, Color.red);

        bool hitFront = Physics.Raycast(positionFront, direction, distance, groundLayer);
        bool hitMiddle = Physics.Raycast(positionMiddle, direction, distance, groundLayer);
        bool hitBack = Physics.Raycast(positionBack, direction, distance, groundLayer);

        bool hitFrontEnemy = Physics.Raycast(positionFront, direction, distance, enemy);
        bool hitMiddleEnemy = Physics.Raycast(positionMiddle, direction, distance, enemy);
        bool hitBackEnemy = Physics.Raycast(positionBack, direction, distance, enemy);

        if (hitFront || hitMiddle || hitBack || hitFrontEnemy || hitMiddleEnemy || hitBackEnemy)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // destroy all game objects that enter this area
        if (other.gameObject.CompareTag("DeathZone"))
        {
            Death(playerSpawnPos, player);
        }
        if (other.gameObject.CompareTag("Kill"))
        {
            GameObject otherSlime = (((other.gameObject).transform.parent.gameObject).transform.parent.gameObject).transform.parent.gameObject;
            Death(otherSlime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Win");
            canvasGameObject.SetActive(true);
        }
    }

}