#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalIsland
{
    /// <summary>Handler perintah konsol. Mengembalikan teks hasil. Blueprint: Core_Logic/DevConsole.md (#56).</summary>
    public delegate string ConsoleHandler(string[] args);

    /// <summary>Satu perintah konsol.</summary>
    public sealed class ConsoleCommand
    {
        public string name;        // Huruf kecil, tanpa spasi
        public string usage;       // Contoh: "additem <itemId> [jumlah]"
        public string help;        // Satu kalimat
        public int minArgs;
        public ConsoleHandler run; // Mengembalikan teks hasil
        public bool taints;        // Perintah mengubah kondisi permainan, jadi run ditandai [CHEAT]

        public ConsoleCommand(string name, string usage, string help, int minArgs, bool taints, ConsoleHandler run)
        {
            this.name = name; this.usage = usage; this.help = help; this.minArgs = minArgs; this.taints = taints; this.run = run;
        }
    }

    /// <summary>
    /// Konsol perintah tersembunyi untuk debugging dan testing. Usulan teknis, belum ada di GDD; membantu menguji semua
    /// sistem, termasuk hari ke-60 dan helikopter (GDD bagian 3). Blueprint: Core_Logic/DevConsole.md (#56).
    ///
    /// Manager persisten (IBootstrapService, urutan bootstrap 18) dan sekaligus UIPanel id DevConsole (#83).
    /// Seluruh file hanya ada di Editor dan DEVELOPMENT_BUILD (keputusan developer, usulan 1: build rilis tanpa konsol).
    /// Panel ditampilkan lewat panelRoot (anak), supaya komponen ini tetap aktif dan Update tetap membaca tombol ~.
    ///
    /// Perintah untuk sistem yang belum ada mencetak pesan TODO. Sistem lain boleh mendaftarkan perintah lewat Register,
    /// dengan #if yang sama, dan tidak boleh menyimpan rujukan ke objek scene.
    /// </summary>
    public class DevConsole : UIPanel, IBootstrapService
    {
        public static DevConsole Instance { get; private set; }

        public const string HistoryPrefsKey = "dev.console.history";

        [SerializeField] private GameObject panelRoot;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Text outputText;
        [Tooltip("Baris penanda [CHEAT], tampil kalau IsRunTainted.")]
        [SerializeField] private GameObject taintLabel;

        private readonly Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();
        private readonly List<string> sortedNames = new List<string>();
        private readonly List<string> history = new List<string>();
        private readonly List<string> tokenBuffer = new List<string>(8);
        private readonly StringBuilder builder = new StringBuilder(4096);
        private string[] outputLines = new string[1];
        private int outputStart;
        private int outputCount;
        private bool outputDirty;
        private int historyIndex = -1;
        private bool isOpen;
        private bool isRunTainted;
        private bool godmode;
        private bool eventLogOn;
        private bool initialized;

        /// <summary>Run ini memakai cheat. Run lanjutan membaca penanda wasTainted di data GameManager.</summary>
        public bool IsRunTainted
        {
            get
            {
                var gm = GameManager.Instance;
                return isRunTainted || (gm != null && gm.wasTainted);
            }
        }

        private static ConsoleBalance B
        {
            get
            {
                var cfg = GameManager.CurrentBalance;
                return cfg != null ? cfg.console : null;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[DevConsole] Instance ganda. Yang baru dimatikan.");
                enabled = false;
                return;
            }
            Instance = this;
            if (panelRoot != null) panelRoot.SetActive(false);
        }

        protected override void OnDestroy()
        {
            if (Instance == this) Instance = null;
            base.OnDestroy();
        }

        /// <summary>Bangun daftar perintah, lalu muat riwayat dari PlayerPrefs.</summary>
        public void Initialize()
        {
            var b = B;
            outputLines = new string[b != null ? Mathf.Max(10, b.maxOutputLines) : 200];
            outputStart = 0;
            outputCount = 0;

            commands.Clear();
            sortedNames.Clear();
            RegisterBuiltIns();

            history.Clear();
            string raw = PlayerPrefs.GetString(HistoryPrefsKey, string.Empty);
            if (!string.IsNullOrEmpty(raw))
            {
                var lines = raw.Split('\n');
                for (int i = 0; i < lines.Length; i++) if (lines[i].Length > 0) history.Add(lines[i]);
            }
            historyIndex = -1;

            if (inputField != null)
            {
                inputField.onValidateInput = ValidateChar;
                inputField.onSubmit.RemoveListener(HandleSubmit);
                inputField.onSubmit.AddListener(HandleSubmit);
            }
            initialized = true;
        }

        /// <summary>Permadeath: matikan godmode, eventlog, dan timescale. history dan forcedNextSeed tetap.</summary>
        public void ResetForNewRun()
        {
            if (godmode && SurvivalStats.Instance != null) SurvivalStats.Instance.SetInvulnerable(false);
            godmode = false;
            SetEventLog(false);
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.SetGameplayTimeScale(1f);
            isRunTainted = false;
        }

        // ---------------------------------------------------------------- UIPanel

        protected override void Show()
        {
            isOpen = true;
            if (panelRoot != null) panelRoot.SetActive(true);
            if (inputField != null)
            {
                // Karakter ` dan ~ tidak ikut masuk ke kolom ketik.
                inputField.onValidateInput = ValidateChar;
                inputField.text = string.Empty;
            }
            outputDirty = true;
        }

        protected override void Hide()
        {
            isOpen = false;
            if (inputField != null) inputField.DeactivateInputField();
            if (panelRoot != null) panelRoot.SetActive(false);
        }

        public override void OnFocusGained()
        {
            if (inputField != null) inputField.ActivateInputField();
        }

        private static char ValidateChar(string text, int charIndex, char addedChar)
        {
            // Tab dipakai pelengkapan nama, bukan karakter.
            if (addedChar == '`' || addedChar == '~' || addedChar == '\t') return '\0';
            return addedChar;
        }

        // ---------------------------------------------------------------- Per frame (waktu tak terskala)

        private void Update()
        {
            if (!initialized) return;

            // Action Map Debug selalu aktif. UIFocusStack tidak memakai toggleAction untuk panel ini.
            var input = InputManager.Instance;
            if (input != null && input.ConsoleTogglePressed)
            {
                var stack = UIFocusStack.Instance;
                if (stack != null)
                {
                    if (stack.Contains(this)) stack.Close(this, UICloseReason.User);
                    else stack.Push(this);
                }
            }

            if (taintLabel != null && taintLabel.activeSelf != IsRunTainted) taintLabel.SetActive(IsRunTainted);

            if (isOpen)
            {
                var kb = Keyboard.current;
                if (kb != null)
                {
                    if (kb.upArrowKey.wasPressedThisFrame) StepHistory(-1);
                    else if (kb.downArrowKey.wasPressedThisFrame) StepHistory(1);
                    else if (kb.tabKey.wasPressedThisFrame) Complete();
                }
            }

            if (outputDirty && isOpen) RenderOutput();
        }

        private void HandleSubmit(string line)
        {
            Submit(line);
            if (inputField != null)
            {
                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }
        }

        // ---------------------------------------------------------------- Parser

        /// <summary>Jalankan satu baris perintah. Boleh dipanggil dari kode tes.</summary>
        public void Submit(string line)
        {
            // 1.
            if (line == null) return;
            line = line.Trim();
            if (line.Length == 0) return;

            // 2.
            AddHistory(line);
            Print("> " + line);

            // 3-4.
            var tokens = Tokenize(line);
            if (tokens.Length == 0) return;
            string name = tokens[0].ToLowerInvariant();

            // 5.
            if (!commands.TryGetValue(name, out var cmd))
            {
                Print($"Perintah tidak dikenal: {name}. Ketik help.");
                return;
            }

            // 6.
            var args = new string[tokens.Length - 1];
            Array.Copy(tokens, 1, args, 0, args.Length);
            if (args.Length < cmd.minArgs)
            {
                Print("Cara pakai: " + cmd.usage);
                return;
            }

            // 7. Pengecualian dicetak merah dan tidak menghentikan game.
            try
            {
                string result = cmd.run(args);
                if (!string.IsNullOrEmpty(result)) Print(result);
            }
            catch (Exception ex)
            {
                PrintError($"{cmd.name}: {ex.Message}");
            }
        }

        /// <summary>Pisah dengan spasi. Teks dalam tanda kutip dianggap satu token.</summary>
        private string[] Tokenize(string line)
        {
            tokenBuffer.Clear();
            var sb = new StringBuilder();
            bool inQuote = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"') { inQuote = !inQuote; continue; }
                if (c == ' ' && !inQuote)
                {
                    if (sb.Length > 0) { tokenBuffer.Add(sb.ToString()); sb.Clear(); }
                    continue;
                }
                sb.Append(c);
            }
            if (sb.Length > 0) tokenBuffer.Add(sb.ToString());
            return tokenBuffer.ToArray();
        }

        // ---------------------------------------------------------------- Pendaftaran

        /// <summary>Tambah perintah. Nama ganda dicatat sebagai peringatan, dan pendaftar terakhir yang berlaku.</summary>
        public void Register(ConsoleCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.name) || command.run == null) return;
            string key = command.name.ToLowerInvariant();
            if (commands.ContainsKey(key)) Debug.LogWarning($"[DevConsole] Perintah '{key}' didaftarkan ulang.");
            else
            {
                int pos = sortedNames.BinarySearch(key, StringComparer.Ordinal);
                sortedNames.Insert(pos < 0 ? ~pos : pos, key);
            }

            // Perintah yang mengubah kondisi menandai run (#57: run cheat tidak masuk statistik).
            if (command.taints)
            {
                var inner = command.run;
                command.run = args =>
                {
                    string r = inner(args);
                    MarkTainted();
                    return r;
                };
            }
            commands[key] = command;
        }

        /// <summary>Tandai run ini memakai cheat. Disimpan juga di GameManager.wasTainted, supaya run lanjutan tetap ditandai.</summary>
        public void MarkTainted()
        {
            isRunTainted = true;
            if (GameManager.Instance != null) GameManager.Instance.MarkTainted();
        }

        private void Reg(string name, string usage, string help, int minArgs, bool taints, ConsoleHandler run)
        {
            Register(new ConsoleCommand(name, usage, help, minArgs, taints, run));
        }

        private void RegisterBuiltIns()
        {
            Reg("help", "help [nama]", "Daftar perintah atau rincian satu perintah.", 0, false, CmdHelp);
            Reg("clear", "clear", "Mengosongkan keluaran.", 0, false, CmdClear);
            Reg("godmode", "godmode <0/1>", "HP tertahan di 1 dan infeksi di 99.", 1, true, CmdGodmode);
            Reg("additem", "additem <itemId> [jumlah]", "Menambah item. Sisa yang tidak muat jatuh di depan pemain.", 1, true, CmdAddItem);
            Reg("removeitem", "removeitem <itemId> [jumlah]", "Mengurangi item.", 1, true, CmdRemoveItem);
            Reg("breaktool", "breaktool <itemId>", "Mengisi durabilitas sampai Destroyed (item inti: Broken).", 1, true, CmdBreakTool);
            Reg("setday", "setday <n>", "Melompat ke hari tertentu tanpa menjalankan hari yang terlewati.", 1, true, CmdSetDay);
            Reg("settime", "settime <jam>", "Mengatur jam (0-24).", 1, true, CmdSetTime);
            Reg("setvital", "setvital <hp|stamina|hunger|thirst|infection> <nilai>", "Mengatur satu status tubuh.", 2, true, CmdSetVital);
            Reg("kill", "kill [alasan]", "Membunuh pemain dengan DeathReason tertentu (melewati godmode).", 0, true, CmdKill);
            Reg("tp", "tp <x> <y> <z>", "Memindahkan pemain.", 3, true, CmdTeleport);
            Reg("tpregion", "tpregion <region>", "Pindah ke titik uji region.", 1, true, CmdTpRegion);
            Reg("spawn", "spawn <enemyId> [jumlah]", "Memunculkan musuh di depan pemain.", 1, true, CmdSpawn);
            Reg("killall", "killall", "Mengembalikan semua musuh ke pool tanpa statistik.", 0, true, CmdKillAll);
            Reg("timescale", "timescale <nilai>", "Skala waktu gameplay lewat UIFocusStack.", 1, true, CmdTimeScale);
            Reg("balance", "balance <bagian>.<nama> <nilai>", "Mengubah angka di salinan GameBalanceConfig.", 2, true, CmdBalance);
            Reg("eventlog", "eventlog <0/1>", "Mencetak nama event yang terkirim.", 1, false, CmdEventLog);
            Reg("seed", "seed [XXXX-XXXX]", "Tampilkan seed, atau paksa seed run berikutnya.", 0, false, CmdSeed);
            Reg("layout", "layout", "Titik suku cadang, NPC kapal, dan jadwal bencana.", 0, false, CmdLayout);
            Reg("streamstat", "streamstat", "Status pemuatan region (#88).", 0, false, CmdStreamStat);
            Reg("streamdelay", "streamdelay <detik>", "Menunda pemuatan region (#88).", 1, false, CmdStreamDelay);
            Reg("disaster", "disaster <Eruption|Earthquake|TropicalStorm> [skala]", "Memicu bencana.", 1, true, CmdDisaster);
            Reg("unlock", "unlock <region>", "Membuka region.", 1, true, CmdUnlock);
            Reg("save", "save", "Memaksa save.", 0, false, CmdSave);
            Reg("loadlast", "loadlast", "Kembali ke menu, lalu melanjutkan save terakhir.", 0, false, CmdLoadLast);
            Reg("savecorrupt", "savecorrupt", "Mengubah satu byte file save utama (#80).", 0, true, CmdSaveCorrupt);
            Reg("savedelay", "savedelay <detik>", "Menunda penulisan save (#80).", 1, false, CmdSaveDelay);
            Reg("spawnstat", "spawnstat", "Musuh aktif, Submerged, dan kegagalan titik spawn (#62).", 0, false, CmdSpawnStat);
            Reg("settide", "settide <low|high|auto>", "Memaksa level air (#91).", 1, true, CmdSetTide);
            Reg("notify", "notify <jenis> <kunci>", "Memanggil NotificationManager.Show.", 2, false, CmdNotify);
            Reg("postfx", "postfx <efek> <0..1|off>", "Memaksa target efek PostProcessController.", 2, false, CmdPostFx);
            Reg("shake", "shake <kind> [scale]", "Memanggil CameraShake.Play.", 1, false, CmdShake);
            Reg("surfacedebug", "surfacedebug <0|1>", "Kisi permukaan di sekitar pemain (SurfaceDebugOverlay, #86).", 1, false, CmdSurfaceDebug);
            Reg("shiptime", "shiptime <detik>", "Mengatur sisa waktu timer kapal (#56).", 1, true, CmdShipTime);
            Reg("heli", "heli <status|install <partId>|warmup <detik>|liftoff>", "Perintah helikopter (#14).", 1, false, CmdHeli);
            Reg("stats", "stats", "Ringkasan stats.json (#57).", 0, false, CmdStats);
            Reg("statsreset", "statsreset", "Mengosongkan stats.json (#57).", 0, false, CmdStatsReset);
        }

        // ---------------------------------------------------------------- Pembantu argumen

        private static bool TryFloat(string s, out float v) =>
            float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v);

        private static bool TryInt(string s, out int v) =>
            int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v);

        private static bool TryBool(string s, out bool v)
        {
            switch ((s ?? string.Empty).ToLowerInvariant())
            {
                case "1": case "on": case "true": v = true; return true;
                case "0": case "off": case "false": v = false; return true;
                default: v = false; return false;
            }
        }

        private string Usage(string name) => commands.TryGetValue(name, out var c) ? "Cara pakai: " + c.usage : name;

        private static bool InRun()
        {
            var rl = RunLifecycle.Instance;
            return rl != null && (rl.State == RunState.InShip || rl.State == RunState.OnIsland);
        }

        private const string NotInRun = "Ditolak: hanya saat run berjalan (InShip atau OnIsland).";
        private const string OnlyIsland = "Hanya di IslandPhase";

        private static Vector3 InFrontOfPlayer(float meters)
        {
            var fpc = FirstPersonCamera.Instance;
            if (fpc != null && fpc.WorldCamera != null)
            {
                var t = fpc.WorldCamera.transform;
                Vector3 fwd = t.forward;
                fwd.y = 0f;
                if (fwd.sqrMagnitude < 0.0001f) fwd = Vector3.forward;
                return t.position + fwd.normalized * meters;
            }
            var player = IslandPlayerController.Instance;
            return player != null ? player.transform.position + player.transform.forward * meters : Vector3.zero;
        }

        // ---------------------------------------------------------------- Perintah

        private string CmdHelp(string[] args)
        {
            if (args.Length > 0)
            {
                string n = args[0].ToLowerInvariant();
                return commands.TryGetValue(n, out var c) ? $"{c.usage}\n  {c.help}" : $"Perintah tidak dikenal: {n}.";
            }
            var sb = new StringBuilder();
            for (int i = 0; i < sortedNames.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(sortedNames[i]);
            }
            return sb.ToString();
        }

        private string CmdClear(string[] args)
        {
            outputStart = 0;
            outputCount = 0;
            outputDirty = true;
            return null;
        }

        private string CmdGodmode(string[] args)
        {
            if (!TryBool(args[0], out bool on)) return Usage("godmode");
            var stats = SurvivalStats.Instance;
            if (stats == null) return "SurvivalStats tidak ada di scene.";
            // Nilainya tidak ikut save, dan ResetForNewRun mematikannya.
            stats.SetInvulnerable(on);
            godmode = on;
            return "godmode " + (on ? "aktif" : "mati");
        }

        /// <summary>
        /// breaktool (ProgressionSafety.md, #113): durabilitas instance pertama diisi sampai Destroyed. Batas perbaikan dianggap
        /// habis, supaya status bisa Destroyed. Item inti berhenti di Broken (DurabilitySystem.GetState).
        /// </summary>
        private string CmdBreakTool(string[] args)
        {
            string id = ItemDatabase.ResolveAlias(args[0]);
            var inv = InventoryManager.Instance;
            if (inv == null) return "InventoryManager tidak ada di scene.";
            var inst = inv.FindFirstInstance(id);
            if (inst == null) return $"Item tidak ada di inventori: {args[0]}";
            if (inst.durability < 0f) return $"{id} tidak punya durabilitas.";
            var repair = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.repair : null;
            inst.repairCount = Mathf.Max(inst.repairCount, repair != null ? repair.maxRepairs : 6);
            var state = DurabilitySystem.ApplyWear(inst, Mathf.Max(1f, inst.durability));
            return $"breaktool {id}: {state}";
        }

        private string CmdAddItem(string[] args)
        {
            // Usulan 4: alias pendek seperti "wood" menjadi Mat_Wood.
            string id = ItemDatabase.ResolveAlias(args[0]);
            var data = ItemDatabase.Get(id);
            if (data == null) return $"Item tidak dikenal: {args[0]}";
            int amount = 1;
            if (args.Length > 1 && (!TryInt(args[1], out amount) || amount <= 0)) return Usage("additem");
            var inv = InventoryManager.Instance;
            if (inv == null) return "InventoryManager tidak ada di scene.";
            int left = inv.AddItem(data, amount);
            if (left > 0 && WorldItemManager.Instance != null)
                WorldItemManager.Instance.Spawn(inv.CreateInstance(data), left, InFrontOfPlayer(1.5f), true);
            return $"Ditambah {amount - left}, jatuh {left}";
        }

        private string CmdRemoveItem(string[] args)
        {
            string id = ItemDatabase.ResolveAlias(args[0]);
            if (ItemDatabase.Get(id) == null) return $"Item tidak dikenal: {args[0]}";
            int amount = 1;
            if (args.Length > 1 && (!TryInt(args[1], out amount) || amount <= 0)) return Usage("removeitem");
            var inv = InventoryManager.Instance;
            if (inv == null) return "InventoryManager tidak ada di scene.";
            inv.RemoveItem(id, amount);
            return $"Dikurangi {amount} {id}";
        }

        private string CmdSetDay(string[] args)
        {
            if (!InRun()) return NotInRun;
            if (!TryInt(args[0], out int day)) return Usage("setday");
            var gm = GameManager.Instance;
            if (gm == null) return "GameManager tidak ada.";
            // Usulan 2: setday tidak mengejar kargo, bencana, dan pembusukan pada hari yang terlewati.
            return gm.DebugSetDay(day) ? $"Hari = {gm.currentDay}" : "Hari minimal 1.";
        }

        private string CmdSetTime(string[] args)
        {
            if (!TryFloat(args[0], out float hour)) return Usage("settime");
            var dn = DayNightDirector.Instance;
            if (dn == null) return OnlyIsland;
            dn.DebugSetTime(hour);
            return $"Jam = {Mathf.Repeat(hour, 24f).ToString("0.00", CultureInfo.InvariantCulture)}";
        }

        private string CmdSetVital(string[] args)
        {
            if (!InRun()) return NotInRun;
            if (!TryFloat(args[1], out float value)) return Usage("setvital");
            var stats = SurvivalStats.Instance;
            if (stats == null) return "SurvivalStats tidak ada di scene.";
            stats.DebugSetVital(args[0], value);
            return $"{args[0]} = {value.ToString(CultureInfo.InvariantCulture)}";
        }

        private string CmdKill(string[] args)
        {
            if (!InRun()) return NotInRun;
            var reason = DeathReason.KilledByEnemy;
            if (args.Length > 0 && (!Enum.TryParse(args[0], true, out reason) || reason == DeathReason.None))
                return "Alasan tidak dikenal. Contoh: Frostbite, Drowned, Starvation.";
            var stats = SurvivalStats.Instance;
            if (stats == null) return "SurvivalStats tidak ada di scene.";
            // DebugKill melewati IsInvulnerable, supaya uji alur mati tetap bisa dilakukan saat godmode.
            stats.DebugKill(reason);
            return "Pemain dibunuh: " + reason;
        }

        private string CmdTeleport(string[] args)
        {
            if (!TryFloat(args[0], out float x) || !TryFloat(args[1], out float y) || !TryFloat(args[2], out float z)) return Usage("tp");
            var player = IslandPlayerController.Instance;
            if (player == null) return OnlyIsland;
            // Teleport sudah memanggil PlayerTraversal.ResetFallTracking dan ZoneManager.ResyncAll.
            player.Teleport(new Vector3(x, y, z));
            return $"Pindah ke {x}, {y}, {z}";
        }

        private string CmdTpRegion(string[] args)
        {
            // TODO(RegionStreaming): titik uji per region dan pemuatan region (#88) belum tersedia.
            return "TODO: tpregion menunggu RegionStreaming (#88).";
        }

        private string CmdSpawn(string[] args)
        {
            int count = 1;
            if (args.Length > 1 && (!TryInt(args[1], out count) || count <= 0)) return Usage("spawn");
            var sm = SpawnManager.Instance;
            if (sm == null) return OnlyIsland;
            sm.DebugSpawn(args[0], count);
            return $"Spawn {count}x {args[0]}";
        }

        private string CmdKillAll(string[] args)
        {
            var sm = SpawnManager.Instance;
            if (sm == null) return OnlyIsland;
            // DebugReturnAll tidak mengirim EnemyKilledEvent, jadi tidak menambah statistik (#57).
            sm.DebugReturnAll();
            return "Semua musuh dikembalikan ke pool.";
        }

        private string CmdTimeScale(string[] args)
        {
            if (!TryFloat(args[0], out float v) || v < 0f) return Usage("timescale");
            var stack = UIFocusStack.Instance;
            if (stack == null) return "UIFocusStack tidak ada.";
            // Tidak menulis Time.timeScale langsung (#83). Saat panel menjeda, nilai berlaku setelah panel tertutup.
            stack.SetGameplayTimeScale(v);
            return "timescale = " + v.ToString(CultureInfo.InvariantCulture);
        }

        private string CmdBalance(string[] args)
        {
            // Reflection hanya dipakai perintah ini. Yang diubah adalah salinan per run, bukan aset asli.
            var cfg = GameManager.CurrentBalance;
            if (cfg == null) return "GameBalanceConfig belum ada.";
            string path = args[0];
            int dot = path.IndexOf('.');
            if (dot <= 0 || dot >= path.Length - 1) return Usage("balance");
            string sectionName = path.Substring(0, dot);
            string fieldName = path.Substring(dot + 1);

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            var sectionField = typeof(GameBalanceConfig).GetField(sectionName, flags);
            if (sectionField == null) return $"Bagian tidak dikenal: {sectionName}. Mirip: {Similar(typeof(GameBalanceConfig), sectionName)}";
            object section = sectionField.GetValue(cfg);
            if (section == null) return $"Bagian {sectionName} kosong.";
            var field = section.GetType().GetField(fieldName, flags);
            if (field == null) return $"Nama tidak dikenal: {fieldName}. Mirip: {Similar(section.GetType(), fieldName)}";

            object oldValue = field.GetValue(section);
            object newValue;
            if (field.FieldType == typeof(float) && TryFloat(args[1], out float f)) newValue = f;
            else if (field.FieldType == typeof(int) && TryInt(args[1], out int i)) newValue = i;
            else if (field.FieldType == typeof(bool) && TryBool(args[1], out bool bv)) newValue = bv;
            else return $"Tipe {field.FieldType.Name} tidak didukung, atau nilai salah. Cara pakai: balance <bagian>.<nama> <nilai>";

            field.SetValue(section, newValue);
            return $"{path}: {Convert.ToString(oldValue, CultureInfo.InvariantCulture)} -> {Convert.ToString(newValue, CultureInfo.InvariantCulture)}";
        }

        private static string Similar(Type type, string query)
        {
            var sb = new StringBuilder();
            string q = query.ToLowerInvariant();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                string n = fields[i].Name;
                string lower = n.ToLowerInvariant();
                if (q.Length == 0 || lower.Contains(q) || q.Contains(lower) || lower[0] == q[0])
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.Append(n);
                }
            }
            return sb.Length > 0 ? sb.ToString() : "-";
        }

        private string CmdEventLog(string[] args)
        {
            if (!TryBool(args[0], out bool on)) return Usage("eventlog");
            SetEventLog(on);
            return "eventlog " + (on ? "aktif" : "mati");
        }

        private void SetEventLog(bool on)
        {
            eventLogOn = on;
            // Fitur ini mengalokasi memori, dan hanya berjalan saat diaktifkan.
            if (on) GameEventRegistry.LogHook = AppendEventLine;
            else if (GameEventRegistry.LogHook == (Action<string>)AppendEventLine) GameEventRegistry.LogHook = null;
        }

        private void AppendEventLine(string channelName)
        {
            if (eventLogOn) Print("[event] " + channelName);
        }

        private string CmdSeed(string[] args)
        {
            var seeds = RunSeedManager.Instance;
            if (seeds == null) return "RunSeedManager tidak ada.";
            if (args.Length == 0)
                return string.IsNullOrEmpty(seeds.CurrentSeedText) ? "Belum ada seed." : "Seed: " + seeds.CurrentSeedText;

            string hex = args[0].Replace("-", string.Empty);
            if (hex.Length != 8 || !uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint seed) || seed == 0)
                return "Format seed: XXXX-XXXX (heksadesimal, bukan 0).";
            seeds.DebugForceNextSeed(seed);
            // Mengisi seed run berikutnya tidak mengubah run ini, jadi tidak menandai [CHEAT].
            return $"Run berikutnya memakai seed {RunSeedManager.FormatSeed(seed)}.";
        }

        private string CmdLayout(string[] args)
        {
            var seeds = RunSeedManager.Instance;
            var layout = seeds != null ? seeds.CurrentLayout : null;
            if (layout == null) return "Belum ada layout.";
            var sb = new StringBuilder();
            sb.Append("Seed ").Append(seeds.CurrentSeedText);
            if (seeds.UsedFallbackLayout) sb.Append(" (layout cadangan)");
            sb.Append("\nSuku cadang -> titik: ");
            for (int i = 0; i < layout.partPointByPart.Length; i++) sb.Append(i).Append("->").Append(layout.partPointByPart[i]).Append(' ');
            sb.Append("\nNPC kapal: ");
            for (int i = 0; i < layout.shipNpcIds.Length; i++)
            {
                sb.Append(layout.shipNpcIds[i]);
                if (i < layout.shipNpcPointByNpc.Length) sb.Append('@').Append(layout.shipNpcPointByNpc[i]);
                sb.Append(' ');
            }
            sb.Append("\nBencana (s.d. hari ").Append(layout.disasterScheduleThroughDay).Append("): ");
            for (int i = 0; i < layout.disasterSchedule.Length; i++)
            {
                var d = layout.disasterSchedule[i];
                sb.Append("H").Append(d.day).Append(' ').Append(d.type).Append('x').Append(d.scale).Append("; ");
            }
            return sb.ToString();
        }

        private string CmdStreamStat(string[] args)
        {
            var rs = RegionStreaming.Instance;
            if (rs == null) return OnlyIsland;
            var sb = new StringBuilder();
            sb.Append("Ready ").Append(rs.IsReady).Append(", Stalled ").Append(rs.IsStalled)
              .Append(", delay ").Append(rs.DebugExtraDelaySeconds.ToString("0.0", CultureInfo.InvariantCulture)).Append(" s\n");
            var regions = (RegionType[])Enum.GetValues(typeof(RegionType));
            // Satu baris per region: state, waktu muat terakhir, dan jumlah objek tersimpan (RegionStreaming.md, #88).
            for (int i = 0; i < regions.Length; i++) sb.Append('\n').Append(rs.DebugStatLine(regions[i]));
            return sb.ToString();
        }

        private string CmdStreamDelay(string[] args)
        {
            if (!TryFloat(args[0], out float seconds) || seconds < 0f) return Usage("streamdelay");
            var rs = RegionStreaming.Instance;
            if (rs == null) return OnlyIsland;
            rs.DebugExtraDelaySeconds = seconds;
            return "Penundaan pemuatan region = " + seconds.ToString(CultureInfo.InvariantCulture) + " s";
        }

        private string CmdDisaster(string[] args)
        {
            if (!Enum.TryParse(args[0], true, out DisasterType type)) return Usage("disaster");
            int scale = 1;
            if (args.Length > 1 && !TryInt(args[1], out scale)) return Usage("disaster");
            var dm = DisasterManager.Instance;
            if (dm == null) return OnlyIsland;
            return dm.ForceStart(type, scale) ? $"Bencana {type} skala {scale} dimulai." : "Ditolak: ada bencana aktif.";
        }

        private string CmdUnlock(string[] args)
        {
            if (!Enum.TryParse(args[0], true, out RegionType region)) return Usage("unlock");
            var gate = RegionGateSystem.Instance;
            if (gate == null) return OnlyIsland;
            gate.Unlock(region);
            return "Region dibuka: " + region;
        }

        private string CmdSave(string[] args)
        {
            EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.Manual));
            return "SaveRequestedEvent (Manual) dikirim.";
        }

        private string CmdLoadLast(string[] args)
        {
            var rl = RunLifecycle.Instance;
            if (rl == null) return "RunLifecycle tidak ada.";
            if (rl.State != RunState.OnIsland && rl.State != RunState.InShip && rl.State != RunState.AwaitingChoice && rl.State != RunState.InMenu)
                return "Ditolak: alur run sedang berpindah.";
            StartCoroutine(LoadLastRoutine());
            return "Kembali ke menu, lalu melanjutkan save terakhir.";
        }

        private IEnumerator LoadLastRoutine()
        {
            var rl = RunLifecycle.Instance;
            if (rl == null) yield break;
            if (rl.State != RunState.InMenu) rl.ReturnToMainMenu();
            float start = Time.realtimeSinceStartup;
            while (rl != null && rl.State != RunState.InMenu)
            {
                if (Time.realtimeSinceStartup - start > 60f) { PrintError("loadlast: menu tidak tercapai."); yield break; }
                yield return null;
            }
            if (rl != null) rl.ContinueRun();
        }

        private string CmdSaveCorrupt(string[] args)
        {
            // Satu byte file save utama diubah lewat WorldStateSave.DebugCorruptMainFile (#80, skenario 9).
            var save = WorldStateSave.Instance;
            if (save == null) return "WorldStateSave tidak ada.";
            save.DebugCorruptMainFile(out string message);
            return "savecorrupt: " + message;
        }

        private string CmdSaveDelay(string[] args)
        {
            // Penundaan buatan sebelum file utama diganti, hanya untuk save di thread lain (#80, skenario 8, 24, 25, 27).
            var save = WorldStateSave.Instance;
            if (save == null) return "WorldStateSave tidak ada.";
            if (!TryFloat(args[0], out float seconds) || seconds < 0f) return Usage("savedelay");
            save.DebugSetWriteDelay(seconds);
            return "savedelay = " + save.DebugWriteDelaySeconds.ToString("0.0", CultureInfo.InvariantCulture) + " s";
        }

        private string CmdSpawnStat(string[] args)
        {
            // Musuh aktif, jumlah Submerged, dan kegagalan mencari titik spawn (SpawnManager.md, #62).
            var sm = SpawnManager.Instance;
            if (sm == null) return OnlyIsland;
            var enemies = sm.ActiveEnemies;
            int submerged = 0;
            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i] != null && enemies[i].AI != null && enemies[i].AI.CurrentState == AIState.Submerged) submerged++;
            return $"Musuh aktif {enemies.Count}, Submerged {submerged}, titik spawn gagal {sm.FailedSpawnPoints}.";
        }

        private string CmdSetTide(string[] args)
        {
            // Memaksa level air lewat TideManager.DebugSetSurge (#91). high: Level selalu >= highThreshold;
            // low: Level selalu <= lowThreshold; auto: surge kembali mengikuti cuaca.
            var tide = TideManager.Instance;
            if (tide == null) return OnlyIsland;
            var t = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.tide : null;
            if (t == null) return "GameBalanceConfig belum ada.";
            switch (args[0].ToLowerInvariant())
            {
                case "high": tide.DebugSetSurge(t.highThreshold, false); break;
                case "low": tide.DebugSetSurge(t.lowThreshold - Mathf.Max(t.dayAmplitude, t.nightAmplitude), false); break;
                case "auto": tide.DebugSetSurge(0f, true); break;
                default: return Usage("settide");
            }
            return "settide " + args[0].ToLowerInvariant();
        }

        private string CmdNotify(string[] args)
        {
            if (!Enum.TryParse(args[0], true, out NotificationKind kind)) return "Jenis: Info, Gain, Warning, Danger, Objective.";
            var nm = NotificationManager.Instance;
            if (nm == null) return "NotificationManager tidak ada.";
            nm.Show(args[1], kind);
            return null;
        }

        private string CmdPostFx(string[] args)
        {
            // Memaksa target efek lewat PostProcessController.DebugForceTarget (#23). Nilai negatif atau "off" melepasnya.
            var pp = PostProcessController.Instance;
            if (pp == null) return "PostProcessController tidak ada di scene ini.";
            string effect = args[0].ToLowerInvariant();
            float value;
            if (args[1].ToLowerInvariant() == "off") value = -1f;
            else if (!TryFloat(args[1], out value)) return Usage("postfx");
            if (!pp.DebugForceTarget(effect, value))
                return "Efek: " + string.Join(", ", PostProcessController.DebugEffectNames) + ".";
            return value < 0f ? "postfx " + effect + " dilepas" : "postfx " + effect + " = " + Mathf.Clamp01(value).ToString("0.00", CultureInfo.InvariantCulture);
        }

        private string CmdSurfaceDebug(string[] args)
        {
            // SurfaceDetection.md "Tampilan debug": overlay dipasang sekali di objek konsol (persisten), lalu dinyalakan/dimatikan.
            if (!TryBool(args[0], out bool on)) return Usage("surfacedebug");
            var overlay = GetComponent<SurfaceDebugOverlay>();
            if (overlay == null && on) overlay = gameObject.AddComponent<SurfaceDebugOverlay>();
            if (overlay != null) overlay.enabled = on;
            return "surfacedebug " + (on ? "1" : "0");
        }

        private string CmdShake(string[] args)
        {
            if (!Enum.TryParse(args[0], true, out CameraShakeKind kind)) return Usage("shake");
            float scale = 1f;
            if (args.Length > 1 && !TryFloat(args[1], out scale)) return Usage("shake");
            CameraShake.Play(kind, scale);
            return null;
        }

        private string CmdShipTime(string[] args)
        {
            if (!TryFloat(args[0], out float seconds) || seconds < 0f) return Usage("shiptime");
            var timer = ShipTimerManager.Instance;
            if (timer == null) return "Hanya di ShipPhase";
            timer.DebugSetSecondsLeft(seconds);
            return "Sisa waktu kapal = " + timer.SecondsLeft.ToString("0.0", CultureInfo.InvariantCulture) + " s";
        }

        private string CmdHeli(string[] args)
        {
            var rescue = RescueEventManager.Instance;
            if (rescue == null) return OnlyIsland;
            switch (args[0].ToLowerInvariant())
            {
                case "status":
                    return $"State {rescue.State}, terpasang {rescue.InstalledCount}, pemanasan {rescue.WarmupLeft.ToString("0.0", CultureInfo.InvariantCulture)} s";
                case "install":
                    if (args.Length < 2) return "Cara pakai: heli install <partId>";
                    // "heli install all": daftar partId resmi dibaca dari HeavyItemSystem (data world_parts, #69).
                    if (args[1].ToLowerInvariant() == "all")
                    {
                        var heavy = HeavyItemSystem.Instance;
                        if (heavy == null) return "HeavyItemSystem tidak ada di scene ini.";
                        for (int i = 0; i < heavy.PartCount; i++)
                        {
                            var r = heavy.GetRecord(i);
                            if (r != null && !string.IsNullOrEmpty(r.partId)) rescue.InstallPart(r.partId);
                        }
                        MarkTainted();
                        return $"InstallPart semua ({heavy.PartCount}). Terpasang {rescue.InstalledCount}.";
                    }
                    rescue.InstallPart(args[1]);
                    MarkTainted();
                    return $"InstallPart({args[1]}). Terpasang {rescue.InstalledCount}.";
                case "warmup":
                    if (args.Length < 2 || !TryFloat(args[1], out float s)) return "Cara pakai: heli warmup <detik>";
                    rescue.DebugSetWarmup(s);
                    MarkTainted();
                    return "warmupLeft = " + s.ToString(CultureInfo.InvariantCulture);
                case "liftoff":
                    MarkTainted();
                    return rescue.LiftOff(true) ? "Lepas landas." : "Ditolak: run sudah berakhir.";
                default:
                    return Usage("heli");
            }
        }

        private string CmdStats(string[] args)
        {
            var ls = LocalStatsManager.Instance;
            if (ls == null) return "LocalStatsManager tidak ada.";
            var d = ls.Data;
            var r = ls.CurrentRun;
            return $"Run {d.totalRuns}, menang {d.totalWins}, hari terlama {d.longestDaySurvived}, zombie {d.totalZombiesKilled}, " +
                   $"akurasi {(ls.GetAccuracy() * 100f).ToString("0", CultureInfo.InvariantCulture)}%, kematian terbanyak {ls.GetTopDeathReason()?.ToString() ?? "-"}\n" +
                   $"Run ini: zombie {r.zombiesKilled}, hewan {r.animalsKilled}, tembak {r.shotsFired}, meleset {r.shotsMissed}, " +
                   $"headshot {r.headshots}, tainted {r.isTainted || IsRunTainted}";
        }

        private string CmdStatsReset(string[] args)
        {
            var ls = LocalStatsManager.Instance;
            if (ls == null) return "LocalStatsManager tidak ada.";
            ls.ResetAllStats();
            return "stats.json dikosongkan.";
        }

        // ---------------------------------------------------------------- Riwayat dan pelengkapan

        private void AddHistory(string line)
        {
            if (history.Count == 0 || history[history.Count - 1] != line) history.Add(line);
            var b = B;
            int max = b != null ? Mathf.Max(1, b.maxHistory) : 50;
            while (history.Count > max) history.RemoveAt(0);
            historyIndex = -1;
            PlayerPrefs.SetString(HistoryPrefsKey, string.Join("\n", history));
        }

        private void StepHistory(int dir)
        {
            if (history.Count == 0 || inputField == null) return;
            if (historyIndex < 0) historyIndex = dir < 0 ? history.Count - 1 : -1;
            else
            {
                historyIndex += dir;
                if (historyIndex >= history.Count) historyIndex = -1;
                else if (historyIndex < 0) historyIndex = 0;
            }
            inputField.text = historyIndex >= 0 ? history[historyIndex] : string.Empty;
            inputField.caretPosition = inputField.text.Length;
        }

        /// <summary>Tab: lengkapi nama perintah, atau ID item setelah additem/removeitem.</summary>
        private void Complete()
        {
            if (inputField == null) return;
            string text = inputField.text ?? string.Empty;
            int space = text.IndexOf(' ');
            var matches = new List<string>();
            string prefix;
            string head;

            if (space < 0)
            {
                prefix = text.ToLowerInvariant();
                head = string.Empty;
                for (int i = 0; i < sortedNames.Count; i++)
                    if (sortedNames[i].StartsWith(prefix, StringComparison.Ordinal)) matches.Add(sortedNames[i]);
            }
            else
            {
                string cmd = text.Substring(0, space).ToLowerInvariant();
                if (cmd != "additem" && cmd != "removeitem") return;
                string rest = text.Substring(space + 1);
                if (rest.IndexOf(' ') >= 0) return;
                prefix = rest;
                head = text.Substring(0, space + 1);
                foreach (var id in ItemDatabase.AllIds)
                    if (id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) matches.Add(id);
                matches.Sort(StringComparer.Ordinal);
            }

            if (matches.Count == 0) return;
            if (matches.Count == 1)
            {
                inputField.text = head + matches[0] + " ";
            }
            else
            {
                Print(string.Join("  ", matches));
                inputField.text = head + CommonPrefix(matches);
            }
            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
        }

        private static string CommonPrefix(List<string> items)
        {
            string first = items[0];
            int len = first.Length;
            for (int i = 1; i < items.Count; i++)
            {
                int k = 0;
                while (k < len && k < items[i].Length && char.ToLowerInvariant(items[i][k]) == char.ToLowerInvariant(first[k])) k++;
                len = k;
            }
            return first.Substring(0, len);
        }

        // ---------------------------------------------------------------- Keluaran

        public void Print(string line)
        {
            if (outputLines.Length == 0) return;
            int index = (outputStart + outputCount) % outputLines.Length;
            outputLines[index] = line;
            if (outputCount < outputLines.Length) outputCount++;
            else outputStart = (outputStart + 1) % outputLines.Length;
            outputDirty = true;
        }

        private void PrintError(string line) => Print("<color=#FF5555>" + line + "</color>");

        private void RenderOutput()
        {
            outputDirty = false;
            if (outputText == null) return;
            builder.Clear();
            for (int i = 0; i < outputCount; i++)
            {
                if (i > 0) builder.Append('\n');
                builder.Append(outputLines[(outputStart + i) % outputLines.Length]);
            }
            outputText.SetText(builder);
        }
    }
}
#endif
