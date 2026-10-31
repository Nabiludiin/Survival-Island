import * as THREE from '../../libs/three.module.js';

export class ScavengeManager {
    constructor(scene, player, audio, onPhaseComplete) {
        this.scene = scene;
        this.player = player;
        this.audio = audio;
        this.onPhaseComplete = onPhaseComplete;

        this.isActive = false;
        this.timeLeft = 60.0;
        this.items = [];
        this.npcs = [];
        this.rescuedNpcs = [];
        this.collectedItems = {
            ammo: 18,        // Starts with pistol ammo
            food: 2,         // 2 canned foods
            water: 2,        // 2 fresh waters
            medkit: 1,       // 1 basic medkit
            wood: 0,
            stone: 0,
            hasHook: true,   // Kail (Raft base item)
            hasPistol: true, // Pistol (L4D2 base item)
            vaccines: 1,     // 1 initial vaccine
            rescuedElena: false,  // Medic NPC
            rescuedMarcus: false  // Craftsman NPC
        };

        this.shipGroup = new THREE.Group();
        this.waterMesh = null;
        this.waterHeight = -1.2;
        this.tiltAngle = 0;
    }

    start() {
        this.isActive = true;
        this.timeLeft = 60.0;
        this.buildShipInterior();
        this.player.position.set(0, 1.0, 12);
        this.player.yaw = Math.PI; // Face forward into ship corridor
        if (this.audio) {
            this.audio.playAlarm();
        }
    }

    buildShipInterior() {
        this.scene.add(this.shipGroup);

        // Corridor walls, floor, ceiling
        const wallMat = new THREE.MeshStandardMaterial({ color: 0x242a38, roughness: 0.8, metalness: 0.3 });
        const floorMat = new THREE.MeshStandardMaterial({ color: 0x181a20, roughness: 0.9, metalness: 0.1 });
        const ceilingMat = new THREE.MeshStandardMaterial({ color: 0x1f232d, roughness: 0.8 });
        const alertLightMat = new THREE.MeshBasicMaterial({ color: 0xff2222 });

        // Floor
        const floorGeo = new THREE.PlaneGeometry(10, 40);
        const floor = new THREE.Mesh(floorGeo, floorMat);
        floor.rotation.x = -Math.PI / 2;
        floor.position.set(0, 0, 0);
        this.shipGroup.add(floor);

        // Ceiling
        const ceiling = new THREE.Mesh(floorGeo, ceilingMat);
        ceiling.rotation.x = Math.PI / 2;
        ceiling.position.set(0, 4.0, 0);
        this.shipGroup.add(ceiling);

        // Left & Right walls
        const wallGeo = new THREE.PlaneGeometry(40, 4.0);
        const leftWall = new THREE.Mesh(wallGeo, wallMat);
        leftWall.position.set(-5, 2.0, 0);
        leftWall.rotation.y = Math.PI / 2;
        this.shipGroup.add(leftWall);

        const rightWall = new THREE.Mesh(wallGeo, wallMat);
        rightWall.position.set(5, 2.0, 0);
        rightWall.rotation.y = -Math.PI / 2;
        this.shipGroup.add(rightWall);

        // End wall (front exit / lifeboat door)
        const doorWallGeo = new THREE.PlaneGeometry(10, 4.0);
        const frontWall = new THREE.Mesh(doorWallGeo, wallMat);
        frontWall.position.set(0, 2.0, -20);
        this.shipGroup.add(frontWall);

        // Back wall
        const backWall = new THREE.Mesh(doorWallGeo, wallMat);
        backWall.position.set(0, 2.0, 20);
        backWall.rotation.y = Math.PI;
        this.shipGroup.add(backWall);

        // Lifeboat exit hatch at front
        const doorGeo = new THREE.BoxGeometry(2.4, 3.2, 0.3);
        const doorMat = new THREE.MeshStandardMaterial({ color: 0x00cc66, emissive: 0x003311, metalness: 0.7 });
        this.exitDoor = new THREE.Mesh(doorGeo, doorMat);
        this.exitDoor.position.set(0, 1.6, -19.8);
        this.exitDoor.isInteractable = true;
        this.exitDoor.interactText = "TEKAN [E]: MASUK KE SEKOCI DARURAT (AKHIRI FASE 1)";
        this.shipGroup.add(this.exitDoor);

        // Red flashing emergency lights along corridor
        this.emergencyLights = [];
        for (let z = -15; z <= 15; z += 10) {
            const bulb = new THREE.Mesh(new THREE.SphereGeometry(0.15, 8, 8), alertLightMat);
            bulb.position.set(0, 3.8, z);
            this.shipGroup.add(bulb);

            const pLight = new THREE.PointLight(0xff2222, 1.8, 14);
            pLight.position.set(0, 3.7, z);
            this.shipGroup.add(pLight);
            this.emergencyLights.push(pLight);
        }

        // Rising water mesh
        const waterGeo = new THREE.PlaneGeometry(20, 60);
        const waterMat = new THREE.MeshStandardMaterial({
            color: 0x004455,
            transparent: true,
            opacity: 0.65,
            roughness: 0.1,
            metalness: 0.8
        });
        this.waterMesh = new THREE.Mesh(waterGeo, waterMat);
        this.waterMesh.rotation.x = -Math.PI / 2;
        this.waterMesh.position.set(0, this.waterHeight, 0);
        this.shipGroup.add(this.waterMesh);

        // Spawn Scavenge Items
        this.spawnItem('ammo', 'Kotak Peluru Pistol (+12 Peluru)', -3.2, 0.4, 8, 0xd4af37);
        this.spawnItem('ammo', 'Kotak Peluru Pistol (+12 Peluru)', 3.4, 0.4, -4, 0xd4af37);
        this.spawnItem('food', 'Makanan Kaleng Nutrisi (+35 Lapar)', -2.8, 0.4, 0, 0x44aa44);
        this.spawnItem('food', 'Makanan Kaleng Nutrisi (+35 Lapar)', 3.0, 0.4, 14, 0x44aa44);
        this.spawnItem('water', 'Air Mineral Botol (+40 Haus)', 2.5, 0.4, -10, 0x3388ff);
        this.spawnItem('medkit', 'Kotak Medis P3K (+50 HP / Kurangi Infeksi)', -3.5, 0.4, -14, 0xffffff);

        // Spawn NPCs
        this.spawnNPC('elena', 'Dr. Elena (Ahli Medis - Memperlambat Laju Infeksi)', -2.5, 0, -8, 0x00e5ff);
        this.spawnNPC('marcus', 'Marcus (Tukang Kayu/Crafting - Diskon Biaya Rakit)', 2.5, 0, 4, 0xffaa00);
    }

