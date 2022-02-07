using System;
using UnityEngine;

namespace RuntimeHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class PositionAxis : HandleBase
    {
        protected Vector3 _startPosition;
        protected Vector3 _interactionOffset;
        protected Vector3 _axis;
        protected Vector3 _perp;

        public PositionAxis Initialize(RuntimeTransformHandle p_runtimeHandle, Vector3 p_axis, Vector3 p_perp,
            Color p_color)
        {
            _parentTransformHandle = p_runtimeHandle;
            _axis = p_axis;
            _perp = p_perp;
            _defaultColor = p_color;
            
            InitializeMaterial();

            transform.SetParent(p_runtimeHandle.transform, false);

            GameObject o = new GameObject();
            o.transform.SetParent(transform, false);
            MeshRenderer mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            MeshFilter mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateCone(2f, .02f, .02f, 8, 1);
            MeshCollider mc = o.AddComponent<MeshCollider>();
            mc.sharedMesh = MeshUtils.CreateCone(2f, .1f, .02f, 8, 1);
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, p_axis);

            o = new GameObject();
            o.transform.SetParent(transform, false);
            mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateCone(.4f, .2f, .0f, 8, 1);
            mc = o.AddComponent<MeshCollider>();
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, _axis);
            o.transform.localPosition = p_axis * 2;

            return this;
        }

        /*public override void Interact(Vector3 p_previousPosition)
        {
            Vector3 mouseVector = (Input.mousePosition - p_previousPosition);
            float mag = mouseVector.magnitude;
            mouseVector = _parentTransformHandle.handleCamera.transform.rotation * mouseVector.normalized;
        
            Vector3 rperp = _parentTransformHandle.space == HandleSpace.LOCAL
                ? _parentTransformHandle.target.rotation * _perp
                : _perp;
            Vector3 projected = Vector3.ProjectOnPlane(mouseVector, rperp);
        
            projected *= Time.deltaTime * mag * RuntimeTransformHandle.MOUSE_SENSITIVITY;
            Vector3 raxis = _parentTransformHandle.space == HandleSpace.LOCAL
                ? _parentTransformHandle.target.rotation * _axis
                : _axis;
            float d = raxis.x * projected.x + raxis.y * projected.y + raxis.z * projected.z;
        
            delta += d;
            Vector3 snappingVector = _parentTransformHandle.positionSnap;
            float snap = Vector3.Scale(snappingVector, _axis).magnitude;
            float snappedDelta = (snap == 0 || _parentTransformHandle.snappingType == HandleSnappingType.ABSOLUTE) ? delta : Mathf.Round(delta / snap) * snap;
            Vector3 position = _startPosition + raxis * snappedDelta;
            if (snap != 0 && _parentTransformHandle.snappingType == HandleSnappingType.ABSOLUTE)
            {
                if (snappingVector.x != 0) position.x = Mathf.Round(position.x / snappingVector.x) * snappingVector.x;
                if (snappingVector.y != 0) position.y = Mathf.Round(position.y / snappingVector.y) * snappingVector.y;
                if (snappingVector.z != 0) position.z = Mathf.Round(position.z / snappingVector.z) * snappingVector.z;
            }
            
            _parentTransformHandle.target.position = position;
        
            base.Interact(p_previousPosition);
        }
        
        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startPosition = _parentTransformHandle.target.position;
        }*/

        public override void Interact(Vector3 p_previousPosition)
        {
            Vector3 raxis = _parentTransformHandle.space == HandleSpace.LOCAL
                ? _parentTransformHandle.target.rotation * _axis
                : _axis;

            const float choosenCameraPlane = 50f;
            Vector3 mousePosition = Input.mousePosition;
            Camera handleCamera = _parentTransformHandle.handleCamera;

            Ray mouseRay = handleCamera.ScreenPointToRay(mousePosition);
            Vector3 firstPlaneVector = mouseRay.GetPoint(1f) - mouseRay.GetPoint(0f);

            Vector3 axisBaseOfObjectOnScreen = handleCamera.WorldToScreenPoint(_parentTransformHandle.target.position);
            axisBaseOfObjectOnScreen.z = choosenCameraPlane;
            Vector3 axisTipOfObjectOnScreen = handleCamera.WorldToScreenPoint(_parentTransformHandle.target.position + raxis);
            axisBaseOfObjectOnScreen.z = choosenCameraPlane;
            Vector3 axisVectorOfObjectOnScreen = axisTipOfObjectOnScreen - axisBaseOfObjectOnScreen;
            Vector3 axisBaseOfMouseInWorld = handleCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, choosenCameraPlane));
            Vector3 axisTipOfMouseInWorld = handleCamera.ScreenToWorldPoint(new Vector3(mousePosition.x + axisVectorOfObjectOnScreen.x, mousePosition.y + axisVectorOfObjectOnScreen.y, choosenCameraPlane));
            Vector3 axisVectorOfMouseInWorld = axisTipOfMouseInWorld - axisBaseOfMouseInWorld;
            Vector3 secondPlaneVector = Vector3.Cross(firstPlaneVector, axisVectorOfMouseInWorld);
            Plane plane = new Plane(mouseRay.GetPoint(0f), mouseRay.GetPoint(0f) + firstPlaneVector, mouseRay.GetPoint(0f) + secondPlaneVector);
            Ray axisRay = new Ray(_startPosition, raxis);
            float d = 0.0f;
            plane.Raycast(axisRay, out d);

            Vector3 hitPoint = axisRay.GetPoint(d);

            Vector3 hitPointOnScreen = handleCamera.WorldToScreenPoint(hitPoint);
            if (hitPointOnScreen.z <= handleCamera.nearClipPlane || hitPointOnScreen.z >= handleCamera.farClipPlane) return;

            Vector3 offset = hitPoint + _interactionOffset - _startPosition;

            Vector3 snapping = _parentTransformHandle.positionSnap;
            float snap = Vector3.Scale(snapping, raxis).magnitude;
            if (snap != 0 && _parentTransformHandle.snappingType == HandleSnappingType.RELATIVE)
            {
                if (snapping.x != 0) offset.x = Mathf.Round(offset.x / snapping.x) * snapping.x;
                if (snapping.y != 0) offset.y = Mathf.Round(offset.y / snapping.y) * snapping.y;
                if (snapping.z != 0) offset.z = Mathf.Round(offset.z / snapping.z) * snapping.z;
            }

            Vector3 position = _startPosition + offset;

            if (snap != 0 && _parentTransformHandle.snappingType == HandleSnappingType.ABSOLUTE)
            {
                if (snapping.x != 0) position.x = Mathf.Round(position.x / snapping.x) * snapping.x;
                if (snapping.y != 0) position.y = Mathf.Round(position.y / snapping.y) * snapping.y;
                if (snapping.x != 0) position.z = Mathf.Round(position.z / snapping.z) * snapping.z;
            }

            _parentTransformHandle.target.position = position;

            base.Interact(p_previousPosition);
        }

        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startPosition = _parentTransformHandle.target.position;
            _interactionOffset = _startPosition - p_hitPoint;
        }
    }
}