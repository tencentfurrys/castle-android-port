using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	[Serializable]
	public class PlayMakerFsmVariable
	{
		public VariableSelectionChoice variableSelectionChoice;

		public VariableType selectedType = VariableType.Unknown;

		public string variableName;

		public string defaultVariableName;

		public bool initialized;

		public bool targetUndefined = true;

		private string variableNameToUse = string.Empty;

		private FsmVariables fsmVariables;

		private NamedVariable _namedVariable;

		private FsmFloat _float;

		private FsmInt _int;

		private FsmBool _bool;

		private FsmGameObject _gameObject;

		private FsmColor _color;

		private FsmMaterial _material;

		private FsmObject _object;

		private FsmQuaternion _quaternion;

		private FsmRect _rect;

		private FsmString _string;

		private FsmTexture _texture;

		private FsmVector2 _vector2;

		private FsmVector3 _vector3;

		public NamedVariable namedVariable
		{
			get
			{
				return _namedVariable;
			}
		}

		public FsmFloat FsmFloat
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Float) || selectedType != VariableType.Float)
				{
					Debug.LogError("Trying to access a FloatFsm Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_float == null && fsmVariables != null && selectedType == VariableType.Float)
				{
					_float = fsmVariables.GetFsmFloat(variableNameToUse);
				}
				return _float;
			}
		}

		public FsmInt FsmInt
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Int) || selectedType != VariableType.Int)
				{
					Debug.LogError("Trying to access a FsmInt Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_int == null && fsmVariables != null && selectedType == VariableType.Int)
				{
					_int = fsmVariables.GetFsmInt(variableNameToUse);
				}
				return _int;
			}
		}

		public FsmBool FsmBool
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Bool) || selectedType != VariableType.Bool)
				{
					Debug.LogError("Trying to access a FsmBool Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_bool == null && fsmVariables != null && selectedType == VariableType.Bool)
				{
					_bool = fsmVariables.GetFsmBool(variableNameToUse);
				}
				return _bool;
			}
		}

		public FsmGameObject FsmGameObject
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.GameObject) || selectedType != VariableType.GameObject)
				{
					Debug.LogError("Trying to access a FsmGameObject Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_gameObject == null && fsmVariables != null && selectedType == VariableType.GameObject)
				{
					_gameObject = fsmVariables.GetFsmGameObject(variableNameToUse);
				}
				return _gameObject;
			}
		}

		public FsmColor FsmColor
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Color) || selectedType != VariableType.Color)
				{
					Debug.LogError("Trying to access a FsmColor Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_color == null && fsmVariables != null && selectedType == VariableType.Color)
				{
					_color = fsmVariables.GetFsmColor(variableNameToUse);
				}
				return _color;
			}
		}

		public FsmMaterial FsmMaterial
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Material) || selectedType != VariableType.Material)
				{
					Debug.LogError("Trying to access a FsmMaterial Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_material == null && fsmVariables != null && selectedType == VariableType.Material)
				{
					_material = fsmVariables.GetFsmMaterial(variableNameToUse);
				}
				return _material;
			}
		}

		public FsmObject FsmObject
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Object) || selectedType != VariableType.Object)
				{
					Debug.LogError("Trying to access a FsmObject Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_object == null && fsmVariables != null && selectedType == VariableType.Object)
				{
					_object = fsmVariables.GetFsmObject(variableNameToUse);
				}
				return _object;
			}
		}

		public FsmQuaternion FsmQuaternion
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Quaternion) || selectedType != VariableType.Quaternion)
				{
					Debug.LogError("Trying to access a FsmQuaternion Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_quaternion == null && fsmVariables != null && selectedType == VariableType.Quaternion)
				{
					_quaternion = fsmVariables.GetFsmQuaternion(variableNameToUse);
				}
				return _quaternion;
			}
		}

		public FsmRect FsmRect
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Rect) || selectedType != VariableType.Rect)
				{
					Debug.LogError("Trying to access a FsmRect Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_rect == null && fsmVariables != null && selectedType == VariableType.Rect)
				{
					_rect = fsmVariables.GetFsmRect(variableNameToUse);
				}
				return _rect;
			}
		}

		public FsmString FsmString
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.String) || selectedType != VariableType.String)
				{
					Debug.LogError("Trying to access a FsmString Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_string == null && fsmVariables != null && selectedType == VariableType.String)
				{
					_string = fsmVariables.GetFsmString(variableNameToUse);
				}
				return _string;
			}
		}

		public FsmTexture FsmTexture
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Texture) || selectedType != VariableType.Texture)
				{
					Debug.LogError("Trying to access a FsmTexture Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_texture == null && fsmVariables != null && selectedType == VariableType.Texture)
				{
					_texture = fsmVariables.GetFsmTexture(variableNameToUse);
				}
				return _texture;
			}
		}

		public FsmVector2 FsmVector2
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Vector2) || selectedType != VariableType.Vector2)
				{
					Debug.LogError("Trying to access a FsmVector2 Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_vector2 == null && fsmVariables != null && selectedType == VariableType.Vector2)
				{
					_vector2 = fsmVariables.GetFsmVector2(variableNameToUse);
				}
				return _vector2;
			}
		}

		public FsmVector3 FsmVector3
		{
			get
			{
				if ((variableSelectionChoice != VariableSelectionChoice.Any && variableSelectionChoice != VariableSelectionChoice.Vector3) || selectedType != VariableType.Vector3)
				{
					Debug.LogError("Trying to access a FsmVector3 Variable when the variable type is actually " + selectedType);
					return null;
				}
				if (_vector3 == null && fsmVariables != null && selectedType == VariableType.Vector3)
				{
					_vector3 = fsmVariables.GetFsmVector3(variableNameToUse);
				}
				return _vector3;
			}
		}

		public PlayMakerFsmVariable()
		{
		}

		public PlayMakerFsmVariable(VariableSelectionChoice variableSelectionChoice)
		{
			this.variableSelectionChoice = variableSelectionChoice;
		}

		public PlayMakerFsmVariable(string defaultVariableName)
		{
			this.defaultVariableName = defaultVariableName;
		}

		public PlayMakerFsmVariable(VariableSelectionChoice variableSelectionChoice, string defaultVariableName)
		{
			this.variableSelectionChoice = variableSelectionChoice;
			this.defaultVariableName = defaultVariableName;
		}

		public bool GetVariable(PlayMakerFsmVariableTarget variableTarget)
		{
			initialized = true;
			targetUndefined = true;
			if (variableTarget.FsmVariables != null)
			{
				targetUndefined = false;
				variableNameToUse = ((!string.IsNullOrEmpty(variableName)) ? variableName : defaultVariableName);
				fsmVariables = variableTarget.FsmVariables;
				_namedVariable = fsmVariables.GetVariable(variableNameToUse);
				if (_namedVariable != null)
				{
					selectedType = GetTypeFromChoice(variableSelectionChoice);
					return true;
				}
			}
			selectedType = VariableType.Unknown;
			return false;
		}

		public override string ToString()
		{
			string text = "<color=blue>" + variableName + "</color>";
			if (string.IsNullOrEmpty(text))
			{
				text = "<color=red>None</color>";
			}
			return string.Format(string.Concat("PlayMaker Variable<{0}>: {1} (", _namedVariable, ")"), variableSelectionChoice, text);
		}

		public static VariableType GetTypeFromChoice(VariableSelectionChoice choice)
		{
			switch (choice)
			{
			case VariableSelectionChoice.Any:
				return VariableType.Unknown;
			case VariableSelectionChoice.Float:
				return VariableType.Float;
			case VariableSelectionChoice.Int:
				return VariableType.Int;
			case VariableSelectionChoice.Bool:
				return VariableType.Bool;
			case VariableSelectionChoice.GameObject:
				return VariableType.GameObject;
			case VariableSelectionChoice.String:
				return VariableType.String;
			case VariableSelectionChoice.Vector2:
				return VariableType.Vector2;
			case VariableSelectionChoice.Vector3:
				return VariableType.Vector3;
			case VariableSelectionChoice.Color:
				return VariableType.Color;
			case VariableSelectionChoice.Rect:
				return VariableType.Rect;
			case VariableSelectionChoice.Material:
				return VariableType.Material;
			case VariableSelectionChoice.Texture:
				return VariableType.Texture;
			case VariableSelectionChoice.Quaternion:
				return VariableType.Quaternion;
			case VariableSelectionChoice.Object:
				return VariableType.Object;
			default:
				return VariableType.Unknown;
			}
		}
	}
}