    spawnItem(type, name, x, y, z, color) {
        const geo = new THREE.BoxGeometry(0.5, 0.4, 0.5);
        const mat = new THREE.MeshStandardMaterial({ color: color, roughness: 0.4, metalness: 0.6 });
        const mesh = new THREE.Mesh(geo, mat);
        mesh.position.set(x, y, z);
        mesh.castShadow = true;
        mesh.isInteractable = true;
        mesh.interactText = `TEKAN [E]: AMBIL ${name.toUpperCase()}`;
        mesh.itemData = { type, name };
        this.shipGroup.add(mesh);
        this.items.push(mesh);
    }

    spawnNPC(id, name, x, y, z, color) {
        const group = new THREE.Group();
        // Body
        const bodyGeo = new THREE.CylinderGeometry(0.35, 0.4, 1.4, 10);
        const bodyMat = new THREE.MeshStandardMaterial({ color: color, roughness: 0.6 });
        const body = new THREE.Mesh(bodyGeo, bodyMat);
        body.position.y = 0.7;
        group.add(body);

        // Head
        const headGeo = new THREE.SphereGeometry(0.28, 10, 10);
        const headMat = new THREE.MeshStandardMaterial({ color: 0xffd1b3, roughness: 0.8 });
        const head = new THREE.Mesh(headGeo, headMat);
        head.position.y = 1.6;
        group.add(head);

        group.position.set(x, y, z);
        group.isInteractable = true;
        group.interactText = `TEKAN [E]: SELAMATKAN ${name.toUpperCase()}`;
        group.npcData = { id, name };

        this.shipGroup.add(group);
        this.npcs.push(group);
    }

    interact(object) {
        if (!object || !this.isActive) return false;

        // Check if Exit Door
        if (object === this.exitDoor) {
            this.finishPhase("Pemain berhasil keluar dengan sekoci darurat tepat waktu!");
            return true;
        }

        // Check if Item
        const itemIdx = this.items.indexOf(object);
        if (itemIdx !== -1) {
            const item = object.itemData;
            if (item.type === 'ammo') this.collectedItems.ammo += 12;
            if (item.type === 'food') this.collectedItems.food += 1;
            if (item.type === 'water') this.collectedItems.water += 1;
            if (item.type === 'medkit') this.collectedItems.medkit += 1;

            if (this.audio) this.audio.playPickup();
            this.shipGroup.remove(object);
            this.items.splice(itemIdx, 1);
            return { success: true, text: `Diambil: ${item.name}` };
        }

        // Check if NPC
        const npcIdx = this.npcs.indexOf(object);
        if (npcIdx !== -1) {
            const npc = object.npcData;
            if (npc.id === 'elena') this.collectedItems.rescuedElena = true;
            if (npc.id === 'marcus') this.collectedItems.rescuedMarcus = true;
            this.rescuedNpcs.push(npc);

            if (this.audio) this.audio.playPickup();
            this.shipGroup.remove(object);
            this.npcs.splice(npcIdx, 1);
            return { success: true, text: `Berhasil Menyelamatkan: ${npc.name}!` };
        }

        return false;
    }

    update(dt) {
        if (!this.isActive) return;

        this.timeLeft -= dt;
        if (this.timeLeft <= 0) {
            this.timeLeft = 0;
            this.finishPhase("WAKTU 60 DETIK HABIS! Kapal tenggelam sepenuhnya ke dasar laut, kamu terhanyut ke pulau!");
            return;
        }

        // Rising water
        this.waterHeight += dt * 0.045; // Rises ~2.7m over 60s
        if (this.waterMesh) {
            this.waterMesh.position.y = this.waterHeight;
        }

        // Sinking tilt animation
        this.tiltAngle = Math.sin(Date.now() * 0.001) * 0.05 + ((60 - this.timeLeft) / 60) * 0.12;
        this.shipGroup.rotation.z = Math.sin(Date.now() * 0.0015) * 0.03;
        this.shipGroup.rotation.x = this.tiltAngle;

        // Emergency flashing lights
        const flash = Math.sin(Date.now() * 0.01) > 0 ? 2.5 : 0.4;
        for (const light of this.emergencyLights) {
            light.intensity = flash;
        }
    }

    finishPhase(reason) {
        this.isActive = false;
        // Clean up 3D ship
        this.scene.remove(this.shipGroup);
        if (this.onPhaseComplete) {
            this.onPhaseComplete({
                reason,
                inventory: this.collectedItems,
                rescuedNpcs: this.rescuedNpcs
            });
        }
    }
}
