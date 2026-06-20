using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private TMP_Text weapon_name_text;
    [SerializeField] private TMP_Text ammo_text;
    [SerializeField] private AudioClip switch_sfx;

    private int current_weapon_index = 0;
    private AudioSource audio_source;
    private GunBehavior current_gun;

    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        EquipWeapon(0);
    }

    void Update()
    {
        HandleWeaponSwitch();

        if (current_gun)
        {
            UpdateAmmoDisplay();
        }
    }

    void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            EquipWeapon((current_weapon_index + 1) % weapons.Length);
        }
        else if (scroll < 0f)
        {
            EquipWeapon((current_weapon_index - 1 + weapons.Length) % weapons.Length);
        }
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i]) weapons[i].SetActive(false);
        }

        current_weapon_index = index;

        if (weapons[current_weapon_index])
        {
            weapons[current_weapon_index].SetActive(true);
            current_gun = weapons[current_weapon_index].GetComponent<GunBehavior>();
        }

        if (switch_sfx && audio_source)
        {
            audio_source.PlayOneShot(switch_sfx);
        }

        UpdateWeaponNameText();
        UpdateAmmoDisplay();
    }

    void UpdateWeaponNameText()
    {
        if (weapon_name_text && weapons[current_weapon_index])
        {
            weapon_name_text.text = weapons[current_weapon_index].name;
        }
    }

    void UpdateAmmoDisplay()
    {
        if (ammo_text && current_gun)
        {
            ammo_text.text = current_gun.GetAmmoDisplay();
        }
    }

    public int GetCurrentWeaponIndex()
    {
        return current_weapon_index;
    }
}