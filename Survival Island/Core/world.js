import * as THREE from '../libs/three.module.js';

export class IslandWorld {
    constructor(scene) {
        this.scene = scene;
        this.interactiveObjects = [];
        this.heliPartsOnGround = [];
        this.helipadStation = null;
        this.luckyMachineObject = null;
        this.bridgeMesh = null;
        this.bridgeBuilt = false;

        this.initWorld();
    }

    initWorld() {
        // 1. Lighting setup
        this.ambientLight = new THREE.AmbientLight(0xffffff, 0.4);
        this.scene.add(this.ambientLight);

        this.sunLight = new THREE.DirectionalLight(0xfff5e6, 1.4);
        this.sunLight.position.set(50, 100, 50);
        this.sunLight.castShadow = true;
        this.sunLight.shadow.mapSize.width = 1024;
        this.sunLight.shadow.mapSize.height = 1024;
        this.scene.add(this.sunLight);

        // 2. Ocean water
        const oceanGeo = new THREE.PlaneGeometry(600, 600, 32, 32);
        const oceanMat = new THREE.MeshStandardMaterial({
            color: 0x004466,
            roughness: 0.15,
            metalness: 0.85,
            transparent: true,
            opacity: 0.88
        });
        this.oceanMesh = new THREE.Mesh(oceanGeo, oceanMat);
        this.oceanMesh.rotation.x = -Math.PI / 2;
        this.oceanMesh.position.y = 0.5;
        this.scene.add(this.oceanMesh);

        // 3. Island Terrain (Procedural heightmap plane)
        this.buildTerrain();

        // 4. Region Landmarks & Foliage
        this.buildRegion1_VolcanoForest();
        this.buildRegion2_TropicalJungle();
        this.buildRegion3_SnowMountainHelipad();
        this.buildRegion4_VillageRuins();

        // 5. Old Abandoned Shelter with Diary (Tutorial Guidebook)
        this.buildDiaryShelter();
    }

    buildTerrain() {
        const size = 260;
        const segments = 90;
        const geo = new THREE.PlaneGeometry(size, size, segments, segments);
        geo.rotateX(-Math.PI / 2);

        const pos = geo.attributes.position;
        // Vertex colors to represent grass, sand, rock, snow
        const colors = [];

        for (let i = 0; i < pos.count; i++) {
            const x = pos.getX(i);
            const z = pos.getZ(i);
            const dist = Math.sqrt(x * x + z * z);

            let height = 0;
            // Island boundary dropoff
            if (dist < 110) {
                const normalizedDist = dist / 110;
                const islandShape = Math.cos(normalizedDist * Math.PI * 0.5);

                // Base elevation
                height = islandShape * 8.0;

                // Region 1: Volcano (X: -40 to -90, Z: -40 to -90)
                if (x < -20 && z < -20) {
                    const vDist = Math.hypot(x + 55, z + 55);
                    if (vDist < 40) {
                        height += Math.max(0, (40 - vDist) * 0.65);
                    }
                }

                // Region 3: Snow Mountain Peak (X: 30 to 80, Z: -30 to -80)
                if (x > 15 && z < -15) {
                    const sDist = Math.hypot(x - 50, z + 50);
                    if (sDist < 45) {
                        height += Math.max(0, (45 - sDist) * 0.85);
                    }
                }

                // Ravine between Region 1 and 2 (Along X: -10 to 10, Z: 0 to 80)
                if (Math.abs(x) < 8 && z > 5) {
                    height = Math.max(0.6, height - 6.0); // Deep chasm!
                }
            } else {
                height = 0.2; // Underwater bed
            }

            pos.setY(i, height);

            // Biome vertex coloring
            if (height < 2.5) {
                // Sandy beach
                colors.push(0.85, 0.78, 0.55);
            } else if (x > 15 && z < -15 && height > 16.0) {
                // Region 3 snow peak
                colors.push(0.92, 0.95, 0.98);
            } else if (x < -20 && z < -20 && height > 12.0) {
                // Volcano charred rock
                colors.push(0.35, 0.25, 0.25);
            } else if (x < -10 && z > 15) {
                // Region 2 tropical lush jungle
                colors.push(0.12, 0.45, 0.15);
            } else if (x > 15 && z > 15) {
                // Region 4 murky muddy village
                colors.push(0.25, 0.28, 0.18);
            } else {
                // Region 1 Pine forest standard grass
                colors.push(0.22, 0.52, 0.24);
            }
        }

        geo.setAttribute('color', new THREE.Float32BufferAttribute(colors, 3));
        geo.computeVertexNormals();

        const terrainMat = new THREE.MeshStandardMaterial({
            vertexColors: true,
            roughness: 0.85,
            metalness: 0.1
        });

        this.terrain = new THREE.Mesh(geo, terrainMat);
        this.terrain.receiveShadow = true;
        this.scene.add(this.terrain);
    }

