using System.IO;
using UnityEngine;

public class LevelValidator : MonoBehaviour
{
    [SerializeField] private Texture2D targetTexture;

    [SerializeField] private Material shapeMaterial;

    private RenderTexture renderTexture;

    private bool showResut = false;

    // Start is called before the first frame update
    void Start()
    {
        renderTexture = GetComponent<RenderTexture>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var validatorCamera = GetComponent<Camera>();
            validatorCamera.enabled = true;
            showResut = true;
            shapeMaterial.color = Color.black;
            GetComponent<Camera>().Render();
            shapeMaterial.color = Color.white;
            validatorCamera.enabled = false;
        }
    }

    void OnPostRender()
    {
        if (showResut)
        {
            Debug.Log(GetLevelRating());
            showResut = false;
        }
    }

    private int GetLevelRating()
    {
        var target = targetTexture.GetPixels();

        var shape = ToTexture2D(
            targetTexture.width,
            targetTexture.height
        ).GetPixels();

        var rating = 0f;

        for (int i = 0; i < target.Length; i++)
        {
            rating += (Mathf.Abs(target[i].r - shape[i].r) < 0.1f) ? 1 : 0;
        }

        return Mathf.RoundToInt(100f * rating / target.Length);
    }

    private Texture2D ToTexture2D(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        return texture;
    }
}
