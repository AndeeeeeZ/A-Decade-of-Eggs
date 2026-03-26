using UnityEngine;
using UnityEngine.InputSystem;

public class ChickenController : MonoBehaviour
{
    [SerializeField] private bool Debugging;
    [SerializeField] private EggGenerator eggGenerator;
    [SerializeField] private AudioClip[] jumpClip, dashClip, dieClip;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float jumpForce, horizontalSpeed, dashSpeed, returnToNormalVelocitySpeed, returnToZeroVelocitySpeed;

    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;
    private bool isMoving, canJump, canJumpAgain;
    private Vector3 startingLocation;
    private GameInputActions inputs;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        inputs = new GameInputActions();
    }
    private void Start()
    {
        ResetToStart();
    }

    public void ResetToStart()
    {
        isMoving = false;
        canJump = true;
        canJumpAgain = true;
        startingLocation = transform.position;
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Chicken.Jump.performed += Jump;
        inputs.Chicken.Move.performed += Move;
        inputs.Chicken.Move.canceled += StopHorizontalMovement;
    }

    private void OnDisable()
    {
        inputs.Chicken.Jump.performed -= Jump;
        inputs.Chicken.Move.performed -= Move;
        inputs.Chicken.Move.canceled -= StopHorizontalMovement;
        inputs.Disable();
    }

    private void Update()
    {
        // Slow down to stop player if not moving
        if (!isMoving)
        {
            LerpCurrentHorizontalVelocityTo(0f, returnToZeroVelocitySpeed);
            return;
        }
        // Slow down player to horizontalSpeed if going too fast
        if (Mathf.Abs(rigidBody.linearVelocityX) > horizontalSpeed)
            LerpCurrentHorizontalVelocityTo(horizontalSpeed, returnToNormalVelocitySpeed);

    }

    // Lerp the player's current horizontal velocity to val
    private void LerpCurrentHorizontalVelocityTo(float val, float returnSpeed)
    {
        rigidBody.linearVelocityX = Mathf.Sign(rigidBody.linearVelocityX) *
                                       Mathf.Lerp(Mathf.Abs(rigidBody.linearVelocityX),
                                                  val,
                                                  returnSpeed * Time.deltaTime);
    }

    // Moves by setting horiztonal velocity
    public void Move(InputAction.CallbackContext context)
    {
        int x = (int)context.ReadValue<float>();

        if (Mathf.Abs(rigidBody.linearVelocityX) < horizontalSpeed
            || Mathf.Sign(rigidBody.linearVelocityX) != Mathf.Sign(x))
            rigidBody.linearVelocityX = x * horizontalSpeed;

        isMoving = true;

        // Ensure rebirth animation finishes
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Rebirth"))
            animator.Play("Walking");

        // Flip sprite according to move direction
        if (x == -1)
            spriteRenderer.flipX = true;
        if (x == 1)
            spriteRenderer.flipX = false;
    }

    public void StopHorizontalMovement(InputAction.CallbackContext context)
    {
        isMoving = false;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Rebirth"))
            animator.Play("Idle");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (TryFirstJump()) return;
        if (TrySecondJump()) return;

        if (Debugging)
            Debug.LogWarning("Player can't jump again");
    }

    private bool TryFirstJump()
    {
        if (!canJump) return false;

        PerformJump();
        PlayOneFrom(jumpClip);
        canJump = false;
        return true;
    }

    // Can only jump again if haven't second jumped yet 
    // And if there is eggs left 
    private bool TrySecondJump()
    {
        if (!canJumpAgain) return false;

        if (!eggGenerator.HaveEggLeft())
        {
            if (Debugging)
                Debug.LogWarning("No more egg available");

            canJumpAgain = false;
            return false;
        }

        PerformJump();
        eggGenerator.ProduceEgg(spawnPoint.position);
        PlayOneFrom(dashClip); // Note this is using dashClip instead of jumpClip
        particle.Play();

        canJumpAgain = false;
        return true;
    }

    private void PerformJump()
    {
        rigidBody.linearVelocityY = 0f;
        rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    public void OnDie()
    {
        // Respawn at the start location if there is no eggs left
        // Respawn at the last egg if one exists
        if (eggGenerator.GetCurrentEggAmount() == 0)
            BackToSpawnPoint();
        else
        {
            animator.Play("Rebirth");
            transform.position = eggGenerator.GetLastEggPosition();
            eggGenerator.RemoveAllEggs();
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
        ResetToStart();
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
