using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        // Rotate the coin continuously around its Y-axis.
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hello, Unity!");

            // Call the GameManager to update the score.
            GameManager2.Instance.IncrementScore(); // Assuming GameManager is a singleton.

            // Destroy this coin after collection.
            Destroy(gameObject);
        }
    }
}
