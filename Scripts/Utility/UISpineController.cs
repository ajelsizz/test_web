using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


namespace FM_INVEN
{
    [ExecuteInEditMode]
    public class UISpineController : UIWidget
    {
        //bool _isStencil;
        //public bool isStencil
        //{
        //    get
        //    {
        //        return _isStencil;
        //    }
        //    set
        //    {
        //        _isStencil = value;
        //        //Init();
        //    }
        //}

        public bool clipping = false;

        public bool usePanelClip = false;
        public Rect clipRange;
        public Vector2 softness;
        public UIPanel _panel = null;

        [HideInInspector]
        [SerializeField]
        public GameObject target;

        public GameObject TartgetParent { get; set; }

        private string _spinePrefabName;
        public string spinePrefabName
        {
            get { return _spinePrefabName; }
            set
            {
                if (_spinePrefabName == value)
                {
                    return;
                }
                DestroySpine();
                _spinePrefabName = value;
                _ChangeSpineData(value);
            }
        }

        public SkeletonAnimation skeletonAnimation { get; private set; }

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

        public override Material material
        {
            get
            {
                if (skeletonAnimation != null)
                {
                    if (Application.isPlaying)
                    {
                        return skeletonAnimation.skeletonDataAsset.atlasAssets[0].materials[0];
                    }
                    else
                    {
                        return skeletonAnimation.skeletonDataAsset.atlasAssets[0].materials[0];
                    }
                }
                return null;
            }
            set
            {
                if (targetRenderer == null)
                    return;
                targetRenderer.material = value;
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (material == null)
                    return null;
                return material.mainTexture;
            }

            set
            {
                if (material == null)
                    return;
                material.mainTexture = value;
                base.mainTexture = value;
            }
        }

        public override Shader shader
        {
            get
            {
                if (material == null)
                    return null;
                return material.shader;
            }

            set
            {
                if (material == null)
                    return;
                material.shader = value;
                base.shader = value;
            }
        }

        public override void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
        {
            Vector4 dim = drawingDimensions;
            verts.Add(new Vector3(dim.x, dim.y, 0.0f));
            verts.Add(new Vector3(dim.x, dim.w, 0.0f));
            verts.Add(new Vector3(dim.z, dim.y, 0.0f));
            verts.Add(new Vector3(dim.z, dim.w, 0.0f));

            uvs.Add(Vector2.zero);
            uvs.Add(Vector2.zero);
            uvs.Add(Vector2.zero);
            uvs.Add(Vector2.zero);

            cols.Add(Color.clear);
            cols.Add(Color.clear);
            cols.Add(Color.clear);
            cols.Add(Color.clear);

            if (onPostFill != null)
            {
                onPostFill(this, verts.Count, verts, uvs, cols);
            }
#if UNITY_EDITOR
            UpdateRenderQueue();
            SetDepth();
#endif
        }


        public Renderer[] renderers;


        protected override void OnEnable()
        {
            base.OnEnable();
            if (usePanelClip)
            {
                _FindClipPanel();
            }

            if( target != null )
            {
                skeletonAnimation = target.GetComponent<SkeletonAnimation>();
                InitMaterials();
            }
        }

        void OnDestroy()
        {
            if (atlasMaterial != null)
            {
                RemoveMaterails();
            }
        }

        void RemoveMaterails()
        {
            NGUITools.DestroyImmediate(atlasMaterial);
            target = null;
            atlasMaterial = null;

            //Debug.Log("#### Remove Cloned Materails");
        }

        protected override void Awake()
        {
            base.Awake();
        }

        void LateUpdate()
        {
            UpdateRenderQueue();
        }

        protected int finalRenderQueue;

        public void InitMaterials()
        {
            if (target == null)
            {
                if (transform.childCount == 0)
                {
                    GameObject go = new GameObject();
                    go.transform.SetParent(transform, false);
                    go.name = "Target";
                    target = go;
                }
                else
                {
                    Transform t = transform.Find("Target");
                    if (t != null)
                    {
                        target = t.gameObject;
                    }
                }
            }

            StartCoroutine(SetUISpineDo());
        }

