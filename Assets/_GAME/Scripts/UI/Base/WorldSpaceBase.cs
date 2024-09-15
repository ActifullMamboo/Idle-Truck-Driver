using System;
using _Game.Scripts.Ui.Base;
using UnityEngine;

namespace _GAME.Scripts.UI.Base
{
    public class WorldSpaceBase : BaseWindow
    {
        private Transform t;
        public Vector3 offset;

        private void LateUpdate()
        {
            transform.position = t.position + offset;
        }

        public virtual void Show(Transform target)
        {
            Anchor anchor = new()
            {
                Scale = Vector3.one,
                EulerAngles = Vector3.zero,
                Position = Vector3.zero
            };

            if (target.TryGetComponent(out WorldSpacePanelOffset worldSpacePanelOffset))
                anchor = worldSpacePanelOffset.Anchor;

            var transform1 = transform;
            t = target;

            offset = anchor.Position;
            transform1.position = target.position + anchor.Position;
            transform1.eulerAngles = Camera.main.transform.eulerAngles;
            transform1.localScale = anchor.Scale;
            base.Open();
        }
        public virtual void Show(Transform target, Vector3 pos, bool withOpen = true)
        {
            Anchor anchor = new()
            {
                Scale = Vector3.one,
                EulerAngles = Vector3.zero,
                Position = Vector3.zero
            };

            if (target.TryGetComponent(out WorldSpacePanelOffset worldSpacePanelOffset))
                anchor = worldSpacePanelOffset.Anchor;

            var transform1 = transform;
            t = target;

            offset = anchor.Position+ pos;
            transform1.position = target.position + anchor.Position + pos;
            transform1.eulerAngles = Camera.main.transform.eulerAngles;
            transform1.localScale = anchor.Scale;
            if (withOpen)
            {
                base.Open();

            }
        }
    }
}
