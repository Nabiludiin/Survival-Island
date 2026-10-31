import * as THREE from '../../libs/three.module.js';

export class CombatHorde {
    constructor(scene, camera, player, audio, status, enemyManager) {
        this.scene = scene;
        this.camera = camera;
        this.player = player;
        this.audio = audio;
        this.status = status;
        this.enemyManager = enemyManager;

        this.currentWeapon = 'pistol'; // 'hook' or 'pistol'
        this.magAmmo = 12;
        this.magCapacity = 12;
        this.isReloading = false;
        this.shootCooldown = 0;

        // Visual weapon models attached to camera
        this.weaponHolder = new THREE.Group();
        this.camera.add(this.weaponHolder);

        this.pistolMesh = null;
        this.hookMesh = null;
        this.muzzleLight = null;

        this.buildWeaponModels();
    }

    buildWeaponModels() {
        // 1. Pistol Model
        this.pistolMesh = new THREE.Group();
        const slideMat = new THREE.MeshStandardMaterial({ color: 0x1f2326, metalness: 0.8, roughness: 0.3 });
        const gripMat = new THREE.MeshStandardMaterial({ color: 0x47392e, roughness: 0.7 });

        // Barrel & Slide
        const slideGeo = new THREE.BoxGeometry(0.08, 0.1, 0.32);
        const slide = new THREE.Mesh(slideGeo, slideMat);
        slide.position.set(0, 0.08, -0.05);
        this.pistolMesh.add(slide);

        // Grip
        const gripGeo = new THREE.BoxGeometry(0.07, 0.2, 0.1);
        const grip = new THREE.Mesh(gripGeo, gripMat);
        grip.position.set(0, -0.05, 0.04);
        grip.rotation.x = 0.25;
        this.pistolMesh.add(grip);

        this.pistolMesh.position.set(0.3, -0.25, -0.45);
        this.weaponHolder.add(this.pistolMesh);

        // Muzzle Light
        this.muzzleLight = new THREE.PointLight(0xffaa22, 0, 8);
        this.muzzleLight.position.set(0.3, -0.15, -0.7);
        this.weaponHolder.add(this.muzzleLight);

        // 2. Kail (Fishing Hook / Melee) Model
        this.hookMesh = new THREE.Group();
        const woodPoleMat = new THREE.MeshStandardMaterial({ color: 0x7c4f2e, roughness: 0.8 });
        const metalHookMat = new THREE.MeshStandardMaterial({ color: 0xc0c5ce, metalness: 0.9, roughness: 0.2 });

        const poleGeo = new THREE.CylinderGeometry(0.03, 0.03, 0.6, 8);
        const pole = new THREE.Mesh(poleGeo, woodPoleMat);
        pole.rotation.x = Math.PI / 4;
        this.hookMesh.add(pole);

        const hookCurvGeo = new THREE.TorusGeometry(0.12, 0.025, 8, 16, Math.PI * 1.3);
        const hookCurv = new THREE.Mesh(hookCurvGeo, metalHookMat);
        hookCurv.position.set(0, 0.2, -0.25);
        hookCurv.rotation.z = Math.PI / 2;
        this.hookMesh.add(hookCurv);

        this.hookMesh.position.set(0.32, -0.25, -0.45);
        this.weaponHolder.add(this.hookMesh);

        this.setWeapon('pistol');
    }

    setWeapon(weaponName) {
        this.currentWeapon = weaponName;
        if (this.pistolMesh) this.pistolMesh.visible = (weaponName === 'pistol');
        if (this.hookMesh) this.hookMesh.visible = (weaponName === 'hook');
    }

    switchWeapon() {
        if (this.currentWeapon === 'pistol') {
            this.setWeapon('hook');
        } else {
            this.setWeapon('pistol');
        }
        return this.currentWeapon;
    }

