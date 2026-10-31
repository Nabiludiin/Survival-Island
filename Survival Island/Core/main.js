import * as THREE from '../libs/three.module.js';
import { AudioManager } from './audio.js';
import { FirstPersonPlayer } from './player.js';
import { IslandWorld } from './world.js';
import { ScavengeManager } from '../LOGIC/60_Seconds_Logic/scavengeManager.js';
import { SurvivalStatus } from '../LOGIC/Raft_Logic/survivalStatus.js';
import { GatheringCrafting } from '../LOGIC/Raft_Logic/gatheringCrafting.js';
import { BaseBuilding } from '../LOGIC/Raft_Logic/baseBuilding.js';
import { EnemyManager } from '../LOGIC/L4D2_Logic/enemies.js';
import { CombatHorde } from '../LOGIC/L4D2_Logic/combatHorde.js';
import { EventManager } from '../LOGIC/L4D2_Logic/events.js';
import { DiarySystem } from './diary.js';
import { LuckyMachine } from './luckyMachine.js';

class GameApp {
    constructor() {
        this.canvas = document.getElementById('game-canvas');
        this.currentPhase = 'menu'; // 'menu', 'phase1', 'phase2', 'gameover', 'victory'

        // Three.js Core
        this.scene = new THREE.Scene();
        this.scene.background = new THREE.Color(0x0a1018);
        this.scene.fog = new THREE.FogExp2(0x0a1018, 0.015);

        this.camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 800);
        this.scene.add(this.camera);

        this.renderer = new THREE.WebGLRenderer({ canvas: this.canvas, antialias: true });
        this.renderer.setSize(window.innerWidth, window.innerHeight);
        this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
        this.renderer.shadowMap.enabled = true;
        this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;

        // Managers & Systems
        this.audio = new AudioManager();
        this.player = new FirstPersonPlayer(this.camera, this.canvas, this.audio);
        this.status = new SurvivalStatus();
        this.crafting = new GatheringCrafting(this.status, this.audio);
        this.building = new BaseBuilding(this.scene, this.audio);
        this.enemies = new EnemyManager(this.scene, this.audio, this.status);
        this.combat = new CombatHorde(this.scene, this.camera, this.player, this.audio, this.status, this.enemies);
        this.diary = new DiarySystem();
        this.lucky = new LuckyMachine(this.status, this.audio);
        this.events = new EventManager(this.scene, this.audio, this.status, this.lucky);

        this.world = null;
        this.scavenge = null;

        this.clock = new THREE.Clock();
        this.activePlacement = null;

