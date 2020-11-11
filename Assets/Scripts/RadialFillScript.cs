using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialFillScript : MonoBehaviour
{
    // Public UI References
    public Image fillImage;
    public Text displayText;
    // Start is called before the first frame update
    void Start()
    {
        fillImage.fillAmount = 0f;
        displayText.text = (0 * 100).ToString("0.00") + "%";
    }

    public void FillPercentage(float fillPercentage)
    {
        fillImage.fillAmount = fillPercentage;
        displayText.text = (fillPercentage * 100).ToString("0.00") + "%";
    }
}
