// Diary System (Buku Catatan Peninggalan Penghuni Lama)
export class DiarySystem {
    constructor() {
        this.isOpen = false;
        this.currentChapter = 1;
        this.chapters = [
            {
                chapter: 1,
                title: "Bab 1: Survival Dasar & Peringatan Pantai",
                author: "Catatan Hari ke-4 - Dr. Aris",
                content: `
                <p><strong>Peringatan Pertama:</strong> Jangan pernah mencoba tidur atau membangun pondasi markas di atas pasir pantai! Air pasang liar dan ombak asam malam hari akan meruntuhkan kayumu dalam hitungan menit. Bangunlah di dataran hutan pinus yang kokoh.</p>
                <p><strong>Anomali Makhluk:</strong> Makhluk terinfeksi (Zombie) memiliki sel tubuh yang hipersensitif terhadap radiasi UV. <em>Mereka akan terbakar dan hancur lebur jika terkena sinar matahari siang!</em> Manfaatkan siang hari untuk menjelajah, menebang pohon, dan menambang.</p>
                <p><strong>Kail Serbaguna:</strong> Kail pancing dari kapal bisa kamu lemparkan dari tebing karang pantai untuk menangkap ikan segar saat kelaparan melanda.</p>
                `
            },
            {
                chapter: 2,
                title: "Bab 2: Wabah Infeksi & Blueprint Jembatan",
                author: "Catatan Hari ke-19 - Peneliti Militer",
                content: `
                <p><strong>Infeksi Mematikan:</strong> Gigitan zombie bukan sekadar luka fisik, melainkan penularan mikroba yang menggerogoti organ dalam. Jika indikator infeksi mencapai 100%, kematian tak terelakkan.</p>
                <p><strong>Formula Vaksin:</strong> Kami berhasil meracik serum penangkal menggunakan <em>getah tanaman langka (Herb)</em> dan <em>spora jamur tropis</em>. Masalahnya, flora ini hanya tumbuh subur di <strong>Hutan Hujan Tropis (Region 2)</strong> di seberang jurang.</p>
                <p><strong>Blueprint:</strong> Kamu harus menebang cukup banyak kayu dan batu untuk merakit <strong>Jembatan Gantung</strong> di atas jurang pembatas untuk menyeberang ke Region 2.</p>
                `
            },
            {
                chapter: 3,
                title: "Bab 3: Kebenaran & Pelarian 60 Hari",
                author: "Catatan Hari ke-58 - Komandan Sektor",
                content: `
                <p><strong>Misi Pelarian:</strong> Ada landasan helipad militer dan kerangka helikopter penyelamat di <strong>Puncak Pegunungan Salju (Region 3)</strong>. Namun jangan gegabah, dinginnya puncak salju akan membekukanmu (Frostbite) tanpa pakaian hangat atau api.</p>
                <p><strong>Suku Cadang Tercecer:</strong> Saat kepanikan evakuasi awal, 4 suku cadang vital helikopter tertinggal di <strong>Reruntuhan Desa (Region 4)</strong> di lembah bawah: Mesin, Baling-baling, Tangki Avtur, dan Modul Navigasi.</p>
                <p><strong>Batas Waktu:</strong> Pada <strong>Hari ke-60</strong>, badai kutub tahunan akan menyapu pulau ini selamanya. Bawa suku cadang dari Region 4 menuju Region 3 dan selesaikan rakitan helikopter sebelum waktu habis!</p>
                `
            }
        ];
    }

    open() {
        this.isOpen = true;
    }

    close() {
        this.isOpen = false;
    }

    setChapter(ch) {
        if (ch >= 1 && ch <= 3) {
            this.currentChapter = ch;
        }
    }

    getCurrentContent() {
        return this.chapters[this.currentChapter - 1];
    }
}