        this.setupUI();
        this.setupEventListeners();
        this.animate = this.animate.bind(this);
        requestAnimationFrame(this.animate);
    }

    setupUI() {
        // Start Game Button
        const btnStart = document.getElementById('btn-start-game');
        btnStart.addEventListener('click', () => {
            document.getElementById('start-screen').style.display = 'none';
            this.audio.init();
            this.startPhase1();
        });

        // Close buttons on modals
        document.getElementById('btn-close-inv').addEventListener('click', () => this.closeModals());
        document.getElementById('btn-close-diary').addEventListener('click', () => this.closeModals());
        document.getElementById('btn-close-lucky').addEventListener('click', () => this.closeModals());

        // Diary chapter tabs
        const tabs = document.querySelectorAll('.diary-tab');
        tabs.forEach(tab => {
            tab.addEventListener('click', (e) => {
                tabs.forEach(t => t.classList.remove('active'));
                tab.classList.add('active');
                const ch = parseInt(tab.dataset.chapter);
                this.diary.setChapter(ch);
                this.renderDiaryContent();
            });
        });

        // Lucky machine spin button
        document.getElementById('btn-spin-lucky').addEventListener('click', () => {
            this.lucky.spin((res) => {
                const resDiv = document.getElementById('lucky-reward-result');
                resDiv.innerText = res.msg;
                this.updateUI();
            });
        });
    }

    startPhase1() {
        this.currentPhase = 'phase1';
        document.getElementById('phase1-timer-box').style.display = 'block';

        this.scavenge = new ScavengeManager(this.scene, this.player, this.audio, (results) => {
            this.onPhase1Complete(results);
        });
        this.scavenge.start();
        this.showToast("FASE 1: 60 DETIK AMBIL PELURU, MAKANAN & SELAMATKAN NPC MENUJU SEKOCI!");
    }

    onPhase1Complete(results) {
        document.getElementById('phase1-timer-box').style.display = 'none';
        this.status.applyScavengeResults(results);

        this.showToast(results.reason || "Kamu berhasil keluar sebelum kapal karam!");

        setTimeout(() => {
            this.startPhase2();
        }, 2000);
    }

    startPhase2() {
        this.currentPhase = 'phase2';

        // Build 3D procedural Island World
        this.world = new IslandWorld(this.scene);

        // Player spawns near the shore of Region 1
        this.player.position.set(-6, 8.5, -4);
        this.player.yaw = 0;

        this.showToast("FASE 2: SELAMAT DATANG DI SURVIVAL ISLAND! KUMPULKAN PARTS HELI DARI REGION 4 & KABUR SEBELUM HARI 60!");
    }

    setupEventListeners() {
        // Window Resize
        window.addEventListener('resize', () => {
            this.camera.aspect = window.innerWidth / window.innerHeight;
            this.camera.updateProjectionMatrix();
            this.renderer.setSize(window.innerWidth, window.innerHeight);
        });

        // Left Click to Shoot / Attack / Place Building
        window.addEventListener('mousedown', (e) => {
            if (e.button !== 0 || !this.player.isLocked) return;

            if (this.building.currentBuildType) {
                // Place current building preview
                const hit = this.player.hoveredObject;
                const point = hit ? hit.point || this.player.position : this.player.position;
                const res = this.building.placeStructure(point, this.player.position);
                this.showToast(res.msg);
                return;
            }

            // Normal Combat Attack
            let interactives = [];
            if (this.world) {
                interactives = this.world.interactiveObjects;
            }
            const res = this.combat.attack(interactives);
            if (res) {
                if (res.action === 'hitResource') {
                    this.handleResourceHit(res.object);
                } else if (res.action === 'hitEnemy' && res.drop) {
                    this.showToast(res.drop);
                }
            }
            this.updateUI();
        });

        // Keyboard actions
        window.addEventListener('keydown', (e) => {
            if (this.currentPhase === 'menu' || this.currentPhase === 'gameover') return;

            // [E] Interact
            if (e.code === 'KeyE') {
                this.handleInteract();
            }
            // [TAB] or [KeyI] Inventory & Crafting
            else if (e.code === 'Tab' || e.code === 'KeyI') {
                e.preventDefault();
                this.toggleInventory();
            }
            // [KeyR] Reload weapon
            else if (e.code === 'KeyR') {
                const res = this.combat.reload();
                if (res && res.msg) this.showToast(res.msg);
                this.updateUI();
            }
            // [KeyQ] Switch weapon
            else if (e.code === 'KeyQ') {
                const w = this.combat.switchWeapon();
                this.showToast(`Senjata Aktif: ${w === 'pistol' ? 'Pistol' : 'Kail (Melee / Hook)'}`);
                this.updateUI();
            }
            // [KeyF] Fishing with Hook
            else if (e.code === 'KeyF') {
                const res = this.crafting.fishHook(this.player.position);
                this.showToast(res.msg);
                this.updateUI();
            }
            // [KeyB] Base Building Cancel
            else if (e.code === 'KeyB') {
                this.building.cancelPlacing();
                this.showToast("Penempatan bangunan dibatalkan.");
            }
            // Number keys for Quick consumable usage
            else if (e.code === 'Digit1') {
                if (this.status.consumeFood()) {
                    this.audio.playPickup();
                    this.showToast("Makan makanan kaleng (+40 Lapar)!");
                }
                this.updateUI();
            } else if (e.code === 'Digit2') {
                if (this.status.consumeWater()) {
                    this.audio.playPickup();
                    this.showToast("Minum air bersih (+45 Haus)!");
                }
                this.updateUI();
            } else if (e.code === 'Digit3') {
                if (this.status.useMedkit()) {
                    this.audio.playPickup();
                    this.showToast("Menggunakan Medkit P3K (+55 HP)!");
                }
                this.updateUI();
            } else if (e.code === 'Digit4') {
                if (this.status.useVaccine()) {
                    this.audio.playPickup();
                    this.showToast("Menyuntikkan Vaksin! Infeksi sembuh total 100%!");
                }
                this.updateUI();
            }
            // [Escape] Close Modals
            else if (e.code === 'Escape') {
                this.closeModals();
            }
        });
    }

    handleInteract() {
        const hover = this.player.hoveredObject;
        if (!hover) return;

        // Phase 1 Scavenge interactions
        if (this.currentPhase === 'phase1' && this.scavenge) {
            const res = this.scavenge.interact(hover);
            if (res && res.text) this.showToast(res.text);
            return;
        }

        // Phase 2 Island interactions
        // 1. Diary Book
        if (hover.isDiary) {
            this.openDiary();
            return;
        }

        // 2. The Lucky Machine
        if (hover.isLuckyMachine) {
            this.openLuckyMachine();
            return;
        }

        // 3. Helicopter Part pickup in Region 4
        if (hover.isHeliPart) {
            const partId = hover.partId;
            this.status.inventory[partId] = true;
            this.scene.remove(hover);
            this.audio.playPickup();
            this.showToast(`Berhasil mengambil: ${hover.partName}! Bawa ke Helipad di Region 3!`);
            this.updateUI();
            return;
        }

        // 4. Helipad Assembly Station in Region 3
        if (hover.isHelipadStation) {
            this.assembleHeliParts();
            return;
        }

        // 5. Beach Supply Crate
        if (hover.isBeachCrate && this.events) {
            const res = this.events.openBeachCrate(hover);
            if (res && res.msg) this.showToast(res.msg);
            this.updateUI();
            return;
        }

        // 6. Resource Plant (Herb / Mushroom)
        if (hover.isResourcePlant) {
            if (hover.plantType === 'herb') {
                this.status.inventory.herbs = (this.status.inventory.herbs || 0) + 1;
                this.showToast("+1 Getah Pohon Herbal (Herbs)");
            } else {
                this.status.inventory.mushrooms = (this.status.inventory.mushrooms || 0) + 1;
                this.showToast("+1 Spora Jamur Tropis (Mushrooms)");
            }
            this.audio.playPickup();
            this.scene.remove(hover);
            this.updateUI();
            return;
        }
    }

    handleResourceHit(obj) {
        if (!obj || !obj.isResource) return;
        obj.hp--;
        if (this.audio) this.audio.playCraft();

        if (obj.resourceType === 'tree') {
            const amount = this.status.hasGoldenAxe ? 5 : 2;
            this.status.inventory.wood += amount;
            this.showToast(`+${amount} Kayu dari Pohon`);
        } else if (obj.resourceType === 'rock') {
            this.status.inventory.stone += 2;
            this.showToast("+2 Batu Tambang");
        }

        if (obj.hp <= 0) {
            this.scene.remove(obj);
        }
        this.updateUI();
    }

    assembleHeliParts() {
        const inv = this.status.inventory;
        const installed = this.status.heliInstalled;
        let installedAny = false;

        if (inv.heliEngine && !installed.heliEngine) {
            installed.heliEngine = true;
            inv.heliEngine = false;
            installedAny = true;
        }
        if (inv.heliRotor && !installed.heliRotor) {
            installed.heliRotor = true;
            inv.heliRotor = false;
            installedAny = true;
        }
        if (inv.heliFuel && !installed.heliFuel) {
            installed.heliFuel = true;
            inv.heliFuel = false;
            installedAny = true;
        }
        if (inv.heliAvionics && !installed.heliAvionics) {
            installed.heliAvionics = true;
            inv.heliAvionics = false;
            installedAny = true;
        }

        const progress = this.status.getHeliProgress();
        if (installedAny) {
            this.audio.playCraft();
            this.showToast(`🔧 Memasang bagian helikopter! (${progress.count} / 4 Bagian Terpasang)`);
        } else {
            this.showToast(`Kamu belum membawa suku cadang baru! Cari di Reruntuhan Desa (Region 4).`);
        }

        if (progress.complete) {
            this.triggerVictory();
        }
        this.updateUI();
    }

    triggerVictory() {
        this.currentPhase = 'victory';
        document.exitPointerLock();
        document.getElementById('victory-modal').classList.add('active');
        this.audio.playPickup();
    }

    triggerGameOver(reason) {
        this.currentPhase = 'gameover';
        document.exitPointerLock();
        document.getElementById('death-reason').innerText = `Penyebab Gugur: ${reason} pada Hari ke-${this.status.currentDay}.`;
        document.getElementById('gameover-modal').classList.add('active');
    }

    // Modal helpers
    toggleInventory() {
        const modal = document.getElementById('inventory-modal');
        if (modal.classList.contains('active')) {
            this.closeModals();
        } else {
            document.exitPointerLock();
            this.renderInventoryModal();
            modal.classList.add('active');
        }
    }

    renderInventoryModal() {
        const invBox = document.getElementById('inv-items-display');
        const inv = this.status.inventory;
        invBox.innerHTML = `
            <span>🪵 Kayu: <strong>${inv.wood}</strong></span>
            <span>🪨 Batu: <strong>${inv.stone}</strong></span>
            <span>🌿 Herbal: <strong>${inv.herbs || 0}</strong></span>
            <span>🍄 Jamur: <strong>${inv.mushrooms || 0}</strong></span>
            <span>🍖 Makanan: <strong>${inv.food}</strong></span>
            <span>💧 Air: <strong>${inv.water}</strong></span>
            <span>🐟 Ikan: <strong>${inv.fish || 0}</strong></span>
            <span>🔫 Peluru: <strong>${inv.ammo}</strong></span>
            <span>💉 Vaksin: <strong>${inv.vaccines || 0}</strong></span>
            <span>🩹 Medkit: <strong>${inv.medkit}</strong></span>
            <span>❄️ Pakaian Hangat: <strong>${inv.warmClothes ? 'Dimiliki' : 'Tidak Ada'}</strong></span>
        `;

        const recipesGrid = document.getElementById('recipes-grid');
        recipesGrid.innerHTML = '';
        this.crafting.recipes.forEach(r => {
            const card = document.createElement('div');
            card.className = 'recipe-card';
            const cost = this.crafting.getEffectiveCost(r.cost);
            let costStr = Object.entries(cost).map(([k, v]) => `${k}: ${v}`).join(', ');
            const can = this.crafting.canCraft(r.id);

            card.innerHTML = `
                <div>
                    <div class="recipe-name">${r.name}</div>
                    <div class="recipe-cost">Biaya: ${costStr}</div>
                    <p style="font-size: 11px; color: #94a3b8; margin-bottom: 12px;">${r.desc}</p>
                </div>
                <button class="btn-primary" ${can ? '' : 'disabled'}>${r.type === 'buildable' ? 'TEMPATKAN' : 'RAKIT'}</button>
            `;

            card.querySelector('button').addEventListener('click', () => {
                const res = this.crafting.craft(r.id, (buildType) => {
                    this.building.startPlacing(buildType);
                    this.closeModals();
                    this.canvas.requestPointerLock();
                });
                this.showToast(res.msg);
                if (res.unlock === 'region2' && this.world) {
                    this.world.unlockBridge();
                }
                this.renderInventoryModal();
                this.updateUI();
            });

            recipesGrid.appendChild(card);
        });
    }

    openDiary() {
        document.exitPointerLock();
        this.diary.open();
        this.renderDiaryContent();
        document.getElementById('diary-modal').classList.add('active');
    }

    renderDiaryContent() {
        const content = this.diary.getCurrentContent();
        const box = document.getElementById('diary-body-content');
        box.innerHTML = `
            <h3 style="color: #00e5ff; margin-bottom: 4px;">${content.title}</h3>
            <div style="font-size: 12px; color: #94a3b8; margin-bottom: 14px;">${content.author}</div>
            ${content.content}
        `;
    }

    openLuckyMachine() {
        document.exitPointerLock();
        const check = this.lucky.canSpin();
        const statusText = document.getElementById('lucky-status-text');
        if (check.isFree) {
            statusText.innerText = "⚡ BADAI ERUPSI AKTIF: PUTARAN GRATIS MENGGUNAKAN LISTRIK STATIS ALAM!";
            statusText.style.color = "#ffeb3b";
        } else {
            statusText.innerText = "Kondisi: Siap Diputar (Biaya: 5 Kayu & 5 Batu)";
            statusText.style.color = "#00e5ff";
        }
        document.getElementById('lucky-modal').classList.add('active');
    }

    closeModals() {
        document.querySelectorAll('.modal-overlay').forEach(m => m.classList.remove('active'));
    }

    showToast(msg) {
        const toast = document.getElementById('toast');
        toast.innerText = msg;
        toast.style.display = 'block';
        clearTimeout(this.toastTimer);
        this.toastTimer = setTimeout(() => {
            toast.style.display = 'none';
        }, 3200);
    }

    updateUI() {
        // Vitals
        document.getElementById('txt-health').innerText = `${Math.round(this.status.health)} / ${this.status.maxHealth}`;
        document.getElementById('bar-health').style.width = `${(this.status.health / this.status.maxHealth) * 100}%`;

        document.getElementById('txt-hunger').innerText = `${Math.round(this.status.hunger)}%`;
        document.getElementById('bar-hunger').style.width = `${this.status.hunger}%`;

        document.getElementById('txt-thirst').innerText = `${Math.round(this.status.thirst)}%`;
        document.getElementById('bar-thirst').style.width = `${this.status.thirst}%`;

        document.getElementById('txt-infection').innerText = `${Math.round(this.status.infection)}%`;
        document.getElementById('bar-infection').style.width = `${this.status.infection}%`;

        document.getElementById('txt-stamina').innerText = `${Math.round(this.status.stamina)}%`;
        document.getElementById('bar-stamina').style.width = `${(this.status.stamina / this.status.maxStamina) * 100}%`;

        // Screen Vignettes
        document.getElementById('damage-vignette').style.opacity = (1 - (this.status.health / this.status.maxHealth)) * 0.85;
        document.getElementById('frost-vignette').style.opacity = (this.status.frostbite / 100) * 0.9;
        document.getElementById('toxic-vignette').style.opacity = (this.status.infection / 100) * 0.8;

        // Weapon
        document.getElementById('hud-weapon').innerText = this.combat.currentWeapon === 'pistol' ? 'PISTOL BERISIKO' : 'KAIL PANCING (MELEE)';
        document.getElementById('hud-mag').innerText = this.combat.currentWeapon === 'pistol' ? this.combat.magAmmo : '∞';
        document.getElementById('hud-ammo').innerText = this.status.inventory.ammo;

        // Heli Progress
        const heliProg = this.status.getHeliProgress();
        document.getElementById('hud-heli').innerText = `🚁 RAKIT HELI: ${heliProg.count} / 4 BAGIAN`;

        // Day and Region
        document.getElementById('hud-day').innerText = `HARI ${this.status.currentDay} / ${this.status.targetDays}`;
        const phaseLabel = this.status.isDay ? "☀️ SIANG HARI (HEWAN BUAS)" : "🌙 MALAM HARI (HORDE ZOMBIE)";
        document.getElementById('hud-phase').innerText = phaseLabel;

        if (this.world) {
            const currentReg = this.world.getCurrentRegion(this.player.position.x, this.player.position.z);
            document.getElementById('hud-region').innerText = this.world.getRegionName(currentReg);
        }
    }

    animate() {
        requestAnimationFrame(this.animate);
        const dt = Math.min(this.clock.getDelta(), 0.1);

        if (this.currentPhase === 'phase1' && this.scavenge) {
            this.scavenge.update(dt);
            this.player.update(dt, null, true);

            // Phase 1 timer text
            const m = Math.floor(this.scavenge.timeLeft / 60);
            const s = Math.floor(this.scavenge.timeLeft % 60);
            document.getElementById('phase1-timer').innerText = `${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;

            // Check interaction in ship
            const prompt = document.getElementById('interact-prompt');
            const hit = this.player.checkInteraction([...this.scavenge.items, ...this.scavenge.npcs, this.scavenge.exitDoor]);
            if (hit) {
                prompt.innerText = hit.text;
                prompt.style.display = 'block';
            } else {
                prompt.style.display = 'none';
            }
        } else if (this.currentPhase === 'phase2' && this.world) {
            const currentReg = this.world.getCurrentRegion(this.player.position.x, this.player.position.z);
            const isNearFire = this.building.isNearCampfire(this.player.position);

            this.status.update(dt, currentReg, isNearFire);
            if (this.status.health <= 0) {
                this.triggerGameOver("Tubuhmu tak sanggup lagi bertahan");
            }

            // Height query func
            const heightFunc = (x, z) => this.world.getApproxHeight(x, z);
            this.player.update(dt, heightFunc, false);

            // Combat update
            this.combat.update(dt);

            // Enemies update
            this.enemies.update(dt, this.player.position, this.status.isDay, currentReg);

            // Natural Events & Supply Drops
            this.events.update(dt);

            // Lighting cycle update (Sun moves across sky)
            if (this.world.sunLight) {
                const angle = this.status.sunAngle;
                this.world.sunLight.position.set(Math.cos(angle) * 80, Math.sin(angle) * 80, 40);
                this.world.sunLight.intensity = this.status.isDay ? 1.5 : 0.08;
                this.world.ambientLight.intensity = this.status.isDay ? 0.45 : 0.08;

                // Fog color transition
                const fogColor = this.status.isDay ? 0x87ceeb : 0x05070a;
                this.scene.fog.color.setHex(fogColor);
                this.scene.background.setHex(fogColor);
            }

            // Interaction Raycast
            const prompt = document.getElementById('interact-prompt');
            const interactives = [
                ...this.world.interactiveObjects,
                ...this.world.heliPartsOnGround,
                this.world.helipadStation,
                this.world.luckyMachineObject,
                ...this.events.beachCrates
            ];

            const hit = this.player.checkInteraction(interactives);
            if (hit) {
                prompt.innerText = hit.text;
                prompt.style.display = 'block';
            } else {
                prompt.style.display = 'none';
            }

            // Building preview placement
            if (this.building.currentBuildType) {
                this.building.updatePreview(this.player.position, null);
            }

            this.updateUI();
        }

        this.renderer.render(this.scene, this.camera);
    }
}

// Instantiate game on load
window.addEventListener('DOMContentLoaded', () => {
    new GameApp();
});
