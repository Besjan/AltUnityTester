﻿namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class DropObject: Command
    {
        UnityEngine.Vector2 position;
        AltUnityObject altUnityObject;

        public DropObject(UnityEngine.Vector2 position, AltUnityObject altUnityObject)
        {
            this.position = position;
            this.altUnityObject = altUnityObject;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("Drop object: " + altUnityObject);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            UnityEngine.Debug.Log("GameOBject: " + gameObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dropHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            return response;
        }
    }
}
