using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Script for tracking and displaying FPS.
/// </summary>
public class FPSTracker : MonoBehaviour
{
    [SerializeField] float secondsPerDisplay;
    [SerializeField] TextMeshProUGUI displayText;

    void Start()
    {
        if (secondsPerDisplay <= 0)
        {
            Debug.LogError("FPSTracker: SecondsPerDisplay must be positive.");
            return;
        }
        StartCoroutine(MeasureFrames());
    }

    IEnumerator MeasureFrames()
    {
        while (true)
        {
            int frames = 0;
            for (float t = 0; t < secondsPerDisplay; t += Time.deltaTime)
            {
                yield return null;
                frames += 1;
            }
            int fps = (int)(frames / secondsPerDisplay);
            displayText.text = "FPS: " + fps;
        }
    }
}