    buildRegion1_VolcanoForest() {
        // Active Volcano top with lava caldera
        const volcanoLava = new THREE.Mesh(
            new THREE.CylinderGeometry(8, 8, 1, 16),
            new THREE.MeshBasicMaterial({ color: 0xff3300 })
        );
        volcanoLava.position.set(-55, 23.5, -55);
        this.scene.add(volcanoLava);

        const lavaLight = new THREE.PointLight(0xff4400, 3.5, 30);
        lavaLight.position.set(-55, 25, -55);
        this.scene.add(lavaLight);

        // Scatter Pine Trees in Region 1
        for (let i = 0; i < 24; i++) {
            const x = -15 - Math.random() * 35;
            const z = 5 - Math.random() * 35;
            this.spawnTree(x, z, 'pine');
        }

        // Scatter Boulders/Rocks
        for (let i = 0; i < 15; i++) {
            const x = -10 - Math.random() * 40;
            const z = -5 - Math.random() * 30;
            this.spawnRock(x, z);
        }

        // Build "The Lucky Machine" near Region 1 center
        this.buildLuckyMachine(-25, 8.5, -15);
    }

    buildRegion2_TropicalJungle() {
        // Dense Amazon Style Jungle (X: -15 to -70, Z: 20 to 80)
        for (let i = 0; i < 35; i++) {
            const x = -20 - Math.random() * 45;
            const z = 25 + Math.random() * 45;
            this.spawnTree(x, z, 'tropical');
        }

        // Medicinal Herbs & Mushrooms (Needed for Vaccine crafting)
        for (let i = 0; i < 16; i++) {
            const x = -25 - Math.random() * 40;
            const z = 30 + Math.random() * 40;
            this.spawnResourcePlant(x, z, i % 2 === 0 ? 'herb' : 'mushroom');
        }

        // Gorge Ravine placeholder & bridge anchor
        this.buildGorgeBridgeAnchor();
    }

    buildGorgeBridgeAnchor() {
        // Bridge across ravine (X: -8 to 8, Z: 40)
        const postMat = new THREE.MeshStandardMaterial({ color: 0x5a3d28, roughness: 0.9 });
        const postGeo = new THREE.CylinderGeometry(0.3, 0.3, 4, 8);

        const p1 = new THREE.Mesh(postGeo, postMat);
        p1.position.set(-6, 7, 40);
        this.scene.add(p1);

        const p2 = new THREE.Mesh(postGeo, postMat);
        p2.position.set(6, 7, 40);
        this.scene.add(p2);

        // Invisible / unbuilt bridge mesh
        const bridgeGeo = new THREE.BoxGeometry(14, 0.4, 3.5);
        const bridgeMat = new THREE.MeshStandardMaterial({ color: 0x6e492e, roughness: 0.8 });
        this.bridgeMesh = new THREE.Mesh(bridgeGeo, bridgeMat);
        this.bridgeMesh.position.set(0, 6.8, 40);
        this.bridgeMesh.visible = false;
        this.scene.add(this.bridgeMesh);
    }

    unlockBridge() {
        this.bridgeBuilt = true;
        if (this.bridgeMesh) {
            this.bridgeMesh.visible = true;
        }
    }

