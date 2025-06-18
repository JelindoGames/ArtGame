using UnityEngine;

/// <summary>
/// Sets the target frame rate.
/// </summary>
public class SetTargetFrameRate : MonoBehaviour
{
    [SerializeField] int targetFrameRate;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
