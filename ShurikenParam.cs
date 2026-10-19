using System;
using System.Collections.Generic;
using UnityEngine;

namespace VFXParamMap
{
    /// <summary>Holds one value of any supported kind; only the field matching the attribute's kind is used.</summary>
    [Serializable]
    public struct ShurikenValue
    {
        public bool toggle;
        public float number;
        public ParticleSystem.MinMaxCurve curve;
        public ParticleSystem.MinMaxGradient gradient;
        public ParticleSystemSimulationSpace space;
        public Vector2 range;
    }

    /// <summary>One attribute of a particle system and the value set directly onto it.</summary>
    [Serializable]
    public class ShurikenParam
    {
        public ShurikenAttribute attribute = ShurikenAttribute.SizeOverLifetime_Size;
        public ShurikenValue value;

        public ShurikenValueKind Kind => ShurikenAttributes.KindOf(attribute);
    }

    /// <summary>A particle system the artist added, with the parameters controlled on it.</summary>
    [Serializable]
    public class ShurikenSystemEntry
    {
        public ParticleSystem system;
        public List<ShurikenParam> parameters = new List<ShurikenParam>();

        public ShurikenParam Find(ShurikenAttribute attribute) => parameters.Find(p => p.attribute == attribute);
    }
}
