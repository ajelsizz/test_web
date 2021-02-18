using System.Collections.Generic;
using UnityEngine;

namespace Banana.Patterns
{
    public interface IManagedObject
    {
        void PreInit();
        void Release();
    }

    public class ObjectPool<T> where T : Component
    {
        private Stack<T> _objectPool;
        private List<T> _popList;

        private T _oriObject;
        private Transform _parent;

        private int _overAllocateCount;
        private string _objName;
        private bool _setActive;
        private int _currAllocCount;

        public ObjectPool(T originalObject, Transform parent, int allocCount, int overAllocCount, bool setActive = false)
        {
            _oriObject = originalObject;
            _parent = parent;
            _setActive = setActive;
            _overAllocateCount = overAllocCount;

            _objectPool = new Stack<T>(allocCount);
            _popList = new List<T>();

            _currAllocCount = 0;

            Allocate(allocCount);
        }

        public void Allocate(int allocCount)
        {
            for(var i = 0; i < allocCount; ++i)
            {
                var obj = Object.Instantiate(_oriObject, _parent, false);
                obj.name = _oriObject.name + _currAllocCount;
                obj.gameObject.SetActive(_setActive);

                if (obj is IManagedObject)
                    (obj as IManagedObject).PreInit();

                _objectPool.Push(obj);
                ++_currAllocCount;
            }
        }

        public T Pop(bool setActive = true)
        {
            if (_objectPool.Count <= 0)
                Allocate(_overAllocateCount);

            var retObj = _objectPool.Pop();
            _popList.Add(retObj);
            retObj.gameObject.SetActive(setActive);
            return retObj;
        }

        public void Push(T obj, bool setActive = false,  bool isRemoveInList = true)
        {
            obj.transform.SetParent(_parent);
            obj.gameObject.SetActive(setActive);

            if (_objectPool.Contains(obj) == false)
            {
                if (obj is IManagedObject)
                    (obj as IManagedObject).Release();

                _objectPool.Push(obj);

                if (isRemoveInList)
                    _popList.Remove(obj);
            }
        }

        public void Push(T[] objs, bool setActive = false)
        {
            foreach (var obj in objs)
                Push(obj, setActive);
        }

        public void PushAll(bool setActive)
        {
            if (_popList.Count == 0)
                return;

            foreach(var popObj in _popList)
                Push(popObj, setActive, false);

            _popList.Clear();
        }
    }
}
