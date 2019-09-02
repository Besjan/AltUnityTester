﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class ClickEvent: Command
    {
        AltUnityObject altUnityObject;

        public ClickEvent(AltUnityObject altObject)
        {
            this.altUnityObject = altObject;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("ClickEvent on " + altUnityObject);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.GameObject foundGameObject = AltUnityRunner.GetGameObject(altUnityObject);
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.EventSystems.ExecuteEvents.Execute(foundGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject));
            return response;
        }
    }
}
