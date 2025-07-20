using UnityEngine;

public class InteractWithLadder : MonoBehaviour
{
    private Ladder _currentLadder;
    private Rigidbody2D body;
    private Animator anim;

    private bool isClimbing = false;

    public float climbSpeed = 3f;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentLadder != null)
        {
            bool atBottom = _currentLadder.IsAtBottom(transform.position);
            if (!isClimbing && Input.GetKey(KeyCode.W) && atBottom)
            {
                Debug.Log("Starting climbing");
                anim.SetBool("isClimbing", true);
                StartClimbing();
            }
        }
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            body.linearVelocity = new Vector2(0, verticalInput * climbSpeed);

            anim.SetBool("isClimbing", true);
        
            if (Input.GetKey(KeyCode.S))
            {
                StopClimbing();
            }
        }
        else
        {
            anim.SetBool("isClimbing", false);
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        body.gravityScale = -1;
        body.linearVelocity = Vector2.zero;
        anim.SetBool("isClimbing", true);
    }

    private void StopClimbing()
    {
        Debug.Log("Stopping climbing");
        isClimbing = false;
        body.gravityScale = 3f;
        anim.SetBool("isClimbing", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ladder ladder = other.GetComponentInParent<Ladder>();
        if (ladder != null)
        {
            _currentLadder = ladder;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Ladder ladder = other.GetComponentInParent<Ladder>();
        if (ladder != null && ladder == _currentLadder)
        {
            _currentLadder = null;
            if (isClimbing)
            {
                StopClimbing();
            }
        }
    }
}
