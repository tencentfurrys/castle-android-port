using DG.Tweening;
using Doozy.PlaymakerActions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DOTween")]
	[Tooltip("Tweens a Transform's localPosition through the given path waypoints, using the chosen path algorithm.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	public class DOTweenTransformLocalPath : FsmStateAction
	{
		public enum LookAt
		{
			nothing = 0,
			position = 1,
			target = 2,
			ahead = 3
		}

		[RequiredField]
		[CheckForComponent(typeof(Transform))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The waypoints to go through")]
		[RequiredField]
		public FsmGameObject[] path;

		[Tooltip("The type of path: Linear (straight path) or CatmullRom (curved CatmullRom path)")]
		public PathType pathType;

		[Tooltip("The path mode: 3D, side-scroller 2D, top-down 2D")]
		public PathMode pathMode;

		[Tooltip("The resolution of the path (useless in case of Linear paths): higher resolutions make for more detailed curved paths but are more expensive. Defaults to 10, but a value of 5 is usually enough if you don't have dramatic long curves between waypoints")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt resolution;

		[UIHint(UIHint.FsmColor)]
		[Tooltip("The color of the path (shown when gizmos are active in the Play panel and the tween is running)")]
		public FsmColor gizmoColor;

		[Tooltip("The duration of the tween")]
		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
		public FsmFloat duration;

		[Tooltip("If isSpeedBased is TRUE sets the tween as speed based (the duration will represent the number of units/degrees the tween moves x second). NOTE: if you want your speed to be constant, also set the ease to Ease.Linear.")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool setSpeedBased;

		[Tooltip("Set a delayed startup for the tween")]
		[UIHint(UIHint.FsmFloat)]
		public FsmFloat startDelay;

		[ActionSection("Set Path Options")]
		[Tooltip("If TRUE the path will be automatically closed")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool closePath;

		[Tooltip("The eventual movement axis to lock.")]
		public AxisConstraint lockPosition;

		[Tooltip("The eventual rotation axis to lock.")]
		public AxisConstraint lockRotation;

		[Tooltip("Select the look at target.")]
		[ActionSection("Set Look At Options")]
		public LookAt lookAt;

		[Tooltip("The position to look at. Orients the target towards the given position")]
		[UIHint(UIHint.FsmVector3)]
		public FsmVector3 lookAtPosition;

		[Tooltip("The target to look at. Orients the target towards the given transform of the GameObject")]
		[UIHint(UIHint.FsmGameObject)]
		public FsmGameObject lookAtTarget;

		[UIHint(UIHint.FsmFloat)]
		[Tooltip("The lookAhead percentage to use when orienting to the path (0 to 1). Orients the target to the path with the given lookAhead")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat lookAhead;

		[UIHint(UIHint.FsmVector3)]
		[Tooltip("The eventual direction to consider as 'forward'. Default: the regular forward side of the transform.")]
		[ActionSection("Set Custom Direction to consider as 'forward'")]
		public FsmVector3 forwardDirection;

		[UIHint(UIHint.FsmVector3)]
		[Tooltip("The vector that defines in which direction up is. Default: Vector3.up")]
		[ActionSection("Set Custom Up to consider which direction is 'up'")]
		public FsmVector3 up;

		[UIHint(UIHint.FsmEvent)]
		[ActionSection("Events")]
		public FsmEvent startEvent;

		[UIHint(UIHint.FsmEvent)]
		public FsmEvent finishEvent;

		[UIHint(UIHint.FsmBool)]
		[Tooltip("If TRUE this action will finish immediately, if FALSE it will finish when the tween is complete.")]
		public FsmBool finishImmediately;

		[UIHint(UIHint.Description)]
		[ActionSection("Tween ID")]
		public string tweenIdDescription = "Set an ID for the tween, which can then be used as a filter with DOTween's Control Methods";

		[Tooltip("Select the source for the tween ID")]
		public DOTweenActionsEnums.TweenId tweenIdType;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Use a String as the tween ID")]
		public FsmString stringAsId;

		[UIHint(UIHint.Tag)]
		[Tooltip("Use a Tag as the tween ID")]
		public FsmString tagAsId;

		[ActionSection("Ease Settings")]
		public DOTweenActionsEnums.SelectedEase selectedEase;

		[Tooltip("Sets the ease of the tween. If applied to a Sequence instead of a Tweener, the ease will be applied to the whole Sequence as if it was a single animated timeline.Sequences always have Ease.Linear by default, independently of the global default ease settings.")]
		public Ease easeType;

		public FsmAnimationCurve animationCurve;

		[UIHint(UIHint.Description)]
		[ActionSection("Loop Settings")]
		public string loopsDescriptionArea = "Setting loops to -1 will make the tween loop infinitely.";

		[Tooltip("Setting loops to -1 will make the tween loop infinitely.")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt loops;

		[Tooltip("Sets the looping options (Restart, Yoyo, Incremental) for the tween. LoopType.Restart: When a loop ends it will restart from the beginning. LoopType.Yoyo: When a loop ends it will play backwards until it completes another loop, then forward again, then backwards again, and so on and on and on. LoopType.Incremental: Each time a loop ends the difference between its endValue and its startValue will be added to the endValue, thus creating tweens that increase their values with each loop cycle. Has no effect if the tween has already started.Also, infinite loops will not be applied if the tween is inside a Sequence.")]
		public LoopType loopType;

		[ActionSection("Special Settings")]
		[Tooltip("If autoKillOnCompletion is set to TRUE the tween will be killed as soon as it completes, otherwise it will stay in memory and you'll be able to reuse it.")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool autoKillOnCompletion;

		[Tooltip("Sets the recycling behaviour for the tween. If you don't set it then the default value (set either via DOTween.Init or DOTween.defaultRecyclable) will be used.")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool recyclable;

		[Tooltip("Sets the type of update (Normal, Late or Fixed) for the tween and eventually tells it to ignore Unity's timeScale. UpdateType.Normal: Updates every frame during Update calls. UpdateType.Late: Updates every frame during LateUpdate calls. UpdateType.Fixed: Updates using FixedUpdate calls. ")]
		public UpdateType updateType;

		[Tooltip(" If TRUE the tween will ignore Unity's Time.timeScale. NOTE: independentUpdate works also with UpdateType.Fixed but is not recommended in that case (because at timeScale 0 FixedUpdate won't run).")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool isIndependentUpdate;

		[UIHint(UIHint.FsmBool)]
		[ActionSection("Debug Options")]
		public FsmBool debugThis;

		private Tweener tweener;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			duration = new FsmFloat
			{
				UseVariable = false
			};
			setSpeedBased = new FsmBool
			{
				UseVariable = false,
				Value = false
			};
			pathType = PathType.Linear;
			pathMode = PathMode.Full3D;
			resolution = new FsmInt
			{
				UseVariable = false,
				Value = 10
			};
			gizmoColor = new FsmColor
			{
				UseVariable = false
			};
			closePath = new FsmBool
			{
				UseVariable = false,
				Value = false
			};
			lockPosition = AxisConstraint.None;
			lockRotation = AxisConstraint.None;
			lookAt = LookAt.nothing;
			lookAtPosition = new FsmVector3
			{
				UseVariable = false,
				Value = Vector3.zero
			};
			lookAtTarget = new FsmGameObject
			{
				UseVariable = false,
				Value = null
			};
			lookAhead = new FsmFloat
			{
				UseVariable = false,
				Value = 0f
			};
			forwardDirection = new FsmVector3
			{
				UseVariable = false,
				Value = Vector3.forward
			};
			up = new FsmVector3
			{
				UseVariable = false,
				Value = Vector3.up
			};
			startEvent = null;
			finishEvent = null;
			finishImmediately = new FsmBool
			{
				UseVariable = false,
				Value = false
			};
			startDelay = new FsmFloat
			{
				Value = 0f
			};
			easeType = Ease.Linear;
			loops = new FsmInt
			{
				Value = 0
			};
			loopType = LoopType.Restart;
			autoKillOnCompletion = new FsmBool
			{
				Value = true
			};
			recyclable = new FsmBool
			{
				Value = false
			};
			updateType = UpdateType.Normal;
			isIndependentUpdate = new FsmBool
			{
				Value = false
			};
			debugThis = new FsmBool
			{
				Value = false
			};
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				if (debugThis.Value)
				{
					Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "] State [" + base.State.Name + "] - DOTween Transform Local Path - ERROR: The gameObject is null");
				}
				return;
			}
			Transform component = ownerDefaultTarget.GetComponent<Transform>();
			if (component == null)
			{
				if (debugThis.Value)
				{
					Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "] State [" + base.State.Name + "] - DOTween Transform Local Path - ERROR: The GameObject [" + ownerDefaultTarget.name + "] does not have a Transform Component");
				}
				return;
			}
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = path[i].Value.transform.position;
			}
			switch (lookAt)
			{
			case LookAt.nothing:
				tweener = component.DOLocalPath(array, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value).SetOptions(closePath.Value, lockPosition, lockRotation);
				break;
			case LookAt.position:
				tweener = component.DOLocalPath(array, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value).SetOptions(closePath.Value, lockPosition, lockRotation).SetLookAt(lookAtPosition.Value, forwardDirection.Value, up.Value);
				break;
			case LookAt.target:
				tweener = component.DOLocalPath(array, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value).SetOptions(closePath.Value, lockPosition, lockRotation).SetLookAt(lookAtTarget.Value.transform, forwardDirection.Value, up.Value);
				break;
			case LookAt.ahead:
				tweener = component.DOLocalPath(array, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value).SetOptions(closePath.Value, lockPosition, lockRotation).SetLookAt(lookAhead.Value, forwardDirection.Value, up.Value);
				break;
			}
			if (setSpeedBased.Value)
			{
				tweener.SetSpeedBased();
			}
			switch (tweenIdType)
			{
			case DOTweenActionsEnums.TweenId.UseString:
				if (!string.IsNullOrEmpty(stringAsId.Value))
				{
					tweener.SetId(stringAsId.Value);
				}
				break;
			case DOTweenActionsEnums.TweenId.UseTag:
				if (!string.IsNullOrEmpty(tagAsId.Value))
				{
					tweener.SetId(tagAsId.Value);
				}
				break;
			case DOTweenActionsEnums.TweenId.UseGameObject:
				tweener.SetId(base.Fsm.GetOwnerDefaultTarget(gameObject));
				break;
			}
			tweener.SetDelay(startDelay.Value);
			switch (selectedEase)
			{
			case DOTweenActionsEnums.SelectedEase.EaseType:
				tweener.SetEase(easeType);
				break;
			case DOTweenActionsEnums.SelectedEase.AnimationCurve:
				tweener.SetEase(animationCurve.curve);
				break;
			}
			tweener.SetLoops(loops.Value, loopType);
			tweener.SetAutoKill(autoKillOnCompletion.Value);
			tweener.SetRecyclable(recyclable.Value);
			tweener.SetUpdate(updateType, isIndependentUpdate.Value);
			if (startEvent != null)
			{
				tweener.OnStart(() =>
				{
					base.Fsm.Event(startEvent);
				});
			}
			if (!finishImmediately.Value)
			{
				if (finishEvent != null)
				{
					tweener.OnComplete(() =>
					{
						base.Fsm.Event(finishEvent);
					});
				}
				else
				{
					tweener.OnComplete(base.Finish);
				}
			}
			tweener.Play();
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "] State [" + base.State.Name + "] - DOTween Transform Local Path - SUCCESS!");
			}
			if (finishImmediately.Value)
			{
				Finish();
			}
		}
	}
}
