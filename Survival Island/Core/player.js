import * as THREE from '../libs/three.module.js';

export class FirstPersonPlayer {
    constructor(camera, domElement, audio) {
        this.camera = camera;
        this.domElement = domElement;
        this.audio = audio;

        this.position = new THREE.Vector3(0, 1.7, 0);
        this.velocity = new THREE.Vector3();
        this.pitch = 0;
        this.yaw = 0;

        this.isLocked = false;
        this.keys = {
            forward: false,
            backward: false,
            left: false,
            right: false,
            sprint: false,
            jump: false
        };

        this.isGrounded = true;
        this.walkSpeed = 5.5;
        this.sprintSpeed = 9.0;
        this.jumpForce = 7.5;
        this.gravity = 22.0;

        this.bobTimer = 0;
        this.footstepTimer = 0;

        // Interaction
        this.raycaster = new THREE.Raycaster();
        this.hoveredObject = null;

        this.setupPointerLock();
        this.setupKeyboard();
    }

    setupPointerLock() {
        this.domElement.addEventListener('click', () => {
            if (!this.isLocked && document.pointerLockElement !== this.domElement) {
                this.domElement.requestPointerLock();
            }
        });

        document.addEventListener('pointerlockchange', () => {
            this.isLocked = (document.pointerLockElement === this.domElement);
        });

        document.addEventListener('mousemove', (e) => {
            if (!this.isLocked) return;

            const sensitivity = 0.0022;
            this.yaw -= e.movementX * sensitivity;
            this.pitch -= e.movementY * sensitivity;

            // Clamp pitch (-85 to +85 degrees)
            const maxPitch = Math.PI / 2 - 0.05;
            this.pitch = Math.max(-maxPitch, Math.min(maxPitch, this.pitch));
        });
    }

    setupKeyboard() {
        window.addEventListener('keydown', (e) => {
            switch (e.code) {
                case 'KeyW': case 'ArrowUp': this.keys.forward = true; break;
                case 'KeyS': case 'ArrowDown': this.keys.backward = true; break;
                case 'KeyA': case 'ArrowLeft': this.keys.left = true; break;
                case 'KeyD': case 'ArrowRight': this.keys.right = true; break;
                case 'ShiftLeft': case 'ShiftRight': this.keys.sprint = true; break;
                case 'Space':
                    if (this.isGrounded) {
                        this.velocity.y = this.jumpForce;
                        this.isGrounded = false;
                    }
                    break;
            }
        });

        window.addEventListener('keyup', (e) => {
            switch (e.code) {
                case 'KeyW': case 'ArrowUp': this.keys.forward = false; break;
                case 'KeyS': case 'ArrowDown': this.keys.backward = false; break;
                case 'KeyA': case 'ArrowLeft': this.keys.left = false; break;
                case 'KeyD': case 'ArrowRight': this.keys.right = false; break;
                case 'ShiftLeft': case 'ShiftRight': this.keys.sprint = false; break;
            }
        });
    }

    update(dt, worldHeightFunc, isPhase1 = false) {
        // Calculate move vector in horizontal camera plane
        const moveVector = new THREE.Vector3();
        if (this.keys.forward) moveVector.z -= 1;
        if (this.keys.backward) moveVector.z += 1;
        if (this.keys.left) moveVector.x -= 1;
        if (this.keys.right) moveVector.x += 1;

        const isMoving = moveVector.lengthSq() > 0;
        if (isMoving) {
            moveVector.normalize();
        }

        // Apply yaw rotation to movement
        const speed = this.keys.sprint ? this.sprintSpeed : this.walkSpeed;
        const sinYaw = Math.sin(this.yaw);
        const cosYaw = Math.cos(this.yaw);

        const worldMoveX = (moveVector.x * cosYaw + moveVector.z * sinYaw) * speed;
        const worldMoveZ = (-moveVector.x * sinYaw + moveVector.z * cosYaw) * speed;

        this.velocity.x = worldMoveX;
        this.velocity.z = worldMoveZ;

        // Apply gravity
        this.velocity.y -= this.gravity * dt;

        // Update position
        this.position.x += this.velocity.x * dt;
        this.position.y += this.velocity.y * dt;
        this.position.z += this.velocity.z * dt;

        // Ground collision & terrain clamp
        if (worldHeightFunc && !isPhase1) {
            const groundY = worldHeightFunc(this.position.x, this.position.z);
            const playerEyeHeight = 1.7;

            // Restrict from wandering infinitely deep into the ocean
            const distFromCenter = Math.hypot(this.position.x, this.position.z);
            if (distFromCenter > 115) {
                // Ocean current pushes back
                const angle = Math.atan2(this.position.z, this.position.x);
                this.position.x = Math.cos(angle) * 114;
                this.position.z = Math.sin(angle) * 114;
            }

            if (this.position.y < groundY + playerEyeHeight) {
                this.position.y = groundY + playerEyeHeight;
                this.velocity.y = 0;
                this.isGrounded = true;
            }
        } else if (isPhase1) {
            // Ship floor height constraint
            if (this.position.y < 1.7) {
                this.position.y = 1.7;
                this.velocity.y = 0;
                this.isGrounded = true;
            }
            // Constrain inside ship corridor (-4.2 to 4.2 X, -19.5 to 19.5 Z)
            this.position.x = Math.max(-4.2, Math.min(4.2, this.position.x));
            this.position.z = Math.max(-19.5, Math.min(19.5, this.position.z));
        }

        // Head bobbing
        let bobOffset = 0;
        if (isMoving && this.isGrounded) {
            const bobFreq = this.keys.sprint ? 14 : 9;
            this.bobTimer += dt * bobFreq;
            bobOffset = Math.sin(this.bobTimer) * 0.06;

            // Footsteps
            this.footstepTimer += dt;
            if (this.footstepTimer > (this.keys.sprint ? 0.32 : 0.5)) {
                this.footstepTimer = 0;
                if (this.audio) this.audio.playHit();
            }
        } else {
            this.bobTimer = 0;
        }

        // Update Camera
        this.camera.position.set(this.position.x, this.position.y + bobOffset, this.position.z);
        this.camera.rotation.order = 'YXZ';
        this.camera.rotation.y = this.yaw;
        this.camera.rotation.x = this.pitch;
    }

    checkInteraction(interactiveObjects) {
        this.raycaster.setFromCamera(new THREE.Vector2(0, 0), this.camera);
        this.raycaster.far = 4.5;

        // Flatten interactive objects if group
        const candidates = [];
        for (const obj of interactiveObjects) {
            if (obj.isMesh) {
                candidates.push(obj);
            } else if (obj.isGroup) {
                obj.traverse(child => {
                    if (child.isMesh) {
                        child.parentGroup = obj;
                        candidates.push(child);
                    }
                });
            }
        }

        const hits = this.raycaster.intersectObjects(candidates, false);
        if (hits.length > 0) {
            const hit = hits[0].object;
            const target = hit.isInteractable ? hit : (hit.parentGroup && hit.parentGroup.isInteractable ? hit.parentGroup : hit);
            this.hoveredObject = target;
            return {
                target,
                text: target.interactText || "TEKAN [E]: INTERAKSI",
                point: hits[0].point
            };
        }

        this.hoveredObject = null;
        return null;
    }
}
