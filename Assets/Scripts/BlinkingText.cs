using System.Collections;
using UnityEngine;
using TMPro; // Для использования TextMeshPro

public class BlinkingText : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText; // TextMeshProUGUI
    [SerializeField] private float fadeDuration = 0.5f; // Продолжительность одного цикла изменения прозрачности (секунды)
    [SerializeField] private float blinkSpeed = 1f; // Скорость мигания (чем выше значение, тем быстрее мигает)
    [SerializeField] private Color baseColor = Color.red; // Цвет текста, который будет изменяться

    private Color originalColor;

    private void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }

        originalColor = tmpText.color;
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        float halfFadeDuration = fadeDuration / 2f;

        while (true)
        {
            // Плавное изменение прозрачности от оригинального цвета до мигающего
            float elapsedTime = 0f;
            while (elapsedTime < halfFadeDuration)
            {
                float t = elapsedTime / halfFadeDuration;
                Color currentColor = Color.Lerp(originalColor, baseColor, t);
                tmpText.color = new Color(currentColor.r, currentColor.g, currentColor.b, t); // Меняем только альфа-канал
                elapsedTime += Time.unscaledDeltaTime * blinkSpeed;
                yield return null;
            }

            // Плавное возвращение к оригинальной прозрачности
            elapsedTime = 0f;
            while (elapsedTime < halfFadeDuration)
            {
                float t = elapsedTime / halfFadeDuration;
                Color currentColor = Color.Lerp(baseColor, originalColor, t);
                tmpText.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1 - t); // Меняем только альфа-канал
                elapsedTime += Time.unscaledDeltaTime * blinkSpeed;
                yield return null;
            }
        }
    }
}
