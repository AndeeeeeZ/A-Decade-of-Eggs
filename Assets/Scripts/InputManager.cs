using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private ChickenController chickenController;
    [SerializeField] private EggGenerator eggGenerator;
    private bool inputEnabled;
    private void Start()
    {
        inputEnabled = false;
    }

    void Update()
    {
        if (inputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                chickenController.Jump();
            }
            ChickenHorizontalMovement();
            EggsInputUpdate();
        }
    }
    private void ChickenHorizontalMovement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            chickenController.Move(1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            chickenController.Move(-1);
        }
        else
        {
            chickenController.StopHorizontalMovement();
        }
    }

    private void EggsInputUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            eggGenerator.Move(1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            eggGenerator.Move(-1);
        }
        else
        {
            eggGenerator.Move(0);
        }
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }

    public void DisableInput()
    {
        inputEnabled = false;
    }
}