    buildRegion3_SnowMountainHelipad() {
        // Snowy Peak (X: 30 to 70, Z: -30 to -70)
        const helipadCenter = new THREE.Vector3(50, 22.5, -50);

        // Concrete Helipad Platform
        const helipadMat = new THREE.MeshStandardMaterial({ color: 0x3d4349, roughness: 0.7 });
        const padGeo = new THREE.CylinderGeometry(10, 10, 0.6, 24);
        const pad = new THREE.Mesh(padGeo, helipadMat);
        pad.position.copy(helipadCenter);
        this.scene.add(pad);

        // Helipad Yellow "H" Marking
        const hMat = new THREE.MeshBasicMaterial({ color: 0xffcc00 });
        const hBar1 = new THREE.Mesh(new THREE.BoxGeometry(1.2, 0.1, 7), hMat);
        hBar1.position.set(50 - 2.2, 22.9, -50);
        this.scene.add(hBar1);

        const hBar2 = new THREE.Mesh(new THREE.BoxGeometry(1.2, 0.1, 7), hMat);
        hBar2.position.set(50 + 2.2, 22.9, -50);
        this.scene.add(hBar2);

        const hCross = new THREE.Mesh(new THREE.BoxGeometry(4.4, 0.1, 1.2), hMat);
        hCross.position.set(50, 22.9, -50);
        this.scene.add(hCross);

        // Helicopter Skeleton Body
        this.heliGroup = new THREE.Group();
        const heliMat = new THREE.MeshStandardMaterial({ color: 0x35414d, metalness: 0.6, roughness: 0.4 });
        const frameGeo = new THREE.BoxGeometry(3.5, 2.2, 7.5);
        const heliBody = new THREE.Mesh(frameGeo, heliMat);
        heliBody.position.y = 2.0;
        this.heliGroup.add(heliBody);

        // Tail
        const tailGeo = new THREE.BoxGeometry(0.8, 0.8, 5.0);
        const tail = new THREE.Mesh(tailGeo, heliMat);
        tail.position.set(0, 2.5, -5.5);
        this.heliGroup.add(tail);

        // Assembly Station (Interactable table)
        this.helipadStation = new THREE.Mesh(
            new THREE.BoxGeometry(1.8, 1.2, 1.2),
            new THREE.MeshStandardMaterial({ color: 0x00ccaa, metalness: 0.7 })
        );
        this.helipadStation.position.set(50 + 5.5, 23.5, -50);
        this.helipadStation.isInteractable = true;
        this.helipadStation.isHelipadStation = true;
        this.helipadStation.interactText = "TEKAN [E]: MEJA RAKIT HELIKOPTER (PASANG SUKU CADANG DARI REGION 4)";
        this.scene.add(this.helipadStation);

        this.heliGroup.position.copy(helipadCenter);
        this.scene.add(this.heliGroup);

        // Snow pine trees
        for (let i = 0; i < 18; i++) {
            const x = 30 + Math.random() * 35;
            const z = -25 - Math.random() * 35;
            this.spawnTree(x, z, 'snowPine');
        }
    }

    buildRegion4_VillageRuins() {
        // Ancient flooded village (X: 25 to 70, Z: 25 to 70)
        // Abandoned Huts
        for (let i = 0; i < 5; i++) {
            const x = 35 + (i % 3) * 16;
            const z = 35 + Math.floor(i / 3) * 20;
            this.spawnRuinedHut(x, z);
        }

        // Scatter 4 Critical Helicopter Parts in Region 4!
        this.spawnHeliPart('heliEngine', 'Mesin Turbin Helikopter (Helicopter Engine)', 38, 4.8, 38, 0xff8800);
        this.spawnHeliPart('heliRotor', 'Baling-baling Utama (Rotor Blades)', 55, 4.8, 42, 0x00bbff);
        this.spawnHeliPart('heliFuel', 'Tangki Bahan Bakar Avtur (Aviation Fuel)', 45, 4.8, 62, 0xff2222);
        this.spawnHeliPart('heliAvionics', 'Modul Navigasi & Avionik (Flight Computer)', 66, 4.8, 58, 0xaa00ff);
    }

    spawnHeliPart(partId, partName, x, y, z, color) {
        const geo = new THREE.BoxGeometry(1.0, 0.8, 1.0);
        const mat = new THREE.MeshStandardMaterial({ color: color, metalness: 0.8, roughness: 0.2 });
        const mesh = new THREE.Mesh(geo, mat);
        mesh.position.set(x, y, z);
        mesh.isInteractable = true;
        mesh.isHeliPart = true;
        mesh.partId = partId;
        mesh.partName = partName;
        mesh.interactText = `TEKAN [E]: AMBIL ${partName.toUpperCase()}`;

        // Glow indicator beacon
        const beacon = new THREE.PointLight(color, 2.0, 10);
        beacon.position.y = 1.0;
        mesh.add(beacon);

        this.scene.add(mesh);
        this.heliPartsOnGround.push(mesh);
    }

