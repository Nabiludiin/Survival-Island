export class LuckyMachine {
    constructor(status, audio) {
        this.status = status;
        this.audio = audio;
        this.isStormCharged = false; // Free spin during volcanic eruption!
        this.isOpen = false;
        this.isSpinning = false;

        this.rewards = [
            { id: 'hpBuff', name: 'Nano-Injector: +25 HP Permanen!', type: 'buff', effect: () => { this.status.maxHealth += 25; this.status.health += 25; } },
            { id: 'staminaBuff', name: 'Adrenaline Core: +30 Stamina Maksimal!', type: 'buff', effect: () => { this.status.maxStamina += 30; this.status.stamina += 30; } },
            { id: 'laserWeapon', name: 'Modul Overclock Senjata (Damage Pistol x2)', type: 'buff', effect: () => { this.status.laserMod = true; } },
            { id: 'goldenAxe', name: 'Golden Fire-Axe (Alat Tebang & Melee Super)', type: 'item', effect: () => { this.status.hasGoldenAxe = true; } },
            { id: 'militarySupplies', name: 'Drop Peti Militer (+30 Peluru & 3 Medkit)', type: 'item', effect: () => { this.status.inventory.ammo += 30; this.status.inventory.medkit += 3; } },
            { id: 'immunityVaccines', name: 'Serum Kebal Darurat (+3 Vaksin Sintetis)', type: 'item', effect: () => { this.status.inventory.vaccines += 3; } }
        ];
    }

    setStormCharged(val) {
        this.isStormCharged = val;
    }

    canSpin() {
        if (this.isStormCharged) return { can: true, isFree: true };
        // Normal cost: 5 Stone + 5 Wood as sacrifice/material
        if (this.status.inventory.stone >= 5 && this.status.inventory.wood >= 5) {
            return { can: true, isFree: false };
        }
        return { can: false, reason: "Dibutuhkan 5 Batu & 5 Kayu (atau putar gratis saat badai petir erupsi gunung)!" };
    }

    spin(callback) {
        if (this.isSpinning) return;
        const check = this.canSpin();
        if (!check.can) {
            callback({ success: false, msg: check.reason });
            return;
        }

        if (!check.isFree) {
            this.status.inventory.stone -= 5;
            this.status.inventory.wood -= 5;
        }

        this.isSpinning = true;
        if (this.audio) this.audio.playAlarm();

        // 1.5s spinning suspense
        setTimeout(() => {
            const reward = this.rewards[Math.floor(Math.random() * this.rewards.length)];
            reward.effect();
            this.isSpinning = false;
            if (this.audio) this.audio.playPickup();

            callback({
                success: true,
                reward,
                msg: `🎉 Dihasilkan dari The Lucky Machine: ${reward.name}`
            });
        }, 1500);
    }
}
