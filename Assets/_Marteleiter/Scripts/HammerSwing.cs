using UnityEngine;
using DG.Tweening;

public class HammerSwing : MonoBehaviour
{
    [SerializeField] public float swingForce;

    [SerializeField] private float rotationBaseSpeed;

    [SerializeField] private float rotationMinDuration;

    [SerializeField] private float timeToDestroy;

    [SerializeField] private float swingMaxAngle;

    private MeshCollider hammerMeshCollider;

    private MeshRenderer hammerMeshRenderer;

    private Tween swingTween;

    // Start is called before the first frame update
    void Start()
    {
        hammerMeshCollider = GetComponentInChildren<MeshCollider>();

        hammerMeshRenderer = GetComponentInChildren<MeshRenderer>();

        var swingDuration = Mathf.Max(rotationBaseSpeed / swingForce, rotationMinDuration);

        swingTween = transform.DORotate(new Vector3(0, 0, swingMaxAngle), swingDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBack);

        swingTween.OnComplete(DestroyHammer);
    }

    public void StopSwing()
    {
        swingTween.Kill();
        hammerMeshCollider.enabled = false;

        DestroyHammer();
    }

    void DestroyHammer()
    {
        foreach (var material in hammerMeshRenderer.materials)
        {
            material.DOFade(0, timeToDestroy).SetEase(Ease.OutQuad);
        }
        GameObject.Destroy(gameObject, timeToDestroy);
    }
}
