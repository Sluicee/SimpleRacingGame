using UnityEngine;

[System.Serializable]
public class CarData
{
    public GameObject carPrefab; // Префаб машины
    public Sprite carImage; // Изображение машины
    public Sprite carLockImage; // Изображение замка для машины
    public float carSpeed; // Скорость машины
    public float carHandling; // Управляемость машины
    public float carPower; // Мощность машины
    public int carPrice; // Цена машины
    public bool isUnlocked; // Разблокирована ли машина
}
