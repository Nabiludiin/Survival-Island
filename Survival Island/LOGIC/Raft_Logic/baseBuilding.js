import * as THREE from '../../libs/three.module.js';

export class BaseBuilding {
    constructor(scene, audio) {
        this.scene = scene;
        this.audio = audio;
        this.structures = [];
        this.previewMesh = null;
        this.currentBuildType = null;

        // Visual materials
        this.woodMat = new THREE.MeshStandardMaterial({ color: 0x7c4f2e, roughness: 0.85 });
        this.stoneMat = new THREE.MeshStandardMaterial({ color: 0x5a5d64, roughness: 0.9 });
        this.fireMat = new THREE.MeshBasicMaterial({ color: 0xff6600 });
        this.previewMat = new THREE.MeshBasicMaterial({ color: 0x00ff88, wireframe: true });
    }

    startPlacing(type) {
        this.cancelPlacing();
        this.currentBuildType = type;

        let geo;
        if (type === 'wall') {
            geo = new THREE.BoxGeometry(4.0, 3.0, 0.4);
        } else if (type === 'floor') {
            geo = new THREE.BoxGeometry(4.0, 0.3, 4.0);
        } else if (type === 'campfire') {
            geo = new THREE.CylinderGeometry(0.8, 1.0, 0.5, 8);
        }

        if (geo) {
            this.previewMesh = new THREE.Mesh(geo, this.previewMat);
            this.scene.add(this.previewMesh);
        }
    }

    cancelPlacing() {
        if (this.previewMesh) {
            this.scene.remove(this.previewMesh);
            this.previewMesh = null;
        }
        this.currentBuildType = null;
    }

    updatePreview(hitPoint, hitNormal) {
        if (!this.previewMesh || !hitPoint) return;
        this.previewMesh.position.copy(hitPoint);
        if (this.currentBuildType === 'wall') {
            this.previewMesh.position.y += 1.5;
        } else if (this.currentBuildType === 'floor') {
            this.previewMesh.position.y += 0.15;
        } else if (this.currentBuildType === 'campfire') {
            this.previewMesh.position.y += 0.25;
        }
    }

    placeStructure(point, playerPos) {
        if (!this.currentBuildType || !point) return { success: false, msg: "Tidak ada objek aktif" };

        // 1. GDD RESTRIKSI PANTAI: Dilarang keras membangun di pasir pantai!
        // Beach area is low altitude near water level (< 3.0m) or outer perimeter (> 95m from island center)
        const distFromCenter = Math.sqrt(point.x * point.x + point.z * point.z);
        if (point.y < 2.5 || distFromCenter > 95) {
            return {
                success: false,
                msg: "⚠️ ATURAN SURVIVAL: DILARANG MEMBANGUN DI ATAS PASIR PANTAI! Air pasang akan merusak struktur. Bangun di daratan hutan!"
            };
        }

        let mesh;
        let isCampfire = false;

        if (this.currentBuildType === 'wall') {
            const geo = new THREE.BoxGeometry(4.0, 3.0, 0.4);
            mesh = new THREE.Mesh(geo, this.woodMat);
            mesh.position.set(point.x, point.y + 1.5, point.z);
            mesh.hp = 250;
        } else if (this.currentBuildType === 'floor') {
            const geo = new THREE.BoxGeometry(4.0, 0.3, 4.0);
            mesh = new THREE.Mesh(geo, this.woodMat);
            mesh.position.set(point.x, point.y + 0.15, point.z);
            mesh.hp = 300;
        } else if (this.currentBuildType === 'campfire') {
            mesh = new THREE.Group();
            const ringGeo = new THREE.CylinderGeometry(0.8, 1.0, 0.4, 8);
            const stones = new THREE.Mesh(ringGeo, this.stoneMat);
            mesh.add(stones);

            const fireGeo = new THREE.ConeGeometry(0.4, 0.8, 6);
            const fire = new THREE.Mesh(fireGeo, this.fireMat);
            fire.position.y = 0.5;
            mesh.add(fire);

            // Light emitter
            const fireLight = new THREE.PointLight(0xff7722, 2.0, 15);
            fireLight.position.y = 1.0;
            mesh.add(fireLight);

            mesh.position.set(point.x, point.y + 0.2, point.z);
            mesh.hp = 100;
            isCampfire = true;
        }

        mesh.castShadow = true;
        mesh.receiveShadow = true;
        mesh.isStructure = true;
        mesh.isCampfire = isCampfire;
        this.scene.add(mesh);
        this.structures.push(mesh);

        if (this.audio) this.audio.playCraft();
        this.cancelPlacing();

        return { success: true, msg: "Bangunan berhasil didirikan di atas daratan!" };
    }

    isNearCampfire(playerPos) {
        for (const s of this.structures) {
            if (s.isCampfire) {
                const dist = s.position.distanceTo(playerPos);
                if (dist < 6.0) return true;
            }
        }
        return false;
    }
}
