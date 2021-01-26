using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

//public enum ESpineType
//{
//	SdSpine,
//	SpSpine
//}

public class BattleSpineController : MonoBehaviour
{

	public ESpineType spineType;

	//public SkeletonAnimation skeletonAnimation { get; private set; }
	public SkeletonAnimation skeletonAnimation;

	int mDepth = 5;

	// Use this for initialization
	//void Start ()
	//{
	//	if( target == null)
	//       {
	//		target = skeletonAnimation.gameObject;
	//	}
	//}

	protected void OnEnable()
	{
		
		if (target != null)
		{
			//            if (skeletonAnimation == null)
			skeletonAnimation = target.GetComponent<SkeletonAnimation>();

			InitMaterials();

			//Debug.LogError("OnEnable InitMaterials = " + target.name);

		}
	}


	private string _spinePrefabName;
	public string spinePrefabName
	{
		set
		{
			if (_spinePrefabName == value)
			{
				return;
			}

			DestroySpine();

			_spinePrefabName = value;
			_ChangeSpineData(value);

			//            Debug.Log("spinePrefabName InitMaterials");
			InitMaterials();
		}

		get { return _spinePrefabName; }
	}

	void InitMaterials()
	{

		if (gameObject.activeInHierarchy == false)
		{
			return;
		}

		if (skeletonAnimation == null)
			return;

		skeletonAnimation.skeletonDataAsset.scale = 1.0f;
		skeletonAnimation.skeletonDataAsset.Reset();
		skeletonAnimation.Initialize(true);

		GameObject spineObj = target;
		int _rendQ = mDepth;
		// Debug.Log("name = " + target.gameObject.name + " mDepth = " + _rendQ);
		StartCoroutine(SetUISpineDo(spineObj, _rendQ));
	}

	MeshRenderer _targetRenderer;
	public MeshRenderer targetRenderer
	{
		get
		{
			if (target == null) return null;
			if (_targetRenderer == null)
			{
				_targetRenderer = target.GetComponent<MeshRenderer>();
			}
			return _targetRenderer;
		}
	}

	private void _ChangeSpineData(string name)
	{
		Debug.Log("_ChangeSpineData = " + name);

		GameObject temp = Instantiate(Resources.Load(name)) as GameObject;

	//	if(spineType == ESpineType.SpSpine)
	//		temp.layer = LayerMask.NameToLayer("UI");
	//	else
    //    {
			temp.layer = LayerMask.NameToLayer("BATTLE");

		//}

		target = temp;

		target.transform.parent = this.transform;
		target.transform.localScale = new Vector3(1, 1, 1);
		target.transform.localPosition = new Vector3(0, 0, 0);

		SkeletonAnimation skeletonAnim = null;
		skeletonAnim = target.GetComponent<SkeletonAnimation>();

		skeletonAnim.gameObject.GetComponent<MeshRenderer>().sortingOrder = 0;
		target = skeletonAnim.gameObject;
		_targetRenderer = skeletonAnim.gameObject.GetComponent<MeshRenderer>();

		if (spineType == ESpineType.SpSpine)
			_targetRenderer.sortingOrder = 8;
		else
			_targetRenderer.sortingOrder = 6;

			//	target.layer = 5;

			skeletonAnimation = skeletonAnim;
		skeletonAnim.transform.SetParent(transform, true);


		//  Debug.Log("_ChangeSpineData InitMaterials = " + name);
		//InitMaterials();
		// skeletonAnimation.skeletonDataAsset.scale = 1.0f;

		switch (spineType)
		{
			case ESpineType.SdSpine:
				SetAnimation("01_idle");
				break;
			case ESpineType.SpSpine:
				SetAnimation("a_01_idle1");
				break;
			default:
				break;
		}
	}

	public IEnumerator SetUISpineDo(GameObject spineObj, int _rendQ)
	{
		//스파인 오브젝트를 생성하고 바로 실행한 경우 한프레임 대기하여 스파인 오브젝트 데이터 로드가 다 되도록 한다.

		yield return new WaitForEndOfFrame();
		//target.SetActive(true);

		if (spineObj == null)
			yield break;

		SkeletonAnimation spineObjskel = spineObj.GetComponent<SkeletonAnimation>();

		if (spineObjskel == null)
			yield break;

		//atlasMaterial.name = "SpineUI_Mat";
		spineObjskel.skeletonDataAsset.atlasAssets[0].materials[0].renderQueue = 3000 + _rendQ;
		Debug.Log("name = " + spineObjskel.name + " _rendQ = " + spineObjskel.skeletonDataAsset.atlasAssets[0].materials[0].renderQueue);
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary> 현재 애니메이션 loop 여부 </summary>
	public bool isLoop { get; private set; }
	public void SetAnimation(string aniName, bool _isLoop = true)
	{
		isLoop = _isLoop;
		skeletonAnimation.loop = isLoop;
		skeletonAnimation.AnimationName = null;
		skeletonAnimation.AnimationName = aniName;

	}

	[HideInInspector]
	[SerializeField]
	public GameObject target;

	/// <summary> 스파인 제거 </summary>
	public void DestroySpine()
	{
		if (target != null)
		{
			if (string.IsNullOrEmpty(_spinePrefabName) == false && skeletonAnimation != null)
			{
				skeletonAnimation.state.Complete -= AnimationComplete;
				skeletonAnimation = null;
				//                AssetBundleManager.UnLoad(_spinePrefabName);
			}
			GameObject.DestroyImmediate(target);
			target = null;
			_spinePrefabName = "";
			Resources.UnloadUnusedAssets();
		}

	}

	public delegate void OnSpineAnimCallback();
	/// <summary> 스파인 애니메이션 종료 콜백 </summary>
	public OnSpineAnimCallback OnComplete;

	void AnimationComplete(Spine.TrackEntry trackEntry)
	{
		//switch (spineType)
		//{
		//	case ESpineType.SdSpine:
		//		if (trackEntry.animation.name != "01_idle" && isLoop == false)
		//			SetAnimation("01_idle");
		//		break;
		//	case ESpineType.SpSpine:
		//		if (trackEntry.animation.name != "a_01_idle1" && isLoop == false)
		//			SetAnimation("a_01_idle1");
		//		break;
		//	default:
		//		break;
		//}

		if (trackEntry.animation.name != "01_idle" && isLoop == false)
			SetAnimation("01_idle");

		if (OnComplete != null)
		{
			OnComplete();
			OnComplete = null; // 1회만 보내기 위해 null처리
		}
	}

}
