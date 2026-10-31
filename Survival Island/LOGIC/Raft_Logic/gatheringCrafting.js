export class GatheringCrafting {
    constructor(status, audio) {
        this.status = status;
        this.audio = audio;

        // Recipes definition
        this.recipes = [
            {
                id: 'campfire',
                name: 'Api Unggun (Campfire)',
                desc: 'Menghangatkan tubuh di malam dingin & mencegah frostbite.',
                cost: { wood: 5, stone: 3 },
                type: 'buildable'
            },
            {
                id: 'wall',
                name: 'Dinding Kayu (Shelter Wall)',
                desc: 'Barikade pelindung markas dari serbuan horde zombie malam.',
                cost: { wood: 8, stone: 0 },
                type: 'buildable'
            },
            {
                id: 'floor',
                name: 'Lantai Kayu (Shelter Floor)',
                desc: 'Fondasi markas di daratan aman.',
                cost: { wood: 6, stone: 0 },
                type: 'buildable'
            },
            {
                id: 'torch',
                name: 'Obor Genggam (Hand Torch)',
                desc: 'Penerangan malam hari & menangkal rasa dingin.',
                cost: { wood: 4, herbs: 2 },
                type: 'item',
                produce: { torches: 1 }
            },
            {
                id: 'warmClothes',
                name: 'Pakaian Hangat (Winter Clothes)',
                desc: 'Wajib untuk mendaki Puncak Salju (Region 3) tanpa terkena frostbite.',
                cost: { wood: 10, herbs: 6 },
                type: 'item',
                produce: { warmClothes: true }
            },
            {
                id: 'bridge',
                name: 'Jembatan Gantung Hutan Tropis (Bridge)',
                desc: 'Membuka akses melintasi jurang menuju Region 2 (Hutan Hujan Amazon).',
                cost: { wood: 35, stone: 10 },
                type: 'worldUnlock',
                target: 'region2'
            },
            {
                id: 'vaccine',
                name: 'Vaksin Sintetis Eksperimental',
                desc: 'Menyembuhkan total 100% wabah infeksi gigitan zombie.',
                cost: { herbs: 3, mushrooms: 3, water: 1 },
                type: 'item',
                produce: { vaccines: 1 }
            },
            {
                id: 'ammo',
                name: 'Rakit Peluru Pistol (+8)',
                desc: 'Membuat amunisi darurat dari serpihan batu dan bubuk mesiu.',
                cost: { stone: 3, wood: 2 },
                type: 'item',
                produce: { ammo: 8 }
            }
        ];
    }

    getEffectiveCost(cost) {
        // Marcus discount (25% less wood & stone)
        const factor = this.status.hasMarcus ? 0.75 : 1.0;
        const effective = {};
        for (const [k, v] of Object.entries(cost)) {
            effective[k] = (k === 'wood' || k === 'stone') ? Math.max(1, Math.round(v * factor)) : v;
        }
        return effective;
    }

    canCraft(recipeId) {
        const recipe = this.recipes.find(r => r.id === recipeId);
        if (!recipe) return false;
        const cost = this.getEffectiveCost(recipe.cost);
        for (const [res, amount] of Object.entries(cost)) {
            if ((this.status.inventory[res] || 0) < amount) {
                return false;
            }
        }
        return true;
    }

    craft(recipeId, onBuildSelected) {
        const recipe = this.recipes.find(r => r.id === recipeId);
        if (!recipe) return { success: false, msg: "Resep tidak ditemukan" };
        if (!this.canCraft(recipeId)) return { success: false, msg: "Bahan resource tidak mencukupi!" };

        const cost = this.getEffectiveCost(recipe.cost);
        // Deduct resource
        for (const [res, amount] of Object.entries(cost)) {
            this.status.inventory[res] -= amount;
        }

        if (this.audio) this.audio.playCraft();

        if (recipe.type === 'item') {
            for (const [k, v] of Object.entries(recipe.produce)) {
                if (typeof v === 'boolean') {
                    this.status.inventory[k] = v;
                } else {
                    this.status.inventory[k] = (this.status.inventory[k] || 0) + v;
                }
            }
            return { success: true, msg: `Berhasil merakit: ${recipe.name}!` };
        } else if (recipe.type === 'buildable') {
            if (onBuildSelected) {
                onBuildSelected(recipe.id);
            }
            return { success: true, msg: `Mode penempatan: ${recipe.name}. Klik kiri untuk menempatkan!`, buildable: recipe.id };
        } else if (recipe.type === 'worldUnlock') {
            return { success: true, msg: `Jembatan Gantung berhasil dibangun! Akses Region 2 dibuka!`, unlock: recipe.target };
        }

        return { success: true, msg: `Selesai: ${recipe.name}` };
    }

    // Hook Fishing near water
    fishHook(playerPos) {
        // Player must be near sea level or shoreline
        if (playerPos.y > 6.0 || (playerPos.x * playerPos.x + playerPos.z * playerPos.z < 600)) {
            return { success: false, msg: "Terlalu jauh dari pantai/laut untuk memancing!" };
        }

        const successChance = Math.random();
        if (this.audio) this.audio.playMelee();

        if (successChance > 0.35) {
            this.status.inventory.fish = (this.status.inventory.fish || 0) + 1;
            if (this.audio) this.audio.playPickup();
            return { success: true, msg: "Berhasil menangkap Ikan Segar (+30 Lapar)!" };
        } else if (successChance > 0.15) {
            this.status.inventory.wood += 2;
            return { success: true, msg: "Kail menjaring kayu apung (+2 Kayu)!" };
        } else {
            return { success: false, msg: "Kail tersangkut karang, belum ada ikan." };
        }
    }
}
