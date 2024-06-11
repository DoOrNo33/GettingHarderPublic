using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage; // 화면을 어둡게 할 Image
    public float fadeDuration = 2.0f; // 페이드 지속 시간

    // 화면을 어둡게 하는 메서드
    public void FadeToBlack()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration) * 0.5f;
            fadeImage.color = color;
            yield return null;
        }

        // 완전히 어두운 상태로 설정
        color.a = 0.5f;
        fadeImage.color = color;
    }
}
