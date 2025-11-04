using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class EggController : MonoBehaviour
{
    [SerializeField]
    private float eggSpeed;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        MarkAsLastEgg();
    }

    private void Update()
    {
        CheckYVelocity();
    }
    public void Move(int x)
    {
        if (rigidBody2D == null)
        {
            Debug.LogWarning("Missing rigidbody 2d");
            rigidBody2D = GetComponent<Rigidbody2D>();
            if (rigidBody2D == null)
            {
                Debug.LogWarning("Unable to get egg's rigidbody"); 
            }
        }
        rigidBody2D.linearVelocityX = x * eggSpeed;
    }

    private void CheckYVelocity()
    {
        if (rigidBody2D.linearVelocityY > 0f)
            rigidBody2D.linearVelocityY = 0f;
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
