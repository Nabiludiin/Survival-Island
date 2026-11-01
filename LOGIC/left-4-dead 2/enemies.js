import * as THREE from '../../libs/three.module.js';

export class EnemyManager {
    constructor(scene, audio, status, building = null) {
        this.scene = scene;
        this.audio = audio;
        this.status = status;
        this.building = building; // BaseBuilding: zombie menyerang dinding markas
        this.enemies = [];

        // Materials
        this.zombieMat = new THREE.MeshStandardMaterial({ color: 0x4a6b4d, roughness: 0.7 });
        this.zombieCamoMat = new THREE.MeshStandardMaterial({ color: 0x2b4f2c, roughness: 0.9 });
        this.zombieArmorMat = new THREE.MeshStandardMaterial({ color: 0x2e3532, metalness: 0.7, roughness: 0.4 });
        this.zombieFastMat = new THREE.MeshStandardMaterial({ color: 0x5c4238, roughness: 0.8 });
        this.wolfMat = new THREE.MeshStandardMaterial({ color: 0x737984, roughness: 0.8 });
        this.bearMat = new THREE.MeshStandardMaterial({ color: 0xf0f0f5, roughness: 0.7 });
        this.jaguarMat = new THREE.MeshStandardMaterial({ color: 0xd4a048, roughness: 0.8 });
        // Material terbakar terpisah, supaya material zombie bersama tidak ikut berubah merah
        this.zombieBurnMat = new THREE.MeshStandardMaterial({ color: 0xff3300, emissive: 0x661100, roughness: 0.7 });

        this.spawnTimer = 0;
    }

    spawnBeast(type, position, region) {
        const group = new THREE.Group();
        let mat = this.wolfMat;
        let hp = 45;
        let speed = 4.2;
        let damage = 12;
        let scale = 1.0;

        if (type === 'wolf') {
            mat = this.wolfMat;
            hp = 40;
            speed = 4.5;
            damage = 10;
        } else if (type === 'bear') {
            mat = this.bearMat;
            hp = 120;
            speed = 3.2;
            damage = 25;
            scale = 1.5;
        } else if (type === 'jaguar') {
            mat = this.jaguarMat;
            hp = 50;
            speed = 5.2;
            damage = 16;
        }

        // Body
        const bodyGeo = new THREE.BoxGeometry(0.8 * scale, 0.6 * scale, 1.4 * scale);
        const body = new THREE.Mesh(bodyGeo, mat);
        body.position.y = 0.5 * scale;
        group.add(body);

        // Head
        const headGeo = new THREE.BoxGeometry(0.5 * scale, 0.4 * scale, 0.5 * scale);
        const head = new THREE.Mesh(headGeo, mat);
        head.position.set(0, 0.7 * scale, 0.8 * scale);
        group.add(head);

        group.position.copy(position);
        this.enableShadows(group);

        group.enemyType = 'beast';
        group.subType = type;
        group.region = region;
        group.hp = hp;
        group.maxHp = hp;
        group.speed = speed;
        group.damage = damage;
        group.attackCooldown = 0;

        this.scene.add(group);
        this.enemies.push(group);
    }

