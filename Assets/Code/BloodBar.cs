using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    public static float blood;
    public static float maxBlood;
    private Image bloodBar;
    // Start is called before the first frame update
    void Start()
    {
        bloodBar= GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bloodBar.fillAmount = blood / maxBlood;
    }
}
