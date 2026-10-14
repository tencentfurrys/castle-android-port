using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Creates a random Vector2. The unitCircleRadius represents the magnitude of the randomized vector. Result can be set fo be on the circle perimeter or inside the circle.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class RandomVector2 : FsmStateAction
	{
		public enum RandomUnitCircleOption
		{
			OnCirclePerimeter = 0,
			InsideCircle = 1
		}

		[Tooltip("Seed for the random number generator. Leave to none for true randomness. Only set to a number if you want predictability and repeatability")]
		public FsmInt seed;

		[Tooltip("Radius of imaginary circle")]
		public FsmFloat unitCircleRadius;

		[Tooltip("Define where the randomize occurs")]
		public RandomUnitCircleOption option;

		[UIHint(UIHint.Variable)]
		[Tooltip("The randomized vector 2")]
		[RequiredField]
		public FsmVector2 storeResult;

		[Tooltip("Update every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			seed = new FsmInt
			{
				UseVariable = true
			};
			storeResult = null;
			unitCircleRadius = 1f;
			option = RandomUnitCircleOption.OnCirclePerimeter;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (!seed.IsNone)
			{
				Random.seed = seed.Value;
			}
			GetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			GetValue();
		}

		private void GetValue()
		{
			if (option == RandomUnitCircleOption.InsideCircle)
			{
				storeResult.Value = Random.insideUnitCircle * unitCircleRadius.Value;
				return;
			}
			Vector3 vector = Random.onUnitSphere * unitCircleRadius.Value;
			storeResult.Value = new Vector2(vector.x, vector.y);
		}
	}
}