    attack(interactiveTargets) {
        if (this.shootCooldown > 0 || this.isReloading) return null;

        if (this.currentWeapon === 'pistol') {
            return this.shootPistol(interactiveTargets);
        } else {
            return this.swingMelee(interactiveTargets);
        }
    }

    shootPistol(interactiveTargets) {
        if (this.magAmmo <= 0) {
            this.reload();
            return { action: 'empty', msg: "Amunisi habis! Menekan reload..." };
        }

        this.magAmmo--;
        this.shootCooldown = 0.25; // 4 shots per sec

        if (this.audio) this.audio.playGunshot();

        // Muzzle flash animation
        this.muzzleLight.intensity = 4.0;
        this.pistolMesh.position.z += 0.08; // Recoil backward
        setTimeout(() => {
            this.muzzleLight.intensity = 0;
            this.pistolMesh.position.z -= 0.08;
        }, 60);

        // Raycast shoot forward from camera center
        const raycaster = new THREE.Raycaster();
        raycaster.setFromCamera(new THREE.Vector2(0, 0), this.camera);
        raycaster.far = 80;

        // Gather targets: enemies + resource objects
        const enemyMeshes = [];
        for (const e of this.enemyManager.enemies) {
            e.traverse(child => {
                if (child.isMesh) {
                    child.rootEnemy = e;
                    enemyMeshes.push(child);
                }
            });
        }

        const hits = raycaster.intersectObjects([...enemyMeshes, ...(interactiveTargets || [])], false);

        if (hits.length > 0) {
            const hit = hits[0];
            if (hit.object.rootEnemy) {
                const res = this.enemyManager.damageEnemy(hit.object.rootEnemy, 40);
                return { action: 'hitEnemy', ...res };
            }
            if (hit.object.isResource) {
                return { action: 'hitResource', object: hit.object, point: hit.point };
            }
        }

        return { action: 'shoot', magAmmo: this.magAmmo, totalAmmo: this.status.inventory.ammo };
    }

    swingMelee(interactiveTargets) {
        this.shootCooldown = 0.45;
        if (this.audio) this.audio.playMelee();

        // Swing animation
        this.hookMesh.rotation.x -= 0.6;
        setTimeout(() => {
            this.hookMesh.rotation.x += 0.6;
        }, 150);

        // Raycast melee in front (short distance ~3m)
        const raycaster = new THREE.Raycaster();
        raycaster.setFromCamera(new THREE.Vector2(0, 0), this.camera);
        raycaster.far = 4.0;

        const enemyMeshes = [];
        for (const e of this.enemyManager.enemies) {
            e.traverse(child => {
                if (child.isMesh) {
                    child.rootEnemy = e;
                    enemyMeshes.push(child);
                }
            });
        }

        const hits = raycaster.intersectObjects([...enemyMeshes, ...(interactiveTargets || [])], false);

        if (hits.length > 0) {
            const hit = hits[0];
            if (hit.object.rootEnemy) {
                const res = this.enemyManager.damageEnemy(hit.object.rootEnemy, 25);
                return { action: 'hitEnemy', ...res };
            }
            if (hit.object.isResource) {
                return { action: 'hitResource', object: hit.object, point: hit.point };
            }
        }

        return { action: 'swing' };
    }

    reload() {
        if (this.isReloading || this.magAmmo === this.magCapacity) return;
        if (this.status.inventory.ammo <= 0) {
            return { success: false, msg: "Tidak ada amunisi tersisa di inventori!" };
        }

        this.isReloading = true;
        const needed = this.magCapacity - this.magAmmo;
        const canTake = Math.min(needed, this.status.inventory.ammo);

        setTimeout(() => {
            this.status.inventory.ammo -= canTake;
            this.magAmmo += canTake;
            this.isReloading = false;
        }, 1200);

        return { success: true, msg: "Mengisi peluru..." };
    }

    update(dt) {
        if (this.shootCooldown > 0) {
            this.shootCooldown -= dt;
        }
    }
}
