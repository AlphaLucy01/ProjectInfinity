using ProjectInfinity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace ProjectInfinity
{
	public class ProjectInfinity : Mod
	{
        private List<IOrderedLoadable> loadCache;
        public static ProjectInfinity Instance { get; set; }

		public ProjectInfinity()
		{
			
		}
		public override void Load()
		{
            Instance = this;
			Logger.Debug("Modname is:" +this.Name);

            loadCache = new List<IOrderedLoadable>();

            foreach (Type type in Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetInterfaces().Contains(typeof(IOrderedLoadable)))
                {
                    object instance = Activator.CreateInstance(type);
                    loadCache.Add(instance as IOrderedLoadable);
                }

                loadCache.Sort((n, t) => n.Priority.CompareTo(t.Priority));
            }

            for (int k = 0; k < loadCache.Count; k++)
            {
                loadCache[k].Load();
                SetLoadingText("Loading " + loadCache[k].GetType().Name);
            }
        }
		public override void Unload()
		{
			Instance = null;
            foreach (IOrderedLoadable loadable in loadCache)
            {
                loadable.Unload();
            }

            loadCache = null;
        }
        public static void SetLoadingText(string text)
        {
            FieldInfo Interface_loadMods = typeof(Mod).Assembly.GetType("Terraria.ModLoader.UI.Interface")!.GetField("loadMods", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo UIProgress_set_SubProgressText = typeof(Mod).Assembly.GetType("Terraria.ModLoader.UI.UIProgress")!.GetProperty("SubProgressText", BindingFlags.Public | BindingFlags.Instance)!.GetSetMethod()!;

            UIProgress_set_SubProgressText.Invoke(Interface_loadMods.GetValue(null), new object[] { text });
        }
    }
}