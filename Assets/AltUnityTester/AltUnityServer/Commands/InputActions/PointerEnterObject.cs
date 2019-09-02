﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class PointerEnterObject:Command
    {
        AltUnityObject altUnityObject;

        public PointerEnterObject(AltUnityObject altUnityObject)
        {
            this.altUnityObject = altUnityObject;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("PointerEnter object: " + altUnityObject);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            UnityEngine.Debug.Log("GameOBject: " + gameObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            return response;
        }
    }
}
