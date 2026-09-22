using UnityEngine;

public class CurtainController : MonoBehaviour
{
    [Header("Curtain Configuration")]

    [Range(0f, 100f)]
    public float openingPercentage = 0f;

    public float minimumHeight = 0.10f;

    private Vector3 closedScale;
    private Vector3 closedPosition;

    private float closedHeight;
    private float topY;

    void Start()
    {
        // Save the curtain dimensions and position when fully closed
        closedScale = transform.localScale;
        closedPosition = transform.localPosition;

        closedHeight = closedScale.y;

        // Calculate the fixed position of the upper edge
        topY = closedPosition.y + (closedHeight / 2f);
    }

    void Update()
    {
        // Convert percentage from 0-100 to 0-1
        float opening = openingPercentage / 100f;

        // Calculate curtain height
        float newHeight = Mathf.Lerp(
            closedHeight,
            minimumHeight,
            opening
        );

        // Change curtain height
        Vector3 newScale = closedScale;
        newScale.y = newHeight;
        transform.localScale = newScale;

        // Move the curtain upward so its upper edge stays fixed
        Vector3 newPosition = closedPosition;
        newPosition.y = topY - (newHeight / 2f);
        transform.localPosition = newPosition;
    }
}