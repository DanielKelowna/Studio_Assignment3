using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed;

    private Rigidbody rb;

    private void Start()
    {
        // Adding MovePlayer as a listener to the OnMove event
        inputManager.OnMove.AddListener(MovePlayer);
        rb = GetComponent<Rigidbody>();
    }

    // This is similar to our code from our Roll-A-Ball tutorial
    // Only difference being, we only listen to Left and Right inputs
    private void MovePlayer(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
        rb.AddForce(speed * moveDirection);
    }
}
