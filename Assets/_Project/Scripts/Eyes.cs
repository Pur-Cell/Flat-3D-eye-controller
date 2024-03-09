using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class Eyes : MonoBehaviour
{
    [SerializeField] Renderer eyesRenderer;

    [Header("Blinking")]
    [SerializeField] int eyeLidTextureXLength = 3;
    [SerializeField] bool isBlinking = false;
    [SerializeField] float timeBetweenBlinks = 3;
    [SerializeField] float blinkFrameStay = 0.1f;

    [Header("Pupil")]
    [SerializeField, Range(0, 1)] float maxPupilOffsetX = 0.3f;
    [SerializeField, Range(0, 1)] float maxPupilOffsetY = 0.13f;

    [Header("UI")]
    [SerializeField] Slider eyeLidSlider;
    [SerializeField] Slider pupilXSlider;
    [SerializeField] Slider pupilYSlider;
    [SerializeField] Toggle blinkToggle;

    private void Awake()
    {
        eyeLidSlider.onValueChanged.AddListener(delegate { HandleEyeLidSliderChanged(); });
        pupilXSlider.onValueChanged.AddListener(delegate { HandlePupilChange(); });
        pupilYSlider.onValueChanged.AddListener(delegate { HandlePupilChange(); });
        blinkToggle.onValueChanged.AddListener(delegate { HandleBlinkToggle(); });
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }

    public void HandleEyeLidSliderChanged()
    {
        SetEyeLidIndex((int)eyeLidSlider.value);
    }

    public void SetEyeLidIndex(int index)
    {
        Mathf.Clamp(index, 0, eyeLidTextureXLength - 1);
        eyesRenderer.material.SetTextureOffset("_EyeLidTex", new Vector2((float)index / eyeLidTextureXLength, 0));
    }

    public void HandlePupilChange()
    {
        ChangePupil(new Vector2(pupilXSlider.value, pupilYSlider.value));
    }

    public void ChangePupil(Vector2 pupilOffset)
    {
        pupilOffset.x = Mathf.Clamp(pupilOffset.x, -1, 1);
        pupilOffset.y = Mathf.Clamp(pupilOffset.y, -1, 1);
        pupilOffset.x *= maxPupilOffsetX;
        pupilOffset.y *= maxPupilOffsetY;

        eyesRenderer.material.SetTextureOffset("_PupilTex", pupilOffset);
    }

    public void HandleBlinkToggle()
    {
        isBlinking = blinkToggle.isOn;
    }

    IEnumerator Blink()
    {
        while (true)
        {
            if(isBlinking)
            {
                yield return new WaitForSeconds(timeBetweenBlinks);
                yield return StartCoroutine(DoBlink());
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator DoBlink()
    {
        int blinkAnimationLengthFrames = eyeLidTextureXLength * 2 - 1;
        for (int i = 1; i < blinkAnimationLengthFrames; i++)
        {
            SetEyeLidIndex((int)Mathf.PingPong(i, eyeLidTextureXLength - 1));
            yield return new WaitForSeconds(blinkFrameStay);
        }
    }
}
