 using UnityEngine;

namespace SLA
{
    [RequireComponent(typeof(Camera))]
    public class Mosaic : MonoBehaviour
    {
        public Material material;
        public float pixelCount = 8f;

        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            material.SetFloat("_PixelCountX", pixelCount);
            material.SetFloat("_PixelCountY", pixelCount * Screen.height / Screen.width);

            Graphics.Blit (source, destination, material);
        }
    }
}
