using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public class LinkerData : ScriptableObject
	{
		private static Type[] _unlinkedTyped = new Type[1] { typeof(Collider2D) };

		public bool debug = true;

		public bool LinkContentUpdateDone;

		public TextAsset Asset;

		public string AssetPath;

		public LinkerData self;

		public Dictionary<string, List<string>> linkerEntries = new Dictionary<string, List<string>>();

		private static LinkerData _instance_;

		public static bool DebugAll
		{
			get
			{
				if (instance == null)
				{
					return true;
				}
				return instance.debug;
			}
		}

		public static LinkerData instance
		{
			get
			{
				if (_instance_ == null)
				{
					return null;
				}
				return _instance_.self;
			}
			set
			{
				_instance_ = value;
			}
		}

		public static void RegisterClassDependancy(Type type, string typeName)
		{
			if (type == null)
			{
				Debug.LogWarning("LinkerData RegisterClassDependancy with no type");
				return;
			}
			if (instance == null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}
			Type[] unlinkedTyped = _unlinkedTyped;
			foreach (Type type2 in unlinkedTyped)
			{
				if (type.IsSubclassOf(type2))
				{
					instance.RegisterLinkerEntry(type2.Namespace, type2.AssemblyQualifiedName);
				}
			}
			instance.RegisterLinkerEntry(type.Assembly.FullName, typeName);
		}

		public static void RegisterClassDependancy(string assemblyName, string typeName)
		{
			if (instance == null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
			}
			else
			{
				instance.RegisterLinkerEntry(assemblyName, typeName);
			}
		}

		public void RegisterLinkerEntry(string assemblyName, string typeName)
		{
			if (instance == null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}
			if (string.IsNullOrEmpty(assemblyName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>assemblyName</color>");
				return;
			}
			if (string.IsNullOrEmpty(typeName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>typeName</color>");
				return;
			}
			if (assemblyName.Contains(","))
			{
				assemblyName = assemblyName.Split(","[0])[0];
			}
			if (typeName.Contains(","))
			{
				typeName = typeName.Split(","[0])[0];
			}
			if (!linkerEntries.ContainsKey(assemblyName))
			{
				linkerEntries.Add(assemblyName, new List<string>());
			}
			linkerEntries[assemblyName].Add(typeName);
		}

		private void OnEnable()
		{
			_instance_ = this;
			self = this;
		}
	}
}
