using UnityEngine;
using DG.Tweening;

public class HammerSwing : MonoBehaviour
{
    [SerializeField] public float swingForce;

    [SerializeField] private float rotationBaseSpeed;

    [SerializeField] private float rotationMinDuration;

    [SerializeField] private float timeToDestroy;

    [SerializeField] private float swingMaxAngle;

    [SerializeField] private AudioClip impactSound;

    public const float maxForce = 15f;

    private AudioSource audioSource;

    private CapsuleCollider hammerCollider;

    private MeshRenderer hammerMeshRenderer;

    private Tween swingTween;

    // Start is called before the first frame update
    void Start()
    {
        swingForce = Mathf.Min(maxForce, swingForce);

        Debug.Log("Spawn Hammer with: " + swingForce);
        audioSource = GetComponent<AudioSource>();

        hammerCollider = GetComponentInChildren<CapsuleCollider>();

        hammerMeshRenderer = GetComponentInChildren<MeshRenderer>();

        var swingDuration = Mathf.Max(rotationBaseSpeed / swingForce, rotationMinDuration);

        swingTween = transform.DORotate(new Vector3(0, 0, swingMaxAngle), swingDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBack);

        swingTween.OnComplete(DestroyHammer);
    }

    public void StopSwing()
    {
        swingTween.Kill();
        hammerCollider.enabled = false;

        audioSource.PlayOneShot(impactSound);

        DestroyHammer();
    }

    void DestroyHammer()
    {
        foreach (var material in hammerMeshRenderer.materials)
        {
            material.DOFade(0, timeToDestroy).SetEase(Ease.OutQuad);
        }
        Destroy(gameObject, timeToDestroy);
    }
}
