export class SurvivalStatus {
    constructor() {
        // Core Vitals
        this.health = 100;
        this.maxHealth = 100;
        this.hunger = 100;
        this.maxHunger = 100;
        this.thirst = 100;
        this.maxThirst = 100;
        this.infection = 0;       // 0 to 100%
        this.stamina = 100;
        this.maxStamina = 100;
        this.frostbite = 0;       // Region 3 cold effect

        // Modifiers from Rescued NPCs
        this.hasElena = false;    // Reduces infection rate
        this.hasMarcus = false;   // Reduces crafting cost

        // Time & Day Cycle (60 Days Target)
        this.currentDay = 1;
        this.targetDays = 60;
        this.dayDuration = 90;    // 90 seconds per full day (45s day, 45s night)
        this.dayTimer = 15;       // Start at morning (0-45 is Day, 45-90 is Night)
        this.isDay = true;
        this.sunAngle = 0;

        // Inventory
        this.inventory = {
            ammo: 18,
            food: 2,
            water: 2,
            medkit: 1,
            wood: 10,
            stone: 5,
            fish: 0,
            herbs: 0,
            mushrooms: 0,
            vaccines: 1,
            torches: 0,
            warmClothes: false,
            // Helicopter parts (collected from Region 4, assembled in Region 3)
            heliEngine: false,
            heliRotor: false,
            heliFuel: false,
            heliAvionics: false
        };

        // Assembled parts at Helipad (Region 3)
        this.heliInstalled = {
            heliEngine: false,
            heliRotor: false,
            heliFuel: false,
            heliAvionics: false
        };
    }

    applyScavengeResults(results) {
        if (!results) return;
        const inv = results.inventory;
        if (inv) {
            this.inventory.ammo = inv.ammo || 18;
            this.inventory.food = inv.food || 2;
            this.inventory.water = inv.water || 2;
            this.inventory.medkit = inv.medkit || 1;
            this.inventory.vaccines = inv.vaccines || 1;
            this.hasElena = !!inv.rescuedElena;
            this.hasMarcus = !!inv.rescuedMarcus;
        }
    }

    update(dt, currentRegion, isNearFire) {
        // 1. Hunger & Thirst drain
        this.hunger = Math.max(0, this.hunger - dt * 0.35);
        this.thirst = Math.max(0, this.thirst - dt * 0.55);

        // Starvation / Dehydration damage
        if (this.hunger <= 0) {
            this.takeDamage(dt * 2.5, "Kelaparan");
        }
        if (this.thirst <= 0) {
            this.takeDamage(dt * 3.5, "Kehausan");
        }

        // 2. Infection progression
        if (this.infection > 0) {
            const infRate = this.hasElena ? 0.35 : 0.8;
            this.infection = Math.min(100, this.infection + dt * infRate);
            if (this.infection >= 100) {
                this.takeDamage(dt * 8.0, "Wabah Infeksi Zombie Parah");
            }
        }

        // 3. Frostbite in Region 3 (Snow Mountain)
        if (currentRegion === 3 && !this.inventory.warmClothes && !isNearFire) {
            this.frostbite = Math.min(100, this.frostbite + dt * 4.0);
            if (this.frostbite >= 60) {
                this.takeDamage(dt * 2.0, "Hipotermia / Suhu Beku Puncak Salju");
            }
        } else {
            this.frostbite = Math.max(0, this.frostbite - dt * 8.0);
        }

        // 4. Stamina regen
        this.stamina = Math.min(this.maxStamina, this.stamina + dt * 15.0);

        // 5. Day / Night cycle
        this.dayTimer += dt;
        if (this.dayTimer >= this.dayDuration) {
            this.dayTimer = 0;
            this.currentDay += 1;
        }

        const halfDay = this.dayDuration / 2;
        this.isDay = this.dayTimer < halfDay;
        // Sun angle: 0 to 2*PI
        this.sunAngle = (this.dayTimer / this.dayDuration) * Math.PI * 2;
    }

    consumeFood() {
        if (this.inventory.food > 0) {
            this.inventory.food--;
            this.hunger = Math.min(this.maxHunger, this.hunger + 40);
            this.health = Math.min(this.maxHealth, this.health + 10);
            return true;
        }
        if (this.inventory.fish > 0) {
            this.inventory.fish--;
            this.hunger = Math.min(this.maxHunger, this.hunger + 30);
            return true;
        }
        return false;
    }

    consumeWater() {
        if (this.inventory.water > 0) {
            this.inventory.water--;
            this.thirst = Math.min(this.maxThirst, this.thirst + 45);
            return true;
        }
        return false;
    }

    useMedkit() {
        if (this.inventory.medkit > 0) {
            this.inventory.medkit--;
            this.health = Math.min(this.maxHealth, this.health + 55);
            this.infection = Math.max(0, this.infection - 20);
            return true;
        }
        return false;
    }

    useVaccine() {
        if (this.inventory.vaccines > 0) {
            this.inventory.vaccines--;
            this.infection = 0;
            this.health = Math.min(this.maxHealth, this.health + 20);
            return true;
        }
        return false;
    }

    takeDamage(amount, reason = "Serangan") {
        this.health = Math.max(0, this.health - amount);
        return { isDead: this.health <= 0, reason };
    }

    infect(amount) {
        this.infection = Math.min(100, this.infection + amount);
    }

    getHeliProgress() {
        let count = 0;
        if (this.heliInstalled.heliEngine) count++;
        if (this.heliInstalled.heliRotor) count++;
        if (this.heliInstalled.heliFuel) count++;
        if (this.heliInstalled.heliAvionics) count++;
        return { count, total: 4, complete: count === 4 };
    }
}
