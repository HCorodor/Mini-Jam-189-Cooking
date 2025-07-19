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
            if (!isClimbing && Input.GetKeyDown(KeyCode.W) && atBottom)
            {
                StartClimbing();
            }
            anim.SetBool("isInteracting", isClimbing);
        }
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            body.velocity = new Vector2(0, verticalInput * climbSpeed);

            anim.SetBool("isInteracting", verticalInput != 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopClimbing();
            }
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        body.gravityScale = -1;
        body.velocity = Vector2.zero;
        anim.SetBool("isInteracting", true);
    }

    private void StopClimbing()
    {
        isClimbing = false;
        body.gravityScale = 3f;
        anim.SetBool("isInteracting", false);
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
