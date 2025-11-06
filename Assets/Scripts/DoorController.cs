using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Vector3 startLocation, endLocation;
    [SerializeField]
    private float speed;
    private Vector3 targetLocation;
    private Rigidbody2D rb;

    private void Start()
    {
        targetLocation = startLocation;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (rb == null)
            transform.position = Vector3.Lerp(transform.position, targetLocation, speed * Time.deltaTime);
        else
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime));
        //rigidBody.MovePosition(Vector3.Lerp(transform.position, targetLocation, speed * Time.deltaTime));

    }
    public void OnButtonPressed()
    {
        targetLocation = endLocation;
    }

    public void OnButtonUnpressed()
    {
        targetLocation = startLocation;
    }
}
