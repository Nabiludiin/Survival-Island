import * as THREE from '../../libs/three.module.js';

export class EventManager {
    constructor(scene, audio, status, luckyMachine, building = null) {
        this.scene = scene;
        this.audio = audio;
        this.status = status;
        this.luckyMachine = luckyMachine;
        this.building = building; // Meteor bisa merusak bangunan markas

        this.eventActive = false;
        this.eventType = null;
        this.eventTimer = 0;
        this.lastCheckedDay = 0;

        this.beachCrates = [];
        this.meteors = [];

        this.crateMat = new THREE.MeshStandardMaterial({ color: 0xb58900, roughness: 0.6, metalness: 0.2 });
        this.meteorMat = new THREE.MeshBasicMaterial({ color: 0xff3300 });
        // Shared geometry so crates and meteors don't allocate new buffers each spawn
        this.crateGeo = new THREE.BoxGeometry(1.2, 0.9, 1.2);
        this.meteorGeo = new THREE.DodecahedronGeometry(0.8);

        this.meteorRadius = 4.0;       // Radius ledakan batu pijar
        this.meteorDamage = 20;        // Damage ke pemain
        this.meteorStructureDamage = 60;
    }

    // Returns notification messages for main.js to show as toasts
    update(dt, playerPos = null, heightFunc = null) {
        const messages = [];

        // Check weekly supply drop on Day 7, 14, 21, etc.
        const day = this.status.currentDay;
        if (day !== this.lastCheckedDay) {
            this.lastCheckedDay = day;
            if (day % 7 === 0) {
                messages.push(this.spawnWeeklyBeachSupply());
            }
            // 35% chance of sudden event every 2 days
            if (day > 1 && day % 2 === 0 && Math.random() < 0.35 && !this.eventActive) {
                messages.push(this.triggerVolcanoEruption());
            }
        }

        // Update active event
        if (this.eventActive) {
            this.eventTimer -= dt;
            if (this.eventType === 'volcano') {
                messages.push(...this.updateMeteors(dt, playerPos, heightFunc));
            }
            if (this.eventTimer <= 0) {
                this.endEvent();
                messages.push("🌋 Letusan gunung berapi mereda.");
            }
        }

        return messages;
    }

    spawnWeeklyBeachSupply() {
        // Clear old crates if any
        for (const c of this.beachCrates) {
            this.scene.remove(c);
        }
        this.beachCrates = [];

        // Spawn 3 crates on beach perimeter
        for (let i = 0; i < 3; i++) {
            const angle = (i * 0.8) + Math.PI / 4;
            const radius = 98; // Beach waterline
            const x = Math.cos(angle) * radius;
            const z = Math.sin(angle) * radius;

            const crate = new THREE.Mesh(this.crateGeo, this.crateMat);
            crate.position.set(x, 1.0, z);
            crate.isInteractable = true;
            crate.isBeachCrate = true;
            crate.interactText = "TEKAN [E]: BUKA KOTAK KARGO KAPAL (SUPLAI MINGGUAN)";
            this.scene.add(crate);
            this.beachCrates.push(crate);
        }

        if (this.audio) this.audio.playPickup();
        return "📦 SUPLAI MINGGUAN: Arus laut membawa 3 Kotak Kargo Kapal Pesiar ke pinggir pantai!";
    }

    openBeachCrate(crate) {
        const idx = this.beachCrates.indexOf(crate);
        if (idx !== -1) {
            this.scene.remove(crate);
            this.beachCrates.splice(idx, 1);

            this.status.inventory.ammo += 18;
            this.status.inventory.food += 2;
            this.status.inventory.water += 2;
            this.status.inventory.medkit += 1;

            if (this.audio) this.audio.playPickup();
            return {
                success: true,
                msg: "Membuka Kargo: +18 Amunisi, +2 Makanan, +2 Air Bersih, +1 Medkit P3K!"
            };
        }
        return { success: false };
    }

    triggerVolcanoEruption() {
        this.eventActive = true;
        this.eventType = 'volcano';
        this.eventTimer = 35; // 35 seconds of chaos!

        if (this.audio) {
            this.audio.playAlarm();
        }

        // Notify Lucky Machine: Free power during lightning eruption!
        if (this.luckyMachine) {
            this.luckyMachine.setStormCharged(true);
        }

        return "🌋 BENCANA ALAM: GUNUNG BERAPI REGION 1 MELETUS! Batu pijar berjatuhan! Mesin Keberuntungan aktif gratis!";
    }

    updateMeteors(dt, playerPos, heightFunc) {
        const messages = [];

        // Spawn falling burning rocks
        if (Math.random() < 0.25 && this.meteors.length < 8) {
            const meteor = new THREE.Mesh(this.meteorGeo, this.meteorMat);
            meteor.position.set(
                (Math.random() - 0.5) * 80 - 40,
                35,
                (Math.random() - 0.5) * 80 - 40
            );
            meteor.velocity = new THREE.Vector3((Math.random() - 0.5) * 5, -25, (Math.random() - 0.5) * 5);
            this.scene.add(meteor);
            this.meteors.push(meteor);
        }

        // Move meteors down
        for (let i = this.meteors.length - 1; i >= 0; i--) {
            const m = this.meteors[i];
            m.position.addScaledVector(m.velocity, dt);
            const groundY = heightFunc ? heightFunc(m.position.x, m.position.z) : 0;
            if (m.position.y <= groundY) {
                if (this.audio) this.audio.playHit();
                messages.push(...this.applyMeteorImpact(m.position, playerPos));
                this.scene.remove(m);
                this.meteors.splice(i, 1);
            }
        }

        return messages;
    }

    applyMeteorImpact(impactPos, playerPos) {
        const messages = [];
        const r = this.meteorRadius;

        if (playerPos) {
            const dx = playerPos.x - impactPos.x;
            const dz = playerPos.z - impactPos.z;
            if (dx * dx + dz * dz <= r * r) {
                this.status.takeDamage(this.meteorDamage, "Tertimpa batu pijar gunung berapi");
                messages.push(`☄️ Tertimpa batu pijar! -${this.meteorDamage} HP`);
            }
        }

        if (this.building) {
            // Copy: damageStructure removes destroyed items from the array
            for (const s of [...this.building.structures]) {
                const dx = s.position.x - impactPos.x;
                const dz = s.position.z - impactPos.z;
                if (dx * dx + dz * dz <= r * r) {
                    const res = this.building.damageStructure(s, this.meteorStructureDamage);
                    if (res.destroyed) messages.push("🔥 Batu pijar menghancurkan salah satu bangunanmu!");
                }
            }
        }

        return messages;
    }

    endEvent() {
        this.eventActive = false;
        if (this.luckyMachine) {
            this.luckyMachine.setStormCharged(false);
        }
        for (const m of this.meteors) {
            this.scene.remove(m);
        }
        this.meteors = [];
        this.eventType = null;
    }
}
