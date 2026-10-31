// Audio Synthesizer using Web Audio API (Zero external assets required)
export class AudioManager {
    constructor() {
        this.ctx = null;
        this.ambientGain = null;
        this.isMuted = false;
        this.initialized = false;
    }

    init() {
        if (this.initialized) return;
        const AudioCtx = window.AudioContext || window.webkitAudioContext;
        if (!AudioCtx) return;
        this.ctx = new AudioCtx();
        this.masterGain = this.ctx.createGain();
        this.masterGain.gain.value = 0.5;
        this.masterGain.connect(this.ctx.destination);
        this.initialized = true;
        this.startAmbientLoop();
    }

    resume() {
        if (this.ctx && this.ctx.state === 'suspended') {
            this.ctx.resume();
        }
    }

    toggleMute() {
        this.isMuted = !this.isMuted;
        if (this.masterGain) {
            this.masterGain.gain.value = this.isMuted ? 0 : 0.5;
        }
        return this.isMuted;
    }

    // Gunshot: noise burst + low kick
    playGunshot() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;

        // Low thump
        const osc = this.ctx.createOscillator();
        const oscGain = this.ctx.createGain();
        osc.type = 'triangle';
        osc.frequency.setValueAtTime(150, t);
        osc.frequency.exponentialRampToValueAtTime(30, t + 0.15);
        oscGain.gain.setValueAtTime(0.8, t);
        oscGain.gain.exponentialRampToValueAtTime(0.001, t + 0.2);
        osc.connect(oscGain);
        oscGain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 0.2);

        // Noise crack
        const bufferSize = this.ctx.sampleRate * 0.1;
        const buffer = this.ctx.createBuffer(1, bufferSize, this.ctx.sampleRate);
        const data = buffer.getChannelData(0);
        for (let i = 0; i < bufferSize; i++) {
            data[i] = (Math.random() * 2 - 1) * Math.exp(-i / (bufferSize * 0.2));
        }
        const noise = this.ctx.createBufferSource();
        noise.buffer = buffer;
        const filter = this.ctx.createBiquadFilter();
        filter.type = 'highpass';
        filter.frequency.setValueAtTime(1000, t);
        noise.connect(filter);
        filter.connect(this.masterGain);
        noise.start(t);
    }

    // Melee attack whoosh
    playMelee() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const gain = this.ctx.createGain();
        osc.type = 'sine';
        osc.frequency.setValueAtTime(350, t);
        osc.frequency.exponentialRampToValueAtTime(90, t + 0.18);
        gain.gain.setValueAtTime(0.4, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 0.18);
        osc.connect(gain);
        gain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 0.18);
    }

    // Hit flesh / impact
    playHit() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const gain = this.ctx.createGain();
        osc.type = 'sawtooth';
        osc.frequency.setValueAtTime(120, t);
        osc.frequency.linearRampToValueAtTime(40, t + 0.12);
        gain.gain.setValueAtTime(0.6, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 0.12);
        osc.connect(gain);
        gain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 0.12);
    }

    // Zombie roar/groan
    playZombieRoar() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const filter = this.ctx.createBiquadFilter();
        const gain = this.ctx.createGain();

        osc.type = 'sawtooth';
        osc.frequency.setValueAtTime(110 + Math.random() * 40, t);
        osc.frequency.linearRampToValueAtTime(60, t + 0.5);

        filter.type = 'bandpass';
        filter.frequency.setValueAtTime(600, t);
        filter.frequency.linearRampToValueAtTime(300, t + 0.5);
        filter.Q.value = 4.0;

        gain.gain.setValueAtTime(0.3, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 0.5);

        osc.connect(filter);
        filter.connect(gain);
        gain.connect(this.masterGain);

        osc.start(t);
        osc.stop(t + 0.5);
    }

    // Beast roar (wolves / bears)
    playBeastGrowl() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const gain = this.ctx.createGain();
        osc.type = 'sawtooth';
        osc.frequency.setValueAtTime(90, t);
        osc.frequency.linearRampToValueAtTime(50, t + 0.4);
        gain.gain.setValueAtTime(0.4, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 0.4);
        osc.connect(gain);
        gain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 0.4);
    }

    // Pickup / Chime
    playPickup() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const gain = this.ctx.createGain();
        osc.type = 'triangle';
        osc.frequency.setValueAtTime(440, t);
        osc.frequency.setValueAtTime(660, t + 0.08);
        osc.frequency.setValueAtTime(880, t + 0.16);
        gain.gain.setValueAtTime(0.3, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 0.3);
        osc.connect(gain);
        gain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 0.3);
    }

    // Craft / Build sound
    playCraft() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const gain = this.ctx.createGain();
        osc.type = 'square';
        osc.frequency.setValueAtTime(220, t);
        osc.frequency.setValueAtTime(440, t + 0.06);
        gain.gain.setValueAtTime(0.2, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 0.18);
        osc.connect(gain);
        gain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 0.18);
    }

    // Siren alarm for Phase 1 ship sink and Volcano Eruption
    playAlarm() {
        if (!this.initialized || this.isMuted) return;
        this.resume();
        const t = this.ctx.currentTime;
        const osc = this.ctx.createOscillator();
        const gain = this.ctx.createGain();
        osc.type = 'sawtooth';
        osc.frequency.setValueAtTime(500, t);
        osc.frequency.linearRampToValueAtTime(850, t + 0.5);
        osc.frequency.linearRampToValueAtTime(500, t + 1.0);
        gain.gain.setValueAtTime(0.2, t);
        gain.gain.exponentialRampToValueAtTime(0.01, t + 1.0);
        osc.connect(gain);
        gain.connect(this.masterGain);
        osc.start(t);
        osc.stop(t + 1.0);
    }

    // Ambient loop (wind & ocean)
    startAmbientLoop() {
        if (!this.initialized || this.ambientGain) return;
        const bufferSize = this.ctx.sampleRate * 2;
        const buffer = this.ctx.createBuffer(1, bufferSize, this.ctx.sampleRate);
        const data = buffer.getChannelData(0);
        let lastOut = 0.0;
        for (let i = 0; i < bufferSize; i++) {
            const white = Math.random() * 2 - 1;
            data[i] = (lastOut + 0.02 * white) / 1.02;
            lastOut = data[i];
        }
        const noise = this.ctx.createBufferSource();
        noise.buffer = buffer;
        noise.loop = true;

        const filter = this.ctx.createBiquadFilter();
        filter.type = 'lowpass';
        filter.frequency.setValueAtTime(280, this.ctx.currentTime);

        this.ambientGain = this.ctx.createGain();
        this.ambientGain.gain.setValueAtTime(0.15, this.ctx.currentTime);

        noise.connect(filter);
        filter.connect(this.ambientGain);
        this.ambientGain.connect(this.masterGain);
        noise.start();
    }
}