    spawnRuinedHut(x, z) {
        const hut = new THREE.Group();
        const wallMat = new THREE.MeshStandardMaterial({ color: 0x3d3025, roughness: 0.9 });
        const roofMat = new THREE.MeshStandardMaterial({ color: 0x2b221a, roughness: 0.95 });

        // Wall base
        const walls = new THREE.Mesh(new THREE.BoxGeometry(6, 3.5, 6), wallMat);
        walls.position.y = 1.75;
        hut.add(walls);

        // Slanted roof
        const roof = new THREE.Mesh(new THREE.ConeGeometry(5, 2.5, 4), roofMat);
        roof.position.y = 4.5;
        roof.rotation.y = Math.PI / 4;
        hut.add(roof);

        hut.position.set(x, 4.5, z);
        this.scene.add(hut);
    }

    buildDiaryShelter() {
        // Abandoned Shelter with Diary (Region 1, near starting area)
        const shelter = new THREE.Group();
        const woodMat = new THREE.MeshStandardMaterial({ color: 0x5a4231, roughness: 0.85 });

        const roof = new THREE.Mesh(new THREE.BoxGeometry(4.5, 0.3, 4.5), woodMat);
        roof.position.y = 2.8;
        shelter.add(roof);

        // 4 corner posts
        for (let dx = -1.8; dx <= 1.8; dx += 3.6) {
            for (let dz = -1.8; dz <= 1.8; dz += 3.6) {
                const post = new THREE.Mesh(new THREE.CylinderGeometry(0.15, 0.15, 2.8, 8), woodMat);
                post.position.set(dx, 1.4, dz);
                shelter.add(post);
            }
        }

        // Table with Diary book
        const table = new THREE.Mesh(new THREE.BoxGeometry(1.6, 0.8, 1.0), woodMat);
        table.position.set(0, 0.4, 0);
        shelter.add(table);

        const bookMat = new THREE.MeshStandardMaterial({ color: 0x8b0000, roughness: 0.5 });
        const book = new THREE.Mesh(new THREE.BoxGeometry(0.6, 0.15, 0.45), bookMat);
        book.position.set(0, 0.9, 0);
        book.isInteractable = true;
        book.isDiary = true;
        book.interactText = "TEKAN [E]: BACA BUKU DIARY (PANDUAN SURVIVAL & LORE PULAU)";
        shelter.add(book);

        shelter.position.set(-8, 7.5, -6);
        this.scene.add(shelter);
    }

    buildLuckyMachine(x, y, z) {
        const group = new THREE.Group();
        const metalMat = new THREE.MeshStandardMaterial({ color: 0x223344, metalness: 0.9, roughness: 0.2 });
        const glassMat = new THREE.MeshStandardMaterial({ color: 0x00ffcc, transparent: true, opacity: 0.6 });

        // Base machine cabinet
        const body = new THREE.Mesh(new THREE.BoxGeometry(2.0, 3.2, 1.8), metalMat);
        body.position.y = 1.6;
        group.add(body);

        // Glass Chamber / Gacha Capsule
        const chamber = new THREE.Mesh(new THREE.CylinderGeometry(0.7, 0.7, 1.4, 16), glassMat);
        chamber.position.set(0, 2.2, 0.4);
        group.add(chamber);

        // Machine status beacon
        this.luckyBeacon = new THREE.PointLight(0x00e5ff, 2.5, 12);
        this.luckyBeacon.position.set(0, 3.5, 0);
        group.add(this.luckyBeacon);

        group.position.set(x, y, z);
        group.isInteractable = true;
        group.isLuckyMachine = true;
        group.interactText = "TEKAN [E]: AKTIFKAN MESIN KEBERUNTUNGAN (THE LUCKY MACHINE)";

        this.scene.add(group);
        this.luckyMachineObject = group;
    }