        Material atlasMaterial = null;
        IEnumerator SetUISpineDo()
        {
            yield return new WaitForEndOfFrame();
            target.SetActive(true);
            GameObject spineObj = target;
            int _rendQ = mDepth;


            SkeletonAnimation spineObjSkel = spineObj.GetComponent<SkeletonAnimation>();

            if (spineObjSkel == null)
                yield break;

            AtlasAsset atlasAsset = ScriptableObject.CreateInstance<AtlasAsset>();
            atlasAsset.atlasFile = spineObjSkel.skeletonDataAsset.atlasAssets[0].atlasFile;

            atlasMaterial = new Material(Shader.Find("Custom/Spine/UnlitTransparent Colored/One-OneMinusAlpha"));
            if (clipping)
            {
                _UpdateClipValue(atlasMaterial);
            }
            else
            {
                atlasMaterial.SetVector("_ClipRange0", new Vector4(0.0f, 0.0f, 2000.0f, 2000.0f));
                //mat.SetVector("_ClipArgs0", Vector4.one * 10000.0f);
                atlasMaterial.SetColor("_Color", color);
            }
            //if (!isStencil)
            //{
            //    atlasMaterial = new Material(Shader.Find("Spine/Skeleton"));
            //}
            //else
            //{
            //    atlasMaterial = new Material(Shader.Find("Spine/SkeletonSpriteMaskable"));
            //}

            atlasMaterial.mainTexture
                = spineObjSkel.skeletonDataAsset.atlasAssets[0].materials[0].mainTexture;
            atlasMaterial.name = "SpineUI_Mat";
            atlasMaterial.renderQueue = 3000 + _rendQ;

            atlasAsset.materials = new[] { atlasMaterial };


            SkeletonDataAsset skeletonDataAsset
                = Instantiate(spineObj.GetComponent<SkeletonAnimation>().skeletonDataAsset) as SkeletonDataAsset;
            skeletonDataAsset.atlasAssets[0] = atlasAsset;

            spineObjSkel.skeletonDataAsset = skeletonDataAsset;

            spineObjSkel.Initialize(true);

            skeletonAnimation.state.Complete += AnimationComplete;
        }
        void SetDepth()
        {
            return;
            if (target == null || atlasMaterial == null)
                return;

            GameObject spineObj = target;
            int _rendQ = mDepth;

            SkeletonAnimation spineObjSkel = spineObj.GetComponent<SkeletonAnimation>();
            AtlasAsset atlasAsset = ScriptableObject.CreateInstance<AtlasAsset>();

            atlasAsset.atlasFile = spineObjSkel.skeletonDataAsset.atlasAssets[0].atlasFile;
            atlasMaterial.renderQueue = finalRenderQueue;
            atlasAsset.materials = new[] { atlasMaterial };


            SkeletonDataAsset skeletonDataAsset
                = Instantiate(spineObj.GetComponent<SkeletonAnimation>().skeletonDataAsset) as SkeletonDataAsset;
            skeletonDataAsset.atlasAssets[0] = atlasAsset;

            spineObjSkel.skeletonDataAsset = skeletonDataAsset;

            spineObjSkel.Initialize(true);
        }

        void UpdateRenderQueue()
        {
            if (drawCall == null)
                return;

            Material mat = atlasMaterial;
            if (mat != null)
            {
                if (drawCall != null)
                {
                    mat.renderQueue = drawCall.finalRenderQueue;
                }

                finalRenderQueue = mat.renderQueue;
            }
            //material = mat;
        }


