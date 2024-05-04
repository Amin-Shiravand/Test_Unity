using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class GameResourceManager : MonoBehaviorSingleton<GameResourceManager>
    {
        public uint CountOfAssetLoaded
        {
            get;
            private set;
        }

        public int TotalCarsSpriteCount
        {
            get;
            private set;
        }

        private Dictionary<int, Object> assetsHashTable = new Dictionary<int, Object>();
        // private Dictionary<int, Object[]> assetPackMap = new Dictionary<int, Object[]>();

        public Texture LoadTexture(string path)
        {
            return GetFormAssetHashTable<Texture>("Textures/" + path) as Texture;
        }

        public Material LoadMaterial(string path)
        {
            return GetFormAssetHashTable<Material>("Materials/" + path) as Material;
        }

        public GameObject LoadPrefab(string path)
        {
            return GetFormAssetHashTable<GameObject>("Prefabs/" + path);
        }

        public Font LoadFont(string path)
        {
            return GetFormAssetHashTable<Font>("Fonts/" + path) as Font;
        }

        public AudioClip LoadAudioClip(string path)
        {
            return GetFormAssetHashTable<AudioClip>("Audios/" + path) as AudioClip;
        }

        public Sprite LoadSprite(string path)
        {
            return GetFormAssetHashTable<Sprite>(path);
        }

        public Sprite LoadCardSprite(string path)
        {
            return GetFormAssetHashTable<Sprite>("Sprites/Cards/" + path);
        }

        public Sprite[] LoadAllCards()
        {
            List<Sprite> avatars = new List<Sprite>();

            for (int i = 0; i < TotalCarsSpriteCount; ++i)
            {
                Sprite sp = GetFormAssetHashTable<Sprite>("Sprites/Cards/" + i.ToString());
                if (sp != null)
                    avatars.Add(sp);
            }

            return avatars.ToArray();
        }

        //public Sprite[] LoadSpritePack(string Path)
        //{

        //    Object[] obj = GetPackAssetsFromMap(Path);
        //    Sprite[] sprites = ;
        //    return (Sprite[]);
        //}

        public void UnloadPrefab(string path)
        {
            UnloadAsset("Prefabs/" + path);
        }

        public void UnloadMaterial(string path)
        {
            UnloadAsset("Materials/" + path);
        }

        public void UnloadTexture(string path)
        {
            UnloadAsset("Textures/" + path);
        }

        public void UnloadFont(string path)
        {
            UnloadAsset("Fonts/" + path);
        }

        public void UnloadAudioClip(string path)
        {
            UnloadAsset("Audios/" + path);
        }

        public void UnloadFontSprite(string path)
        {
            UnloadAsset("Sprites/" + path);
        }

        public bool UnloadAllAssets()
        {

            var iterator = assetsHashTable.GetEnumerator();

            while (iterator.MoveNext())
            {
                Object current = iterator.Current.Value;

                if (current.GetType() != typeof(GameObject))
                    Resources.UnloadAsset(current);

                if (CountOfAssetLoaded > 0)
                    --CountOfAssetLoaded;
                current = null;
            }
            assetsHashTable.Clear();
            Resources.UnloadUnusedAssets();
            return CountOfAssetLoaded == 0 ? true : false;
        }

        private bool UnloadAsset(string path)
        {
            bool isUnloaded = false;
            int hashCode = path.GetHashCode();
            if (assetsHashTable.ContainsKey(hashCode))
            {
                Object unloadObject = assetsHashTable[hashCode];
                if (unloadObject.GetType() != typeof(GameObject))
                    Resources.UnloadAsset(unloadObject);
                assetsHashTable.Remove(hashCode);
                isUnloaded = true;
                if (CountOfAssetLoaded > 0)
                    --CountOfAssetLoaded;
            }
            return isUnloaded;
        }

     

        private T GetFormAssetHashTable<T>(string path) where T : Object
        {
            System.Type type = typeof(T);
            Object loadedObject = null;

            int hashCode = path.GetHashCode();
            if (assetsHashTable.ContainsKey(hashCode))
                loadedObject = assetsHashTable[hashCode];
            else
                assetsHashTable.Add(hashCode, loadedObject);

            if (loadedObject == null)
            {
                loadedObject = Resources.Load(path, type);


                if (loadedObject == null)
                {
                    assetsHashTable.Remove(hashCode);
                    return null;
                }
                assetsHashTable[hashCode] = loadedObject;
            }

            ++CountOfAssetLoaded;
            return (T)loadedObject;
        }

        private void Awake()
        {
            SetCardsCount();
        }

        private void SetCardsCount()
        {
            UnityEngine.Object[] avatars = Resources.LoadAll("Sprites/Cards");
            TotalCarsSpriteCount = avatars.Length;
            for (int i = 0; i < avatars.Length; ++i)
                Resources.UnloadAsset(avatars[i]);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}