using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VFXParamMap.Editor
{
    /// <summary>
    /// Copies each ShurikenParamMap's systems and parameters from Play Mode
    /// back onto the same scene instance when Play Mode ends, unless keepPlayModeChanges is off.
    /// Objects are matched by scene path plus a sibling-index/name path, since instance IDs change.
    /// </summary>
    [InitializeOnLoad]
    static class ShurikenParamPlayModeKeeper
    {
        const string SessionKey = "VFXParamMap.PlayModeSnapshot";

        [Serializable]
        class SystemRecord
        {
            public string systemPath;
            public List<ShurikenParam> parameters = new List<ShurikenParam>();
        }

        [Serializable]
        class MapRecord
        {
            public string scenePath;
            public string mapPath;
            public List<SystemRecord> systems = new List<SystemRecord>();
        }

        [Serializable]
        class Snapshot
        {
            public List<MapRecord> maps = new List<MapRecord>();
        }

        static ShurikenParamPlayModeKeeper()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        static void OnPlayModeChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.ExitingPlayMode) Capture();
            else if (change == PlayModeStateChange.EnteredEditMode) Restore();
        }

        static void Capture()
        {
            var snapshot = new Snapshot();
            foreach (var map in UnityEngine.Object.FindObjectsByType<ShurikenParamMap>(FindObjectsInactive.Include))
            {
                if (!map.keepPlayModeChanges || !map.gameObject.scene.IsValid() || string.IsNullOrEmpty(map.gameObject.scene.path))
                    continue;
                snapshot.maps.Add(ToRecord(map));
            }
            SessionState.SetString(SessionKey, snapshot.maps.Count > 0 ? JsonUtility.ToJson(snapshot) : "");
        }

        static void Restore()
        {
            var json = SessionState.GetString(SessionKey, "");
            SessionState.EraseString(SessionKey);
            if (string.IsNullOrEmpty(json)) return;

            var snapshot = JsonUtility.FromJson<Snapshot>(json);
            var kept = new List<string>();
            foreach (var record in snapshot.maps)
            {
                var scene = SceneManager.GetSceneByPath(record.scenePath);
                if (!scene.IsValid() || !scene.isLoaded) continue;
                var mapTransform = Resolve(scene, null, record.mapPath);
                var map = mapTransform != null ? mapTransform.GetComponent<ShurikenParamMap>() : null;
                if (map == null) continue;

                // Skip instances whose parameters did not change, so the scene is not dirtied for nothing.
                if (JsonUtility.ToJson(ToRecord(map)) == JsonUtility.ToJson(record)) continue;

                Undo.RecordObject(map, "Keep Play Mode changes");
                map.systems = new List<ShurikenSystemEntry>();
                foreach (var sr in record.systems)
                {
                    var t = string.IsNullOrEmpty(sr.systemPath) ? null : Resolve(scene, map.transform, sr.systemPath);
                    map.systems.Add(new ShurikenSystemEntry
                    {
                        system = t != null ? t.GetComponent<ParticleSystem>() : null,
                        parameters = sr.parameters,
                    });
                }
                PrefabUtility.RecordPrefabInstancePropertyModifications(map);
                EditorSceneManager.MarkSceneDirty(scene);
                kept.Add(map.name);
            }
            if (kept.Count > 0)
                Debug.Log("ShurikenParamMap: kept Play Mode changes on " + string.Join(", ", kept));
        }

        static MapRecord ToRecord(ShurikenParamMap map)
        {
            var record = new MapRecord { scenePath = map.gameObject.scene.path, mapPath = PathOf(map.transform, null) };
            foreach (var entry in map.systems)
            {
                record.systems.Add(new SystemRecord
                {
                    systemPath = entry.system != null ? PathOf(entry.system.transform, map.transform) : "",
                    parameters = entry.parameters,
                });
            }
            return record;
        }

        // Path segments are "siblingIndex:name". Relative to the map when the target sits under it,
        // otherwise from the scene root (prefixed with "/").
        static string PathOf(Transform t, Transform relativeTo)
        {
            var segments = new List<string>();
            var cur = t;
            while (cur != null && cur != relativeTo)
            {
                segments.Insert(0, cur.GetSiblingIndex() + ":" + cur.name);
                cur = cur.parent;
            }
            var path = string.Join("/", segments);
            return relativeTo != null && cur == null ? "/" + path : path;
        }

        static Transform Resolve(Scene scene, Transform relativeTo, string path)
        {
            if (string.IsNullOrEmpty(path)) return relativeTo;
            Transform cur = relativeTo;
            if (path.StartsWith("/"))
            {
                cur = null;
                path = path.Substring(1);
            }
            foreach (var segment in path.Split('/'))
            {
                int colon = segment.IndexOf(':');
                int index = int.Parse(segment.Substring(0, colon));
                string name = segment.Substring(colon + 1);
                Transform next;
                if (cur == null)
                {
                    var roots = scene.GetRootGameObjects();
                    next = index < roots.Length ? roots[index].transform : null;
                }
                else
                {
                    next = index < cur.childCount ? cur.GetChild(index) : null;
                }
                if (next == null || next.name != name) return null;
                cur = next;
            }
            return cur;
        }
    }
}