        public ESpineType spineType;
        private void _ChangeSpineData(string name)
        {
            SkeletonAnimation skeletonAnim = null;
            switch (spineType)
            {
                case ESpineType.SdSpine:
                    skeletonAnim = PrefabLoadManager.Instance.GetSDSpineAnimation(name);

                    break;
                case ESpineType.SpSpine:
                    skeletonAnim = PrefabLoadManager.Instance.GetSPSpineAnimation(name);
                    break;
                default:
                    break;
            }
            if (skeletonAnim == null)
                return;

            skeletonAnim.gameObject.SetActive(false);
            skeletonAnim.gameObject.GetComponent<MeshRenderer>().sortingOrder = order;
            target = skeletonAnim.gameObject;
            _targetRenderer = skeletonAnim.gameObject.GetComponent<MeshRenderer>();
            skeletonAnimation = skeletonAnim;

            //target.layer = 5;
            target.layer =  LayerMask.NameToLayer("UI");
            skeletonAnim.transform.SetParent(transform, false);

            InitMaterials();
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

        public delegate void OnSpineAnimCallback();
        /// <summary> 스파인 애니메이션 종료 콜백 </summary>
        public OnSpineAnimCallback OnComplete;

        void AnimationComplete(Spine.TrackEntry trackEntry)
        {
            switch (spineType)
            {
                case ESpineType.SdSpine:
                    if (trackEntry.animation.name != "01_idle" && isLoop == false && isEndStop == false)
                        SetAnimation("01_idle");
                    break;
                case ESpineType.SpSpine:
                    if (trackEntry.animation.name != "a_01_idle1" && isLoop == false && isEndStop == false)
                        SetAnimation("a_01_idle1");
                    break;
                default:
                    break;
            }

            if (OnComplete != null)
            {
                OnComplete();
                OnComplete = null; // 1회만 보내기 위해 null처리
            }
        }

        /// <summary> 현재 애니메이션 loop 여부 </summary>
        public bool isLoop { get; private set; }
        /// <summary> 해당 애니메이션 종료시 멈춤 </summary>
        public bool isEndStop { get; private set; }

        /// <summary> 스파인 애니메이션 실행 </summary>
        /// <param name="aniName"> 애니메이션 이름 </param>
        /// <param name="_isLoop"> 반복 여부 </param>
        /// <param name="_isEndStop"> 애니메이션이 1회 플레이후 종료될 시 idle 애니로 안넘어가고 멈출지 여부 </param>
        public void SetAnimation(string aniName, bool _isLoop = true, bool _isEndStop = false)
        {
            if (skeletonAnimation == null)
            {
                Debug.LogWarning("skeletonAnimation null");
                return;
            }

            isLoop = _isLoop;
            isEndStop = _isEndStop;
            skeletonAnimation.loop = isLoop;
            skeletonAnimation.AnimationName = null;
            skeletonAnimation.AnimationName = aniName;

        }

        int order = 0;
        /// <summary> 스파인 세팅 (생성시 자동으로 기존의 스파인은 제거됨)</summary>
        public void SetSpine(ESpineType _spineType, string _spineName, int _depth, int _order = 0)
        {
            order = _order;
            depth = _depth;
            spineType = _spineType;
            spinePrefabName = _spineName;
        }

        /// <summary> 스파인 제거 </summary>
        public void DestroySpine()
        {
            if (target != null)
            {
                if (string.IsNullOrEmpty(_spinePrefabName) == false && skeletonAnimation != null)
                {
                    skeletonAnimation.state.Complete -= AnimationComplete;
                    skeletonAnimation = null;
           //         AssetBundleManager.UnLoad(_spinePrefabName);
                }
                GameObject.DestroyImmediate(target);
                target = null;
                _spinePrefabName = "";
                Resources.UnloadUnusedAssets();
            }
        }

        ///// <summary> 스파인 인터렉션 </summary>
        //public SpineInteraction spineInteraction
        //{
        //    get
        //    {
        //        if (target == null)
        //            return null;

        //        return target.GetComponentInChildren<SpineInteraction>();
        //    }
        //}

        private void _UpdateClipValue(Material mat)
        {
            if (mat == null)
                return;
            Vector4 finalClip =
                new Vector4(
                    clipRange.x, clipRange.y, clipRange.width, clipRange.height
                    );
            Vector4 finalSoftness = new Vector4(softness.x, softness.y);
            if (usePanelClip)
            {
                if (_panel == null)
                {
                    _FindClipPanel();
                }
                if (_panel != null)
                {


                    Vector3 pos =
                        cachedTransform.InverseTransformPoint(
                        _panel.cachedTransform.position);
                    /*
                    finalClip.x = pos.x + _panel.clipOffset.x + _panel.baseClipRegion.x;
                    finalClip.y = pos.y + _panel.clipOffset.y + _panel.baseClipRegion.y;
                    finalClip.z = _panel.baseClipRegion.z;
                    finalClip.w = _panel.baseClipRegion.w;
                    //*/
                    finalSoftness.x = _panel.clipSoftness.x;
                    finalSoftness.y = _panel.clipSoftness.y;

                    finalClip = _panel.drawCallClipRange;

                    //finalClip.z += softness.x;
                    //finalClip.w += softness.y;
                    float sizeX = _panel.cachedTransform.lossyScale.x / transform.lossyScale.x;
                    float sizeY = _panel.cachedTransform.lossyScale.y / transform.lossyScale.y;
                    finalClip.z *= sizeX;
                    finalClip.w *= sizeY;

                    finalClip.x += pos.x;
                    finalClip.y += pos.y;

                    // 자식으로 붙은 스파인오브젝트의 오프셋 보정해줘야함.
                    finalClip.x -= target.transform.localPosition.x * sizeX;
                    finalClip.y -= target.transform.localPosition.y * sizeY;

                    clipRange.x = finalClip.x;
                    clipRange.y = finalClip.y;
                    clipRange.width = finalClip.z;
                    clipRange.height = finalClip.w;
                    softness = _panel.clipSoftness;
                }
            }
            else
            {
                _panel = null;
                // 자식으로 붙은 스파인오브젝트의 오프셋 보정해줘야함.
                finalClip.x -= target.transform.localPosition.x;
                finalClip.y -= target.transform.localPosition.y;
            }

            if (finalSoftness.x > 0.0f)
            {
                finalSoftness.x = finalClip.z / finalSoftness.x;
            }
            else
            {
                finalSoftness.x = 0.0f;
            }
            if (finalSoftness.y > 0.0f)
            {
                finalSoftness.y = finalClip.w / finalSoftness.y;
            }
            else
            {
                finalSoftness.y = 0.0f;
            }

            mat.SetVector("_ClipRange0", finalClip);
            //mat.SetVector("_ClipRange0", new Vector4(-finalClip.x / finalClip.z, -finalClip.y / finalClip.w, 1f / finalClip.z, 1f / finalClip.w));
            //mat.SetVector("_ClipArgs0", finalSoftness);
            mat.SetColor("_Color", color);
        }

        private void _FindClipPanel()
        {
            Transform t = transform.parent;
            while (t != null)
            {
                UIPanel panel = t.GetComponent<UIPanel>();
                if (panel != null)
                {
                    if (panel.clipping == UIDrawCall.Clipping.SoftClip)
                    {
                        _panel = panel;
                        break;
                    }
                }
                t = t.parent;
            }
        }
    }
}

