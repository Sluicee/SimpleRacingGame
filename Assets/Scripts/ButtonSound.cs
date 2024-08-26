using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip soundClip; // Звук для кнопки
    [SerializeField] private AudioSource audioSource; // AudioSource, через который будет воспроизводиться звук

    private void Start()
    {
        // Получаем компонент Button и добавляем слушатель нажатия на кнопку
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    private void PlaySound()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }
}
