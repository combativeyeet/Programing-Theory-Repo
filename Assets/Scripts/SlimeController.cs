using UnityEngine;

public class SlimeController : GameManager
{
    private Rigidbody slimeRB;
    private Animator anim;
    private float rbMass;

    private Vector3 forwardRotation = new(0, 90, 0);
    private Vector3 backwardRotation = new(0, 270, 0);

    public LayerMask groundLayer;
    public bool isGrounded = false;
    public bool jump = false;

    private void Update()
    {
        slimeRB = gameObject.GetComponent<Rigidbody>();
        rbMass = slimeRB.mass;
        anim = gameObject.GetComponent<Animator>();

        isGrounded = IsGrounded();
        anim.SetBool("isGrounded", isGrounded);

        anim.SetBool("Jump", jump);
    }

    private void FixedUpdate()
    {
        Walk();
        Jump();
    }

    private void Walk()
    {
        //       float walkAcceleration = 100f;

        float hAxis = Input.GetAxis("Horizontal");
        float hAxisRaw = Input.GetAxisRaw("Horizontal");
        if (hAxis > 0.2 && isGrounded)
        {
            transform.eulerAngles = forwardRotation;
            anim.SetFloat("Speed", hAxisRaw);
        }
        else if (hAxis < -0.2 && isGrounded)
        {
            transform.eulerAngles = backwardRotation;
            anim.SetFloat("Speed", hAxisRaw * -1);
        }
    }

    private void Jump()
    {
        float vAxis = Input.GetAxis("Vertical");
        if (vAxis > 0.2 || vAxis < -0.2)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    private bool IsGrounded()
    {
        float distance = .5f;
        Vector3 direction = Vector3.down;
        Vector3 positionFront = transform.position + new Vector3(0.4f, .2f, 0);
        Vector3 positionBack = transform.position + new Vector3(-0.4f, .2f, 0);

        Debug.DrawRay(positionFront, direction, Color.red);
        Debug.DrawRay(positionBack, direction, Color.red);

        bool hitFront = Physics.Raycast(positionFront, direction, distance, groundLayer);
        bool hitBack = Physics.Raycast(positionBack, direction, distance, groundLayer);

        if (hitFront != false || hitBack != false)
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
    }
}