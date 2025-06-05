using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float deactivateDelay = 1f;

    private CinemachineImpulseSource impulseSource;

    private void OnEnable()
    {
        if (impulseSource == null)
            impulseSource = GetComponent<CinemachineImpulseSource>();

        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        // Sau khi shake xong thì tự tắt GameObject
        Invoke(nameof(DeactivateSelf), deactivateDelay);
    }

    private void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}

