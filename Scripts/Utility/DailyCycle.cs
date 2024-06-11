using System.Collections;
using UnityEngine;

public class DailyCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    public int day = 1;

    private void Awake()
    {
        DataManager.Instance.timeSet += SetTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        StartCoroutine(DailyRoutine());
    }

    private void Update()
    {
        DataManager.Instance.UpdateTime(time);
    }

    IEnumerator DailyRoutine()
    {
        while (true) 
        {
            float previousTime = time;
            time = (time + timeRate * Time.deltaTime) % 1.0f;

            if (previousTime > time)
            {
                day++;
                UpdateDayText();
            }

            UpdateLighting(sun, sunColor, sunIntensity);
            UpdateLighting(moon, moonColor, moonIntensity);

            RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
            RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
            yield return null;
        }
    }
    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }

    public event System.Action<int> OnDayChanged;
    void UpdateDayText()
    {
        if (OnDayChanged != null)
        {
            OnDayChanged(day);
        }
    }

    public void SetTime(float value)
    {
        time = value;
    }
}
