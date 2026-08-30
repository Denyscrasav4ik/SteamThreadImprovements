using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.Stickers;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_BetterStickerStack)]
[HarmonyPatch(typeof(StickerScreenController))]
internal static class BetterStickerStack
{
    private static InventorySticker? visualClone = null;
    private static int lastHeldId = -1;

    [HarmonyPatch("UpdateStickerInventoryPositions")]
    [HarmonyPostfix]
    static void Postfix(
        StickerScreenController __instance,
        List<InventorySticker> ___inventoryStickers,
        bool ___holdingSticker,
        int ___heldStickerInstantiationId,
        int ___heldStickerInventoryId,
        Transform ___inventoryStickersTransform,
        InventorySticker ___inventoryStickerPrefab)
    {
        if (___holdingSticker)
        {
            InventorySticker heldSticker = ___inventoryStickers[___heldStickerInstantiationId];

            int totalInInv = Singleton<StickerManager>.Instance.TotalInInventory(heldSticker.Sticker);

            if (totalInInv > 1)
            {
                if (visualClone == null)
                {
                    visualClone = Object.Instantiate(___inventoryStickerPrefab, ___inventoryStickersTransform);

                    CanvasGroup group = visualClone.gameObject.AddComponent<CanvasGroup>();
                    group.blocksRaycasts = false;
                }

                if (!visualClone.gameObject.activeSelf || lastHeldId != ___heldStickerInstantiationId)
                {
                    visualClone.gameObject.SetActive(true);
                    visualClone.Initialize(__instance, ___heldStickerInstantiationId, ___heldStickerInventoryId);
                    lastHeldId = ___heldStickerInstantiationId;
                }

                float num = Mathf.RoundToInt(264f / (float)___inventoryStickers.Count);
                Vector3 listPosition = new Vector3((___heldStickerInstantiationId % 2 != 0) ? 96 : 0, (0f - num) * (float)___heldStickerInstantiationId - num / 2f, 0f);

                visualClone.SetPosition(listPosition);
                visualClone.SetHotspotHeight(num * 2f);

                visualClone.SetValue(totalInInv - 1);
                heldSticker.SetValue(1);
            }
            else
            {
                if (visualClone != null && visualClone.gameObject.activeSelf)
                    visualClone.gameObject.SetActive(false);
            }
        }
        else
        {
            if (visualClone != null && visualClone.gameObject.activeSelf)
            {
                visualClone.gameObject.SetActive(false);
                lastHeldId = -1;
            }
        }
    }
}
