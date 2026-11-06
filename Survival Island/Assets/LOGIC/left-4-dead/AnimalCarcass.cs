using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Bangkai hewan yang bisa dikuliti dengan Pisau. Blueprint: left-4-dead/WildlifeAI.md bagian "Bangkai dan menguliti" (#61).
    /// GDD bagian 3: berburu hewan untuk makanan. Collider bangkai (anak objek, layer Interactable) dimatikan sampai Activate.
    /// Daging Food_RawMeat dan kulit Mat_Hide diisi di AnimalData.harvest.
    /// </summary>
    public class AnimalCarcass : MonoBehaviour, IInteractable, IPoolable
    {
        [SerializeField] private Collider interactCollider;
        [SerializeField] private Renderer[] renderers;

        private AnimalData data;
        private bool active;
        private bool harvested;
        private Coroutine lifetime;

        private void Awake()
        {
            var h = GetComponent<EnemyHealth>();
            data = h != null ? h.Data as AnimalData : null;
            if (renderers == null || renderers.Length == 0) renderers = GetComponentsInChildren<Renderer>(true);
            if (interactCollider != null) interactCollider.enabled = false;
        }

        /// <summary>Dipanggil EnemyHitReaction setelah corpseHoldSeconds.</summary>
        public void Activate()
        {
            active = true;
            harvested = false;
            if (interactCollider != null) interactCollider.enabled = true;
            if (lifetime != null) StopCoroutine(lifetime);
            lifetime = StartCoroutine(LifetimeRoutine());
        }

        private IEnumerator LifetimeRoutine()
        {
            var bal = GameManager.CurrentBalance;
            yield return new WaitForSeconds(bal != null ? bal.wildlife.carcassLifetimeSeconds : 180f);
            // Umur habis: pudar lalu kembali ke pool (Killed).
            float fade = bal != null ? bal.hitReaction.corpseFadeSeconds : 1.5f;
            var block = new MaterialPropertyBlock();
            for (float f = 0f; f < fade; f += Time.deltaTime)
            {
                foreach (var r in renderers)
                {
                    if (r == null) continue;
                    r.GetPropertyBlock(block);
                    block.SetFloat("_Fade", 1f - f / fade);
                    r.SetPropertyBlock(block);
                }
                yield return null;
            }
            lifetime = null;
            Despawn(DespawnReason.Killed);
        }

        public bool IsAvailable => active && !harvested && data != null && data.harvest != null && data.harvest.Length > 0;
        public float HoldSeconds => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.wildlife.skinHoldSeconds : 3f;
        public string GetPrompt() { var loc = LocalizationManager.Instance; return loc != null && loc.Has("prompt.skin") ? loc.Get("prompt.skin") : "Kuliti"; }   // #104

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            string held = EventChannel<EquippedItemChangedEvent>.HasLast ? EventChannel<EquippedItemChangedEvent>.Last.itemId : null;
            if (held != PlayerPinnedState.KnifeItemId) return InteractBlockReason.MissingTool;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter() { }
        public void OnHoverExit() { }

        public bool Interact(in InteractContext ctx)
        {
            if (!IsAvailable || CanInteract(ctx) != InteractBlockReason.None) return false;
            harvested = true;
            // 1. Jumlah dari aliran Gameplay (#89).
            for (int i = 0; i < data.harvest.Length; i++)
            {
                var h = data.harvest[i];
                if (h.item == null) continue;
                int amount = h.max > h.min && RunSeedManager.Instance != null
                    ? RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay).Range(h.min, h.max + 1) : h.min;
                if (amount <= 0) continue;
                // 2. Sisa yang tidak muat dijatuhkan sebagai WorldItem dari pool (#66).
                int rest = ctx.Inventory != null ? ctx.Inventory.AddItem(h.item, amount) : amount;
                if (rest > 0 && WorldItemManager.Instance != null && ctx.Inventory != null)
                    WorldItemManager.Instance.Spawn(ctx.Inventory.CreateInstance(h.item), rest, transform.position + Vector3.up * 0.5f, true);
            }
            // 3. Suara alat: hewan lain di teritori bisa mendengar.
            var bal = GameManager.CurrentBalance;
            NoiseEmitter.Emit(transform.position, bal != null ? bal.wildlife.skinNoiseRadius : 6f, NoiseSourceKind.Tool);
            // 4.
            Despawn(DespawnReason.Harvested);
            return true;
        }

        private void Despawn(DespawnReason reason)
        {
            active = false;
            if (interactCollider != null) interactCollider.enabled = false;
            if (SpawnManager.Instance != null) SpawnManager.Instance.Despawn(gameObject, reason);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        public void OnSpawned()
        {
            active = false;
            harvested = false;
            if (interactCollider != null) interactCollider.enabled = false;
            foreach (var r in renderers) if (r != null) r.SetPropertyBlock(null);
        }

        public void OnReturned()
        {
            if (lifetime != null) { StopCoroutine(lifetime); lifetime = null; }
            active = false;
        }
    }
}
