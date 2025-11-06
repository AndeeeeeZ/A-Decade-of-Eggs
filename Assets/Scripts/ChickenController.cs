using UnityEditor;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    [SerializeField]
    private bool Debugging;
    [SerializeField]
    private EggGenerator eggGenerator;

    [SerializeField]
    private AudioClip[] jumpClip, dashClip, dieClip;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float jumpForce, horizontalSpeed, dashSpeed, returnToNormalVelocitySpeed, returnToZeroVelocitySpeed;

    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private bool isMoving, canJump, canJumpAgain;
    private Vector3 startingLocation;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isMoving = false;
        canJump = true;
        canJumpAgain = true;
        startingLocation = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            if (Mathf.Abs(rigidBody.linearVelocityX) > horizontalSpeed)
            {
                rigidBody.linearVelocityX = Mathf.Sign(rigidBody.linearVelocityX) *
                                            Mathf.Lerp(Mathf.Abs(rigidBody.linearVelocityX),
                                                       horizontalSpeed,
                                                       returnToNormalVelocitySpeed * Time.deltaTime);
            }
        }
        else
        {
            rigidBody.linearVelocityX = Mathf.Sign(rigidBody.linearVelocityX) *
                                            Mathf.Lerp(Mathf.Abs(rigidBody.linearVelocityX),
                                                       0f,
                                                       returnToZeroVelocitySpeed * Time.deltaTime);
        }
    }

    public void Move(int x)
    {
        if (Mathf.Abs(rigidBody.linearVelocityX) < horizontalSpeed
            || Mathf.Sign(rigidBody.linearVelocityX) != Mathf.Sign(x))
            rigidBody.linearVelocityX = x * horizontalSpeed;
        isMoving = true;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Rebirth"))
            animator.Play("Walking");
        if (x == -1)
            spriteRenderer.flipX = true;
        if (x == 1)
            spriteRenderer.flipX = false;
    }
    public void StopHorizontalMovement()
    {
        isMoving = false;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Rebirth"))
            animator.Play("Idle");
    }

    public void Jump()
    {
        if (canJump)
        {
            PerformJump();
            PlayOneFrom(jumpClip);
            canJump = false;
        }
        else if (canJumpAgain)
        {
            if (eggGenerator.HaveEggLeft())
            {
                PerformJump();
                eggGenerator.ProduceEgg(spawnPoint.position);
                PlayOneFrom(dashClip);
            }
            else
            {
                if (Debugging)
                    Debug.LogWarning("No more egg available");
            }
            canJumpAgain = false;
        }
        else
        {
            if (Debugging)
                Debug.LogWarning("Player can't jump again");
        }
    }

    private void PerformJump()
    {
        rigidBody.linearVelocityY = 0f;
        rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    public void OnDie()
    {
        if (eggGenerator.GetCurrentEggAmount() == 0)
            BackToSpawnPoint();
        else
        {
            animator.Play("Rebirth"); 
            transform.position = eggGenerator.GetLastEggPosition();
        }

        rigidBody.linearVelocity = Vector2.zero;
        PlayOneFrom(dieClip);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
            OnDie();
    }

    public void TouchFloor()
    {
        canJump = true;
        canJumpAgain = true;
        if (Debugging)
            Debug.Log("Player is touching floor");
    }

    public void BackToSpawnPoint()
    {
        transform.position = startingLocation;
    }

    // Play one audio clip from the clips array
    private void PlayOneFrom(AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("Missing sound effect");
            return;
        }
        int i = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[i]);
    }
}
