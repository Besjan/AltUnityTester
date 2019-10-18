﻿namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class ClickEventCommand: Command
    {
        AltUnityObject altUnityObject;

        public ClickEventCommand(AltUnityObject altObject)
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