    spawnZombie(subType, position, region) {
        const group = new THREE.Group();
        let mat = this.zombieMat;
        let hp = 60;
        let speed = 3.0;
        let damage = 14;
        let infectAmount = 15;
        let armor = 0;

        if (subType === 'camo') {
            mat = this.zombieCamoMat;
            hp = 65;
            speed = 3.3;
        } else if (subType === 'military') {
            mat = this.zombieArmorMat;
            hp = 130;
            armor = 0.5; // Takes 50% less bullet damage!
            speed = 2.6;
            damage = 22;
        } else if (subType === 'fast') {
            mat = this.zombieFastMat;
            hp = 45;
            speed = 5.0; // Fast mud/lake runner
            damage = 12;
        }

        // Zombie humanoid body
        const torsoGeo = new THREE.BoxGeometry(0.6, 0.9, 0.35);
        const torso = new THREE.Mesh(torsoGeo, mat);
        torso.position.y = 1.0;
        group.add(torso);

        // Head with glowing reddish eyes
        const headGeo = new THREE.BoxGeometry(0.4, 0.45, 0.4);
        const head = new THREE.Mesh(headGeo, mat);
        head.position.y = 1.65;
        group.add(head);

        // Arms reaching forward (L4D style)
        const armGeo = new THREE.BoxGeometry(0.18, 0.18, 0.7);
        const leftArm = new THREE.Mesh(armGeo, mat);
        leftArm.position.set(-0.38, 1.15, 0.35);
        group.add(leftArm);

        const rightArm = new THREE.Mesh(armGeo, mat);
        rightArm.position.set(0.38, 1.15, 0.35);
        group.add(rightArm);

        // Legs
        const legGeo = new THREE.BoxGeometry(0.2, 0.7, 0.22);
        const leftLeg = new THREE.Mesh(legGeo, mat);
        leftLeg.position.set(-0.18, 0.35, 0);
        group.add(leftLeg);

        const rightLeg = new THREE.Mesh(legGeo, mat);
        rightLeg.position.set(0.18, 0.35, 0);
        group.add(rightLeg);

        group.position.copy(position);
        this.enableShadows(group);

        group.enemyType = 'zombie';
        group.subType = subType;
        group.region = region;
        group.hp = hp;
        group.maxHp = hp;
        group.speed = speed;
        group.damage = damage;
        group.armor = armor;
        group.infectAmount = infectAmount;
        group.attackCooldown = 0;
        group.isBurning = false;

        this.scene.add(group);
        this.enemies.push(group);
    }

    update(dt, playerPos, isDay, currentRegion) {
        const messages = [];
        this.spawnTimer += dt;

        // Auto spawn enemies according to Day/Night and Region
        if (this.spawnTimer > 4.0 && this.enemies.length < 16) {
            this.spawnTimer = 0;
            this.spawnContextualEnemy(playerPos, isDay, currentRegion);
        }

        // Process all enemies
        for (let i = this.enemies.length - 1; i >= 0; i--) {
            const enemy = this.enemies[i];

            // 1. GDD SPEC: Zombie terbakar dan mati di bawah sinar matahari siang!
            if (enemy.enemyType === 'zombie' && isDay) {
                if (!enemy.isBurning) {
                    enemy.isBurning = true;
                    enemy.children.forEach(c => {
                        if (c.isMesh) c.material = this.zombieBurnMat;
                    });
                }
                enemy.hp -= dt * 35.0; // Burn to ash in ~2 seconds!

                if (enemy.hp <= 0) {
                    // Mati karena matahari: tidak ada loot gratis
                    this.killEnemy(i, "Zombie terbakar musnah oleh sinar matahari siang!", false);
                    continue;
                }
            }

            // 2. Daytime beasts return to den at night
            if (enemy.enemyType === 'beast' && !isDay) {
                // Despawn beast at night
                this.removeEnemyAt(i);
                continue;
            }

            // Move towards player, unless a wall of the base blocks the way
            const toPlayer = new THREE.Vector3().subVectors(playerPos, enemy.position);
            toPlayer.y = 0;
            const dist = toPlayer.length();
            enemy.targetStructure = null;

            if (dist > 1.2) {
                toPlayer.normalize();
                const step = enemy.speed * dt;
                const blocker = this.findBlockingStructure(
                    enemy.position.x + toPlayer.x * step,
                    enemy.position.z + toPlayer.z * step
                );
                if (blocker) {
                    enemy.targetStructure = blocker;
                } else {
                    enemy.position.addScaledVector(toPlayer, step);
                }
                enemy.lookAt(playerPos.x, enemy.position.y, playerPos.z);
            }

            // Attack wall (zombie only) or player
            if (enemy.attackCooldown > 0) {
                enemy.attackCooldown -= dt;
            } else if (enemy.targetStructure) {
                if (enemy.enemyType === 'zombie') {
                    const res = this.building.damageStructure(enemy.targetStructure, enemy.damage);
                    if (res.destroyed) {
                        messages.push("🧟 Horde zombie menghancurkan dinding markasmu!");
                    }
                    if (this.audio) this.audio.playHit();
                    enemy.attackCooldown = 1.4;
                }
            } else if (dist <= 2.2) {
                // Strike player
                this.status.takeDamage(enemy.damage, `Diserang oleh ${enemy.subType}`);
                if (enemy.enemyType === 'zombie') {
                    this.status.infect(enemy.infectAmount);
                    if (this.audio) this.audio.playZombieRoar();
                } else {
                    if (this.audio) this.audio.playBeastGrowl();
                }
                if (this.audio) this.audio.playHit();
                enemy.attackCooldown = 1.4;
            }
        }

        return messages;
    }