    spawnTree(x, z, type) {
        const group = new THREE.Group();
        const trunkMat = new THREE.MeshStandardMaterial({ color: 0x4a2e18, roughness: 0.9 });
        let foliageMat;

        if (type === 'pine') {
            foliageMat = new THREE.MeshStandardMaterial({ color: 0x1d4722, roughness: 0.8 });
        } else if (type === 'tropical') {
            foliageMat = new THREE.MeshStandardMaterial({ color: 0x10591f, roughness: 0.7 });
        } else if (type === 'snowPine') {
            foliageMat = new THREE.MeshStandardMaterial({ color: 0x9fb3a8, roughness: 0.8 });
        }

        // Trunk
        const trunk = new THREE.Mesh(new THREE.CylinderGeometry(0.3, 0.45, 4.0, 8), trunkMat);
        trunk.position.y = 2.0;
        group.add(trunk);

        // Foliage cone
        const foliage = new THREE.Mesh(new THREE.ConeGeometry(2.2, 4.5, 8), foliageMat);
        foliage.position.y = 4.8;
        group.add(foliage);

        // Position on ground
        const groundY = this.getApproxHeight(x, z);
        group.position.set(x, groundY, z);

        // Resource properties (Choppable for wood)
        group.isResource = true;
        group.resourceType = 'tree';
        group.resourceName = 'Pohon Pinus/Tropis';
        group.hp = 3;

        this.scene.add(group);
        this.interactiveObjects.push(group);
    }

    spawnRock(x, z) {
        const rockMat = new THREE.MeshStandardMaterial({ color: 0x61666d, roughness: 0.9 });
        const rock = new THREE.Mesh(new THREE.DodecahedronGeometry(1.2), rockMat);
        const groundY = this.getApproxHeight(x, z);
        rock.position.set(x, groundY + 0.6, z);

        rock.isResource = true;
        rock.resourceType = 'rock';
        rock.resourceName = 'Bongkahan Batu Tambang';
        rock.hp = 3;

        this.scene.add(rock);
        this.interactiveObjects.push(rock);
    }

    spawnResourcePlant(x, z, type) {
        const mat = new THREE.MeshStandardMaterial({
            color: type === 'herb' ? 0x00ff88 : 0xaa00ff,
            roughness: 0.6
        });
        const geo = type === 'herb' ? new THREE.ConeGeometry(0.4, 0.8, 6) : new THREE.SphereGeometry(0.35, 8, 8);
        const mesh = new THREE.Mesh(geo, mat);
        const groundY = this.getApproxHeight(x, z);
        mesh.position.set(x, groundY + 0.3, z);

        mesh.isInteractable = true;
        mesh.isResourcePlant = true;
        mesh.plantType = type;
        mesh.interactText = type === 'herb' ? "TEKAN [E]: AMBIL TANAMAN GETAH OBAT (HERB)" : "TEKAN [E]: AMBIL JAMUR TROPIS (MUSHROOM)";

        this.scene.add(mesh);
        this.interactiveObjects.push(mesh);
    }

    getApproxHeight(x, z) {
        const dist = Math.hypot(x, z);
        if (dist > 110) return 0.2;
        let h = Math.cos((dist / 110) * Math.PI * 0.5) * 8.0;

        if (x < -20 && z < -20) {
            const vDist = Math.hypot(x + 55, z + 55);
            if (vDist < 40) h += (40 - vDist) * 0.65;
        }
        if (x > 15 && z < -15) {
            const sDist = Math.hypot(x - 50, z + 50);
            if (sDist < 45) h += (45 - sDist) * 0.85;
        }
        return Math.max(0.8, h);
    }

    getCurrentRegion(x, z) {
        if (x < 0 && z < 0) return 1; // Region 1 (Forest & Volcano)
        if (x < 0 && z >= 0) return 2; // Region 2 (Tropical Jungle)
        if (x >= 0 && z < 0) return 3; // Region 3 (Snow Mountain & Helipad)
        return 4;                      // Region 4 (Ruined Village)
    }

    getRegionName(regionId) {
        switch (regionId) {
            case 1: return "Region 1: Hutan Terbuka & Gunung Berapi";
            case 2: return "Region 2: Hutan Tropis Lebat (Amazon)";
            case 3: return "Region 3: Pegunungan Es Salju & Helipad Militer";
            case 4: return "Region 4: Reruntuhan Desa Kuno";
            default: return "Samudera Lepas";
        }
    }
}
