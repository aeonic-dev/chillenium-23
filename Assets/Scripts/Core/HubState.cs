using System;
using Gameplay;
using Gameplay.Interactions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core {
    [Serializable]
    public struct HubState {
        public string playerData;
        public string playerPosition;
        public string playerVelocity;
        public SerializedMap<string, string> presentObjectives;

        public static HubState Empty = new();
        
/*
             __             _,-"~^"-.
       _// )      _,-"~`         `.
     ." ( /`"-,-"`                 ;
    / 6                             ;
   /           ,             ,-"     ;
  (,__.--.      \           /        ;
   //'   /`-.\   |          |        `._________
     _.-'_/`  )  )--...,,,___\     \-----------,)
   ((("~` _.-'.-'           __`-.   )         //
     jgs ((("`             (((---~"`         //
                                            ((________________
                                            `----""""~~~~^^^```
*/

        public static HubState Collect() {
            var state = new HubState {
                playerData = JsonUtility.ToJson(Player.Get()),
                playerPosition = JsonUtility.ToJson(Player.Get().transform.position),
                playerVelocity = JsonUtility.ToJson(Player.Get().GetComponent<Rigidbody2D>().velocity),
                presentObjectives = new()
            };
            foreach (PresentObjectiveInteraction obj in Object.FindObjectsOfType<PresentObjectiveInteraction>()) {
                state.presentObjectives.Set(obj.objective.objectiveName, JsonUtility.ToJson(obj));
            }
            
            return state;
        }
        
        public static void Restore(HubState state) {
            var player = Player.Get();
            
            JsonUtility.FromJsonOverwrite(state.playerData, player);
            player.transform.position = JsonUtility.FromJson<Vector3>(state.playerPosition);
            player.GetComponent<Rigidbody2D>().velocity = JsonUtility.FromJson<Vector2>(state.playerVelocity);
            
            foreach (PresentObjectiveInteraction obj in Object.FindObjectsOfType<PresentObjectiveInteraction>()) {
                if (state.presentObjectives.Contains(obj.objective.objectiveName)) {
                    string data = state.presentObjectives.Get(obj.objective.objectiveName);
                    if (data != null) JsonUtility.FromJsonOverwrite(data, obj);
                }
            }
        }
    }
}