    findBlockingStructure(x, z) {
        if (!this.building) return null;
        const margin = 0.5;
        for (const s of this.building.structures) {
            if (!s.blocksMovement || !s.bounds) continue;
            const b = s.bounds;
            if (x >= b.min.x - margin && x <= b.max.x + margin &&
                z >= b.min.z - margin && z <= b.max.z + margin) {
                return s;
            }
        }
        return null;
    }

    enableShadows(group) {
        group.traverse(c => {
            if (c.isMesh) c.castShadow = true;
        });
    }

    spawnContextualEnemy(playerPos, isDay, region) {
        // Spawn radius: 25-40m away from player
        const angle = Math.random() * Math.PI * 2;
        const radius = 25 + Math.random() * 15;
        const spawnPos = new THREE.Vector3(
            playerPos.x + Math.cos(angle) * radius,
            0,
            playerPos.z + Math.sin(angle) * radius
        );

        if (isDay) {
            // Day: Territorial Beasts only!
            if (region === 1) {
                this.spawnBeast('wolf', spawnPos, 1);
            } else if (region === 2) {
                this.spawnBeast('jaguar', spawnPos, 2);
            } else if (region === 3) {
                this.spawnBeast('bear', spawnPos, 3);
            } else {
                this.spawnBeast('wolf', spawnPos, 4);
            }
        } else {
            // Night: Horde Zombie emerges!
            if (region === 1) {
                this.spawnZombie('standard', spawnPos, 1);
            } else if (region === 2) {
                this.spawnZombie(Math.random() > 0.5 ? 'camo' : 'standard', spawnPos, 2);
            } else if (region === 3) {
                this.spawnZombie(Math.random() > 0.4 ? 'military' : 'standard', spawnPos, 3);
            } else {
                this.spawnZombie(Math.random() > 0.3 ? 'fast' : 'standard', spawnPos, 4);
            }
            if (this.audio) this.audio.playZombieRoar();
        }
    }

    damageEnemy(enemy, damage, isBullet = false) {
        const idx = this.enemies.indexOf(enemy);
        if (idx === -1) return null;

        // Rompi anti-peluru zombie militer hanya mengurangi damage peluru
        const effectiveDamage = (isBullet && enemy.armor) ? damage * (1 - enemy.armor) : damage;
        enemy.hp -= effectiveDamage;

        if (this.audio) this.audio.playHit();

        if (enemy.hp <= 0) {
            return this.killEnemy(idx, `Mengalahkan ${enemy.subType}`);
        }
        return { killed: false, hp: enemy.hp };
    }

    killEnemy(index, msg, giveLoot = true) {
        const enemy = this.enemies[index];
        this.removeEnemyAt(index);

        // Chance to drop resources
        const dropChance = Math.random();
        let drop = null;
        if (!giveLoot) {
            // no drop
        } else if (enemy.enemyType === 'beast') {
            this.status.inventory.food += 1;
            drop = "+1 Daging Segar";
        } else if (enemy.subType === 'military') {
            this.status.inventory.ammo += 10;
            drop = "+10 Peluru Senjata Militer";
        } else if (dropChance > 0.6) {
            this.status.inventory.ammo += 4;
            drop = "+4 Amunisi";
        }

        return { killed: true, enemy, drop, msg };
    }

    removeEnemyAt(index) {
        const enemy = this.enemies[index];
        this.scene.remove(enemy);
        this.enemies.splice(index, 1);
        // Geometry is created per enemy; materials are shared and stay alive
        enemy.traverse(c => {
            if (c.geometry) c.geometry.dispose();
        });
    }

    clear() {
        for (let i = this.enemies.length - 1; i >= 0; i--) {
            this.removeEnemyAt(i);
        }
    }
}
