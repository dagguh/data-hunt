using System;
using UnityEngine;

namespace view.gui
{
    public static class GameObjectExtensions
    {
        public static GameObject AttachTo(this GameObject child, GameObject parent)
        {
            child.transform.SetParent(parent.transform, false);
            return child;
        }

        public static GameObject FindOrFail(string name)
        {
            var gameObject = GameObject.Find(name);
            if (gameObject == null)
            {
                throw new SystemException("Cannot find game object named " + name + ". Maybe it's inactive?");
            }
            return gameObject;
        }
    }
}
