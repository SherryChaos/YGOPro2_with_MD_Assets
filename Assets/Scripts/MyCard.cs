using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class MyCard
    {
        private static Dictionary<string, Texture2D> cachedAvatars = new Dictionary<string, Texture2D>();

        public static void LoadAvatar(string username, Action<Texture2D> callback)
        {
            if (cachedAvatars.ContainsKey(username))
            {
                var avatar = cachedAvatars[username];
                if (avatar != null)
                {
                    callback(cachedAvatars[username]);
                }
                return;
            }
            cachedAvatars.Add(username, null);

            var request =
                UnityWebRequestTexture.GetTexture($"https://sapi.moecube.com:444/avatar/avatar/{username}/100/ygopro2.png");
            var operation = request.SendWebRequest();
            operation.completed += _ =>
            {
                if (request.error != null)
                {
                    Debug.LogWarning($"{request.url}: {request.error}");
                    return;
                }

                var avatar = ((DownloadHandlerTexture)request.downloadHandler).texture;
                cachedAvatars[username] = avatar;
                callback(avatar);
            };
        }
    }
}