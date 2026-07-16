using UnityEngine;

public class SanityUI : MonoBehaviour
{
    [SerializeField] private SanitySystem sanitySystem;
    [SerializeField] private StatusImageUI statusImageUI;

    private void OnEnable()
    {
        sanitySystem.OnSanityUpdated += statusImageUI.UpdateImageUI;
    }

    private void OnDisable()
    {
        sanitySystem.OnSanityUpdated -= statusImageUI.UpdateImageUI;
    }
}
