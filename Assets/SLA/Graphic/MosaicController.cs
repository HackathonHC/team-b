using UnityEngine;
using System.Collections;

namespace SLA
{
    [RequireComponent(typeof(Camera), typeof(Mosaic))]
    public class MosaicController : MonoBehaviour
    {
        Mosaic mMosaic;
        float mTimeCount;
        [SerializeField] AnimationCurve pixelCount;

         public void Play(float pixelCount = 16f)
        {
            if (mMosaic == null)
            {
                mMosaic = GetComponent<Mosaic>();
            }
            if (mMosaic != null)
            {
                if(SystemInfo.supportsRenderTextures)
                {
                    mTimeCount = 0f;
                    mMosaic.enabled = true;
                    mMosaic.pixelCount = pixelCount;
                }
            }
        }

        void Update()
        {
            if (mMosaic != null)
            {
                if (mMosaic.enabled)
                {
                    mMosaic.pixelCount = pixelCount.Evaluate(mTimeCount);
                    mTimeCount += Time.deltaTime;
                    if (mTimeCount >= pixelCount.length)
                    {
                        mMosaic.enabled = false;
                    }
                }
            }
        }
    }
}
