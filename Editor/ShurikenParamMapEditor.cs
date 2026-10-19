using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace VFXParamMap.Editor
{
    /// <summary>
    /// UI Toolkit inspector for ShurikenParamMap, built entirely in C# with inline styles.
    /// Particle-system first: add systems from under the FX root, then add parameters to each.
    /// </summary>
    [CustomEditor(typeof(ShurikenParamMap))]
    public class ShurikenParamMapEditor : UnityEditor.Editor
    {
        ShurikenParamMap map;
        VisualElement systemsContainer;
        string structure;

        static bool Dark => EditorGUIUtility.isProSkin;

        public override VisualElement CreateInspectorGUI()
        {
            map = (ShurikenParamMap)target;
            var root = new VisualElement();

            var settings = Box(0);
            settings.Add(new PropertyField(serializedObject.FindProperty(nameof(ShurikenParamMap.keepPlayModeChanges))));
            settings.Add(new PropertyField(serializedObject.FindProperty(nameof(ShurikenParamMap.enableTargetModulesOnApply))));
            root.Add(settings);

            var header = Row();
            header.style.marginTop = 6;
            header.style.marginBottom = 2;
            var title = new Label("Particle Systems");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.flexGrow = 1;
            header.Add(title);
            Button addSystem = null;
            addSystem = new Button(() => ShowAddSystemMenu(addSystem)) { text = "Add System", tooltip = "Add a particle system from under this FX root." };
            header.Add(addSystem);
            root.Add(header);

            systemsContainer = new VisualElement();
            root.Add(systemsContainer);

            var resetAll = new Button(() => Modify("Reset all Shuriken parameters", () =>
            {
                if (Application.isPlaying) map.ResetAll();
                else foreach (var e in map.systems) foreach (var p in e.parameters) Seed(e.system, p);
            })) { text = "Reset all", tooltip = "Play Mode: restore authored values on all systems. Edit Mode: reload every value from its system." };
            resetAll.style.marginTop = 6;
            root.Add(resetAll);

            Rebuild();

            // Value edits re-apply in Play Mode; structural changes (including Undo) rebuild the list.
            root.TrackSerializedObjectValue(serializedObject, _ =>
            {
                map.MarkDirty();
                if (Structure() != structure) Rebuild();
            });
            return root;
        }

        // ---------- building ----------

        void Rebuild()
        {
            serializedObject.Update();
            structure = Structure();
            systemsContainer.Clear();

            if (map.systems.Count == 0)
                systemsContainer.Add(Muted("No particle systems yet. Use Add System."));

            for (int i = 0; i < map.systems.Count; i++)
                systemsContainer.Add(BuildSystem(i));
        }

        VisualElement BuildSystem(int i)
        {
            var entry = map.systems[i];
            var box = Box(1);
            string key = FoldoutKey(entry);

            var header = Row();
            var systemFoldout = HeaderFoldout(entry.system != null ? entry.system.name : "(Missing)", key + ".system");
            systemFoldout.tooltip = entry.system != null ? RelativePath(entry.system.transform) : "";
            header.Add(systemFoldout);

            var summary = Muted(entry.parameters.Count == 1 ? "1 parameter" : entry.parameters.Count + " parameters");
            header.Add(summary);

            Button addParam = null;
            addParam = new Button(() =>
            {
                // Adding a parameter should show it, so open both levels.
                SessionState.SetBool(key + ".system", true);
                SessionState.SetBool(key + ".parameters", true);
                ShowAddParameterMenu(addParam, i);
            }) { text = "Add Parameter" };
            addParam.SetEnabled(entry.system != null);
            header.Add(addParam);
            header.Add(new Button(() => RemoveSystem(i)) { text = "\u00d7", tooltip = "Remove this system (and its parameters)." });
            box.Add(header);

            var body = new VisualElement();
            box.Add(body);
            BindFoldout(systemFoldout, body, () => summary.style.display = systemFoldout.value ? DisplayStyle.None : DisplayStyle.Flex);

            if (entry.system == null)
                body.Add(new HelpBox("This particle system no longer exists. Remove it or add another.", HelpBoxMessageType.Warning));

            var parameters = Box(2);
            parameters.style.marginTop = 4;
            var parametersFoldout = HeaderFoldout($"Parameters ({entry.parameters.Count})", key + ".parameters");
            parameters.Add(parametersFoldout);
            var list = new VisualElement();
            parameters.Add(list);
            BindFoldout(parametersFoldout, list, null);

            if (entry.parameters.Count == 0)
                list.Add(Muted("No parameters. Use Add Parameter."));
            for (int j = 0; j < entry.parameters.Count; j++)
                list.Add(BuildParameter(i, j));
            body.Add(parameters);
            return box;
        }

        /// <summary>A Foldout used only as a clickable header; its open state is kept for the editor session.</summary>
        static Foldout HeaderFoldout(string text, string stateKey)
        {
            var foldout = new Foldout { text = text, value = SessionState.GetBool(stateKey, true) };
            foldout.style.flexGrow = 1;
            foldout.Q<Toggle>().style.unityFontStyleAndWeight = FontStyle.Bold;
            foldout.Q<Toggle>().style.marginLeft = 0;
            foldout.contentContainer.style.display = DisplayStyle.None;
            foldout.userData = stateKey;
            return foldout;
        }

        static void BindFoldout(Foldout foldout, VisualElement content, Action changed)
        {
            void Sync()
            {
                content.style.display = foldout.value ? DisplayStyle.Flex : DisplayStyle.None;
                changed?.Invoke();
            }
            foldout.RegisterValueChangedCallback(e =>
            {
                if (e.target != foldout) return;
                SessionState.SetBool((string)foldout.userData, e.newValue);
                Sync();
            });
            Sync();
        }

        string FoldoutKey(ShurikenSystemEntry entry)
        {
            string system = entry.system != null ? RelativePath(entry.system.transform) : "missing";
            return $"VFXParamMap.Fold.{map.gameObject.scene.path}.{RelativePathFromScene(map.transform)}.{system}";
        }

        static string RelativePathFromScene(Transform t)
        {
            var path = t.name;
            for (var cur = t.parent; cur != null; cur = cur.parent) path = cur.name + "/" + path;
            return path;
        }
        VisualElement BuildParameter(int i, int j)
        {
            var entry = map.systems[i];
            var param = entry.parameters[j];
            var box = Box(3);

            var header = Row();
            var name = new Label(DisplayName(param.attribute).Replace("/", " › "));
            name.style.unityFontStyleAndWeight = FontStyle.Bold;
            name.style.flexGrow = 1;
            header.Add(name);
            header.Add(new Button(() => ResetParameter(i, j))
            {
                text = "Reset",
                tooltip = "Play Mode: restore the authored value on the system. Edit Mode: reload the value from the system.",
            });
            header.Add(new Button(() => RemoveParameter(i, j)) { text = "×", tooltip = "Remove this parameter." });
            box.Add(header);

            var valueProp = serializedObject.FindProperty(nameof(ShurikenParamMap.systems))
                .GetArrayElementAtIndex(i).FindPropertyRelative(nameof(ShurikenSystemEntry.parameters))
                .GetArrayElementAtIndex(j).FindPropertyRelative(nameof(ShurikenParam.value))
                .FindPropertyRelative(ValueField(param.attribute));
            box.Add(ValueField(valueProp, ShurikenAttributes.IsAngular(param.attribute) ? "Value (deg/s)" : "Value"));

            var help = new HelpBox("", HelpBoxMessageType.Warning);
            help.style.display = DisplayStyle.None;
            box.Add(help);
            box.schedule.Execute(() =>
            {
                var ps = entry.system;
                var module = ShurikenAttributes.ModuleOf(param.attribute);
                bool off = ps != null && !map.enableTargetModulesOnApply && !ShurikenAttributes.IsEnabledToggle(param.attribute)
                           && !ShurikenAttributes.IsModuleEnabled(ps, module);
                help.text = off
                    ? $"{ObjectNames.NicifyVariableName(module.ToString())} is off on this system, so this value has no visible effect. Turn on Enable Target Modules On Apply, or enable the module on the system."
                    : "";
                help.style.display = off ? DisplayStyle.Flex : DisplayStyle.None;
            }).Every(500);
            return box;
        }

        static PropertyField ValueField(SerializedProperty prop, string label)
        {
            var pf = new PropertyField(prop, label);
            pf.BindProperty(prop);
            // The built-in MinMaxCurve drawer labels some modes with the field name and rebuilds its
            // fields when the curve mode changes, so keep relabelling them.
            string fieldName = prop.name, fieldLabel = prop.displayName;
            pf.schedule.Execute(() => pf.Query<Label>().ForEach(l =>
            {
                if (l.text == fieldName || l.text == fieldLabel) l.text = label;
            })).Every(250);
            return pf;
        }

        // ---------- menus ----------

        void ShowAddSystemMenu(VisualElement anchor)
        {
            var menu = new GenericMenu();
            var selected = Selection.GetFiltered<ParticleSystem>(SelectionMode.Editable);
            if (selected.Length > 0)
            {
                menu.AddItem(new GUIContent($"Selected in Hierarchy ({selected.Length})"), false, () => AddSystems(selected));
                menu.AddSeparator("");
            }

            var all = map.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in all)
            {
                // GenericMenu treats '/' as a submenu, so show the path with a look-alike separator.
                var label = new GUIContent(RelativePath(ps.transform).Replace("/", " ∕ "));
                if (map.Find(ps) != null) menu.AddDisabledItem(label, true);
                else menu.AddItem(label, false, () => AddSystems(new[] { ps }));
            }
            if (all.Length == 0) menu.AddDisabledItem(new GUIContent("No particle systems under this object"));
            else
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("All"), false, () => AddSystems(all));
            }
            menu.DropDown(anchor.worldBound);
        }

        void ShowAddParameterMenu(VisualElement anchor, int i)
        {
            var entry = map.systems[i];
            var menu = new GenericMenu();
            var attributes = (ShurikenAttribute[])Enum.GetValues(typeof(ShurikenAttribute));
            var ordered = attributes.OrderBy(a => Array.IndexOf(ShurikenAttributes.MenuOrder, ShurikenAttributes.ModuleOf(a))).ThenBy(a => (int)a);
            foreach (var attr in ordered)
            {
                var label = new GUIContent(DisplayName(attr));
                if (entry.Find(attr) != null) menu.AddDisabledItem(label, true);
                else menu.AddItem(label, false, () => AddParameter(i, attr));
            }
            menu.DropDown(anchor.worldBound);
        }

        // ---------- actions ----------

        void AddSystems(ParticleSystem[] systems) => Modify("Add particle system", () =>
        {
            foreach (var ps in systems)
                if (map.Find(ps) == null) map.systems.Add(new ShurikenSystemEntry { system = ps });
        });

        void RemoveSystem(int i) => Modify("Remove particle system", () =>
        {
            var entry = map.systems[i];
            if (Application.isPlaying)
                foreach (var p in entry.parameters) map.Restore(entry.system, p.attribute);
            map.systems.RemoveAt(i);
        });

        void AddParameter(int i, ShurikenAttribute attr) => Modify("Add Shuriken parameter", () =>
        {
            var entry = map.systems[i];
            var p = new ShurikenParam { attribute = attr };
            Seed(entry.system, p);
            entry.parameters.Add(p);
        });

        void RemoveParameter(int i, int j) => Modify("Remove Shuriken parameter", () =>
        {
            var entry = map.systems[i];
            if (Application.isPlaying) map.Restore(entry.system, entry.parameters[j].attribute);
            entry.parameters.RemoveAt(j);
        });

        void ResetParameter(int i, int j) => Modify("Reset Shuriken parameter", () =>
        {
            var entry = map.systems[i];
            var p = entry.parameters[j];
            if (Application.isPlaying) map.ResetParam(entry.system, p);
            else Seed(entry.system, p);
        });

        void Modify(string undoName, Action change)
        {
            Undo.RecordObject(map, undoName);
            change();
            PrefabUtility.RecordPrefabInstancePropertyModifications(map);
            EditorUtility.SetDirty(map);
            map.MarkDirty();
            Rebuild();
        }

        /// <summary>Fills the value from the system's current setting (its authored setting in Play Mode).</summary>
        void Seed(ParticleSystem ps, ShurikenParam p)
        {
            if (ps == null) return;
            p.value = Application.isPlaying ? map.ReadOriginal(ps, p.attribute) : ShurikenAttributes.Read(ps, p.attribute);
        }

        // ---------- helpers ----------

        string Structure()
        {
            var sb = new StringBuilder();
            foreach (var e in map.systems)
            {
                sb.Append(e.system != null ? e.system.GetHashCode() : 0).Append(':');
                foreach (var p in e.parameters) sb.Append((int)p.attribute).Append(',');
                sb.Append(';');
            }
            return sb.ToString();
        }

        string RelativePath(Transform t)
        {
            if (t == map.transform) return t.name;
            var path = t.name;
            for (var cur = t.parent; cur != null && cur != map.transform; cur = cur.parent)
                path = cur.name + "/" + path;
            return path;
        }

        static string DisplayName(ShurikenAttribute attr)
        {
            var field = typeof(ShurikenAttribute).GetField(attr.ToString());
            var inspectorName = field?.GetCustomAttribute<InspectorNameAttribute>();
            return inspectorName != null ? inspectorName.displayName : ObjectNames.NicifyVariableName(attr.ToString());
        }

        static string ValueField(ShurikenAttribute attr)
        {
            switch (ShurikenAttributes.KindOf(attr))
            {
                case ShurikenValueKind.Bool: return nameof(ShurikenValue.toggle);
                case ShurikenValueKind.Float: return nameof(ShurikenValue.number);
                case ShurikenValueKind.Gradient: return nameof(ShurikenValue.gradient);
                case ShurikenValueKind.Space: return nameof(ShurikenValue.space);
                case ShurikenValueKind.Range: return nameof(ShurikenValue.range);
                default: return nameof(ShurikenValue.curve);
            }
        }

        static VisualElement Row()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            return row;
        }

        static Label Muted(string text)
        {
            var l = new Label(text);
            l.style.color = Dark ? new Color(1, 1, 1, 0.45f) : new Color(0, 0, 0, 0.45f);
            l.style.unityFontStyleAndWeight = FontStyle.Italic;
            l.style.paddingLeft = 2;
            l.style.paddingTop = 2;
            l.style.paddingBottom = 2;
            return l;
        }

        /// <summary>A bordered panel; deeper levels get a slightly stronger tint so nesting reads clearly.</summary>
        static VisualElement Box(int level)
        {
            var e = new VisualElement();
            float tint = 0.03f * level;
            e.style.backgroundColor = Dark ? new Color(1, 1, 1, tint) : new Color(0, 0, 0, tint);
            var border = Dark ? new Color(0, 0, 0, 0.55f) : new Color(0, 0, 0, 0.2f);
            e.style.borderTopColor = e.style.borderBottomColor = e.style.borderLeftColor = e.style.borderRightColor = border;
            e.style.borderTopWidth = e.style.borderBottomWidth = e.style.borderLeftWidth = e.style.borderRightWidth = 1;
            e.style.borderTopLeftRadius = e.style.borderTopRightRadius = e.style.borderBottomLeftRadius = e.style.borderBottomRightRadius = 3;
            e.style.paddingLeft = e.style.paddingRight = 4;
            e.style.paddingTop = e.style.paddingBottom = 3;
            e.style.marginTop = level == 0 ? 0 : 2;
            e.style.marginBottom = 2;
            return e;
        }
    }
}
