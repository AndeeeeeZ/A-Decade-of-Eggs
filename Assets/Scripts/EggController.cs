using UnityEngine;

public class EggController : MonoBehaviour
{
    [SerializeField] private float eggSpeed;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private int moveInput; 

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalColor = spriteRenderer.color;
        MarkAsLastEgg();
    }

    private void Update()
    {
        rigidBody2D.linearVelocityX = moveInput * eggSpeed;
    }

    public void Move(int x)
    {
        moveInput = x; 
    }

    public void MarkAsLastEgg()
    {
        spriteRenderer.color = Color.red;
        if (spriteRenderer.color == null)
        {
            Debug.LogWarning("Egg missing sprite renderer color");
        }
    }

    public void UnmarkAsLastEgg()
    {
        spriteRenderer.color = originalColor;
    }
}
