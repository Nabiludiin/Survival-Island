using UnityEngine;

namespace SurvivalIsland
{
    public enum FireMode { SemiAuto, Auto }

    /// <summary>
    /// Angka satu senjata api. Blueprint: left-4-dead/GunController.md bagian "WeaponData" (#58).
    /// Nilai Base_Pistol (usulan disetujui developer, #85): damage 30, magazine 12, reload 1,6 detik, jarak 60 m, suara 35 m.
    /// ItemData.weaponData merujuk aset ini.
    /// </summary>
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Survival/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public FireMode fireMode = FireMode.SemiAuto;
        public float damage = 30f;
        public float secondsBetweenShots = 0.25f;
        public int magazineSize = 12;
        public int startingMagazine = 12;
        [Tooltip("Kosong = cadangan tak terbatas (Laser Pistol).")]
        public string ammoItemId = "Ammo_9mm";
        public float reloadSeconds = 1.6f;
        public float maxRange = 60f;
        public float spreadBase = 0.4f;
        public float spreadMoving = 1.5f;
        public float spreadPerShot = 0.6f;
        public float spreadMax = 4f;
        public float spreadRecoverPerSec = 6f;
        public float recoilPitch = 1.2f;
        public float recoilYaw = 0.4f;
        public float noiseRadius = 35f;
        public float wearPerShot = 0.25f;
        public string muzzleFlashTag = "VFX_Muzzle";
        public string impactTag = "VFX_Impact";
        public float cameraShakeScale = 0.6f;
    }

    /// <summary>
    /// Statistik akhir senjata. Catatan Langkah 5: WeaponStatCalculator (#113) belum ada, jadi isinya disalin langsung dari
    /// WeaponData. Upgrade dan modifier Lucky Machine (#72) masuk di sini setelah #113 ditulis.
    /// </summary>
    public struct WeaponStats
    {
        public float damage, secondsBetweenShots, reloadSeconds, maxRange;
        public float spreadBase, spreadMoving, spreadPerShot, spreadMax, spreadRecoverPerSec;
        public int magazineSize;

        public static WeaponStats From(WeaponData d)
        {
            return new WeaponStats
            {
                damage = d.damage, secondsBetweenShots = d.secondsBetweenShots, reloadSeconds = d.reloadSeconds,
                maxRange = d.maxRange, spreadBase = d.spreadBase, spreadMoving = d.spreadMoving,
                spreadPerShot = d.spreadPerShot, spreadMax = d.spreadMax, spreadRecoverPerSec = d.spreadRecoverPerSec,
                magazineSize = d.magazineSize
            };
        }
    }
}
