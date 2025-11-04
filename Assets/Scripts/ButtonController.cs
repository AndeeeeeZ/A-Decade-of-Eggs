using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonUnpressed;

    [SerializeField]
    private Sprite Pressed, Unpressed;

    [SerializeField]
    private bool willBounceBack;

    [SerializeField]
    private AudioClip onClip, offClip;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int amountInContact;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        SwitchSpriteTo(Unpressed);
        amountInContact = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Egg"))
        {
            OnButtonPressed?.Invoke();
            SwitchSpriteTo(Pressed);
            audioSource.PlayOneShot(onClip);
            amountInContact++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (willBounceBack)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Egg"))
            {
                amountInContact--;
                if (amountInContact == 0)
                {
                    OnButtonUnpressed?.Invoke();
                    SwitchSpriteTo(Unpressed);
                    audioSource.PlayOneShot(offClip);
                }
            }
        }

    }

    private void SwitchSpriteTo(Sprite s)
    {
        if (s != null)
            spriteRenderer.sprite = s;
    }
}
