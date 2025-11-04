using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Vector3 startLocation, endLocation;
    [SerializeField]
    private float speed;
    private Vector3 targetLocation;
    private Rigidbody2D rigidBody;

    private void Start()
    {
        targetLocation = startLocation;
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rigidBody == null)
            transform.position = Vector3.Lerp(transform.position, targetLocation, speed * Time.deltaTime);
        else
            rigidBody.MovePosition(Vector3.Lerp(transform.position, targetLocation, speed * Time.deltaTime));

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
