using System.Collections.Generic;
using UnityEngine;

namespace VFXParamMap
{
    /// <summary>
    /// Lives on an FX root. The artist adds particle systems, then the attributes to control on each;
    /// every value is written directly onto its system. Values are applied in Play Mode and in builds;
    /// the authored Shuriken data is never modified outside Play Mode.
    /// </summary>
    [DisallowMultipleComponent]
    public class ShurikenParamMap : MonoBehaviour
    {
        [Tooltip("When leaving Play Mode, copy this instance's parameter values back onto it in the scene.")]
        public bool keepPlayModeChanges = true;

        [Tooltip("When a parameter is applied, turn on its module (e.g. Size over Lifetime) on the system if it is off, so the value is visible. Reset turns it back off.")]
        public bool enableTargetModulesOnApply = true;

        public List<ShurikenSystemEntry> systems = new List<ShurikenSystemEntry>();

        // Authored values captured before the first write, used only to restore them.
        readonly Dictionary<(ParticleSystem, ShurikenAttribute), ShurikenValue> originals =
            new Dictionary<(ParticleSystem, ShurikenAttribute), ShurikenValue>();

        bool dirty;

        void OnEnable() => ApplyAll();

        void OnValidate()
        {
            if (Application.isPlaying) dirty = true;
        }

        void LateUpdate()
        {
            if (!dirty) return;
            dirty = false;
            ApplyAll();
        }

        /// <summary>Marks the parameters as changed so they are written at the end of this frame.</summary>
        public void MarkDirty() => dirty = true;

        public void ApplyAll()
        {
            if (!Application.isPlaying) return;
            foreach (var entry in systems)
                foreach (var p in entry.parameters)
                    Apply(entry.system, p);
        }

        public void Apply(ParticleSystem ps, ShurikenParam p)
        {
            if (!Application.isPlaying || ps == null || p == null) return;
            Capture(ps, p.attribute);
            ShurikenAttributes.Write(ps, p.attribute, p.value);

            if (enableTargetModulesOnApply && !ShurikenAttributes.IsEnabledToggle(p.attribute))
            {
                var module = ShurikenAttributes.ModuleOf(p.attribute);
                if (!ShurikenAttributes.IsModuleEnabled(ps, module))
                {
                    Capture(ps, ShurikenAttributes.EnabledAttributeOf(module));
                    ShurikenAttributes.SetModuleEnabled(ps, module, true);
                }
            }
        }

        void Capture(ParticleSystem ps, ShurikenAttribute attribute)
        {
            var key = (ps, attribute);
            if (!originals.ContainsKey(key))
                originals[key] = ShurikenAttributes.Read(ps, attribute);
        }

        /// <summary>The authored value of an attribute on a system, even after it has been overridden.</summary>
        public ShurikenValue ReadOriginal(ParticleSystem ps, ShurikenAttribute attribute)
        {
            return originals.TryGetValue((ps, attribute), out var v) ? v : ShurikenAttributes.Read(ps, attribute);
        }

        /// <summary>Writes the authored value (and module enabled state, if this map turned it on) back onto the system.</summary>
        public void Restore(ParticleSystem ps, ShurikenAttribute attribute)
        {
            if (ps == null) return;
            if (originals.TryGetValue((ps, attribute), out var v))
            {
                ShurikenAttributes.Write(ps, attribute, v);
                originals.Remove((ps, attribute));
            }
            var enabled = ShurikenAttributes.EnabledAttributeOf(ShurikenAttributes.ModuleOf(attribute));
            if (enabled != attribute && originals.TryGetValue((ps, enabled), out var wasEnabled))
            {
                ShurikenAttributes.Write(ps, enabled, wasEnabled);
                originals.Remove((ps, enabled));
            }
        }

        /// <summary>Restores the authored value and reloads the parameter's value from the system.</summary>
        public void ResetParam(ParticleSystem ps, ShurikenParam p)
        {
            if (ps == null || p == null) return;
            Restore(ps, p.attribute);
            p.value = ShurikenAttributes.Read(ps, p.attribute);
        }

        public void ResetAll()
        {
            foreach (var entry in systems)
                foreach (var p in entry.parameters)
                    ResetParam(entry.system, p);
        }

        public ShurikenSystemEntry Find(ParticleSystem ps) => systems.Find(e => e.system == ps);
        public ShurikenSystemEntry Find(string systemName) => systems.Find(e => e.system != null && e.system.name == systemName);

        public bool Set(string systemName, ShurikenAttribute attribute, bool value) => Set(systemName, attribute, p => p.value.toggle = value);
        public bool Set(string systemName, ShurikenAttribute attribute, float value) => Set(systemName, attribute, p => p.value.number = value);
        public bool Set(string systemName, ShurikenAttribute attribute, ParticleSystem.MinMaxCurve value) => Set(systemName, attribute, p => p.value.curve = value);
        public bool Set(string systemName, ShurikenAttribute attribute, ParticleSystem.MinMaxGradient value) => Set(systemName, attribute, p => p.value.gradient = value);
        public bool Set(string systemName, ShurikenAttribute attribute, ParticleSystemSimulationSpace value) => Set(systemName, attribute, p => p.value.space = value);

        bool Set(string systemName, ShurikenAttribute attribute, System.Action<ShurikenParam> assign)
        {
            var entry = Find(systemName);
            var p = entry?.Find(attribute);
            if (p == null) return false;
            assign(p);
            Apply(entry.system, p);
            return true;
        }
    }
}
