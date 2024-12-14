using System;
using System.Collections;
using System.Collections.Generic;
using OVR.OpenVR;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EmbroideryController : MonoBehaviour
{
   public StitchRenderer stitchRenderer;
   [SerializeField] private GameObject _needle;
   [SerializeField] private float _offset = 0.0005f;
   [SerializeField] GameObject pointPrefab;
   private Vector3? _start;
   private Vector3? _end;
   private Color _color;
   private List<GameObject> _stithes = new List<GameObject>();
   private GameObject _startPointVisual;
   private float _waiter = 0;
   private void Update()
   {
      _waiter =+ Time.deltaTime;
      _color = stitchRenderer._lineMaterial.color;
     // GetPoints();
    
         CreateStitch();
      
   }

   private void CreateStitch()
   {
      if (_start.HasValue && _end.HasValue)
      {
         Vector3 surfaceNormal = transform.forward;
         Vector3 start = _start.Value + (-surfaceNormal * _offset);
         if (_startPointVisual != null)
         {
            Destroy(_startPointVisual);
         }
         _startPointVisual = Instantiate(pointPrefab, start, Quaternion.identity);
         Vector3 end = _end.Value + (-surfaceNormal * _offset);
         Color stitchColor = _color;
         GameObject newStitch = stitchRenderer.CreateStitch(start, end, stitchColor);
         if (newStitch != null)
         {
            Destroy(_startPointVisual);
            _stithes.Add(newStitch);
         }

        
         _start = null;
         _end = null;
      }
   }
/*
   private Vector3 GetCentralPoint(BoxCollider collider)
   {
      Vector3 localBottomCenter = new Vector3(0, -collider.size.y / 2, 0);
      return transform.TransformPoint(collider.center+localBottomCenter);
   }
*/
   private void OnTriggerEnter(Collider other)
   {
      RaycastHit hit;
      // Cast a ray from the needle downward to find the surface point
      if (Physics.Raycast(_needle.transform.position, -_needle.transform.up, out hit, 0.2f))
      {
         if (_start == null)
         {
            _start = hit.point;
         }
         else
         {
            _end = hit.point;
         }
      }

   }
/*
   private void GetPoints()
   {
      
      Vector3 bottomCenter = GetCentralPoint(this.GetComponent<BoxCollider>());
      if (Physics.Raycast(bottomCenter, -transform.up, out RaycastHit hitInfo, raylength, layerMask))
      {
         if (_start == null)
         {
            _start = hitInfo.point;
            _waiter = 0f;
         }
         else if (_end == null&& _waiter > 0.5f)
         {
            _end = hitInfo.point;
         }
      } ;
      Debug.DrawRay(bottomCenter, -transform.up * raylength, Color.red, 5f);
      
   }

  */
}
