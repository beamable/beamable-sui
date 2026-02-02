using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Scripts.Helpers
{
    public static class Utilities
    {
        public static async UniTask<Sprite> GetSpriteAsync(AssetReferenceSprite spriteReference)
        {
            if (spriteReference == null || !spriteReference.RuntimeKeyIsValid())
            {
                Debug.LogError("Sprite reference is null or invalid sprite reference.");
                return null;
            }

            try
            {
                // Ensure the asset is not already loaded
                if (!spriteReference.OperationHandle.IsValid() || spriteReference.OperationHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    var spriteHandle = spriteReference.LoadAssetAsync();
                    await spriteHandle;

                    if (spriteHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        return spriteHandle.Result;
                    }
                    else
                    {
                        Debug.LogError($"Failed to load sprite: {spriteHandle.OperationException}");
                        return null;
                    }
                }
                else
                {
                    return spriteReference.OperationHandle.Result as Sprite;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while loading sprite: {e.Message}");
                return null;
            }
        }
        
        public static async UniTask<Sprite> DownloadImageFromUrl(string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download image from URL: {request.error}");
                return null;
            }
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}