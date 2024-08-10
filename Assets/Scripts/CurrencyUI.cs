using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    private void Start()
    {
        currencyText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        currencyText.text = CurrencyManager.Instance.GetCurrency().ToString();
    }
}
