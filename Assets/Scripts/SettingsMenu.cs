using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider sensitivity_slider;
    [SerializeField] private TMP_Text sensitivity_value_text;
    [SerializeField] private CameraController camera_controller;
    [SerializeField] private GameObject settings_panel;

    void Start()
    {
        if (sensitivity_slider)
        {
            sensitivity_slider.onValueChanged.AddListener(OnSensitivityChanged);
        }

        if (settings_panel)
        {
            settings_panel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSettings();
        }
    }

    void ToggleSettings()
    {
        if (settings_panel)
        {
            settings_panel.SetActive(!settings_panel.activeSelf);
        }
    }

    void OnSensitivityChanged(float value)
    {
        if (camera_controller)
        {
            camera_controller.SetSensitivity(value);
        }

        if (sensitivity_value_text)
        {
            sensitivity_value_text.text = "Sensitivity: " + value.ToString("0.0");
        }
    }
}