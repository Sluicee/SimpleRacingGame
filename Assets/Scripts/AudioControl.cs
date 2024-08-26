using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Аудиомиксер для управления громкостью
    [SerializeField] private Button muteButton; // Кнопка для управления звуком
    [SerializeField] private Sprite soundOnIcon; // Иконка для включенного звука
    [SerializeField] private Sprite soundOffIcon; // Иконка для выключенного звука

    private bool isMuted = false; // Состояние звука

    private void Start()
    {
        // Присваиваем событие нажатия на кнопку
        muteButton.onClick.AddListener(ToggleMute);

        // Устанавливаем начальное состояние кнопки в зависимости от того, что сохранено
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        UpdateAudioState();
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0); // Сохраняем состояние звука
        UpdateAudioState();
    }

    private void UpdateAudioState()
    {
        // Управляем громкостью через аудиомиксер
        audioMixer.SetFloat("MasterVolume", isMuted ? -80f : 0f); // -80 dB — минимальное значение громкости, фактически тишина

        // Меняем иконку на кнопке
        muteButton.image.sprite = isMuted ? soundOffIcon : soundOnIcon;
    }
}
