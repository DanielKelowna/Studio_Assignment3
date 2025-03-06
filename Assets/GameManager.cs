using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float score = 0;

    //A reference to our ballController
    [SerializeField] private BallController ball;

    //A reference for our PinCollection prefab we made in Section 2.2
    [SerializeField] private GameObject pinCollection;

    //A reference for an empty GameObject which we'll
    //use to spawn our pin collection prefab
    [SerializeField] private Transform pinAnchor;

    //A reference for our input manager
    [SerializeField] private InputManager inputManager;

    [SerializeField] private TextMeshProUGUI scoreText;
    private FallTrigger[] fallTriggers;
    private GameObject pinObjects;

    private void Start()
    {
        
        inputManager.OnResetPressed.AddListener(HandleReset);
        SetPins();
    }

    private void HandleReset()
    {
        ball.ResetBall();
        SetPins();
    }

    private void SetPins()
    {
        // Reset score when resetting pins
        score = 0;
        scoreText.text = $"Score: {score}";

        
        GameObject[] oldPins = GameObject.FindGameObjectsWithTag("PinCollection");
        foreach (GameObject obj in oldPins)
        {
            Destroy(obj);
        }

        
        pinObjects = Instantiate(pinCollection, pinAnchor.position, Quaternion.identity, transform);
        
        pinObjects.tag = "PinCollection";

        
        fallTriggers = pinObjects.GetComponentsInChildren<FallTrigger>();
        foreach (FallTrigger pin in fallTriggers)
        {
            pin.OnPinFall.RemoveAllListeners();
            pin.OnPinFall.AddListener(IncrementScore);
        }
    }

    private void IncrementScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }
}
