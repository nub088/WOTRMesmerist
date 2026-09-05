using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Mesmerist.Utils
{
    /// <summary>
    /// Loads the mod's own icon art from Assets/Icons and hands back UnityEngine.Sprites.
    ///
    /// BlueprintCore's SetIcon takes an Asset&lt;Sprite&gt;, which converts implicitly from a
    /// Sprite, so a loaded sprite drops straight into any configurator. Sprites are cached by
    /// name: blueprints are built once during BlueprintsCache.Init, but a trick's feature,
    /// ability and buff all share one icon, so each file would otherwise be decoded 3 times.
    /// </summary>
    public static class IconLoader
    {
        private static readonly Dictionary<string, Sprite> Cache = new();
        private static string _iconDirectory;

        /// <summary>Assets/Icons next to the loaded assembly.</summary>
        private static string IconDirectory
        {
            get
            {
                if (_iconDirectory is null)
                {
                    var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    _iconDirectory = Path.Combine(assemblyDir, Path.Combine("Assets", "Icons"));
                }
                return _iconDirectory;
            }
        }

        /// <summary>
        /// Returns the sprite for "<paramref name="name"/>.png", or null if it is missing or
        /// unreadable. Callers fall back to a stock game icon on null, so a missing art file
        /// degrades to the old borrowed icon instead of breaking blueprint creation.
        /// </summary>
        public static Sprite Get(string name)
        {
            if (Cache.TryGetValue(name, out var cached)) { return cached; }

            Sprite sprite = null;
            try
            {
                var path = Path.Combine(IconDirectory, name + ".png");
                if (File.Exists(path))
                {
                    // Size is a placeholder; LoadImage replaces the texture's dimensions with
                    // whatever the PNG actually contains.
                    var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (texture.LoadImage(File.ReadAllBytes(path)))
                    {
                        texture.filterMode = FilterMode.Bilinear;
                        sprite = Sprite.Create(
                            texture,
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f));
                        sprite.name = name;
                    }
                    else
                    {
                        Main.log.Log($"Icon '{name}' is not a readable PNG.");
                    }
                }
                else
                {
                    Main.log.Log($"Icon '{name}' not found at {path}.");
                }
            }
            catch (Exception e)
            {
                Main.log.Log(string.Concat($"Failed to load icon '{name}'.", e));
            }

            Cache[name] = sprite;
            return sprite;
        }

        /// <summary>
        /// The mod's icon if it exists, otherwise <paramref name="fallback"/>. Lets a blueprint
        /// opt into custom art without losing its stock icon before the art is drawn.
        /// </summary>
        public static Sprite GetOr(string name, Sprite fallback)
        {
            return Get(name) ?? fallback;
        }
    }
}
