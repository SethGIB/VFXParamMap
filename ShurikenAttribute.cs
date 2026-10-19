using UnityEngine;

namespace VFXParamMap
{
    /// <summary>
    /// Shuriken module settings a ShurikenParam can drive. The hundreds digit is the module
    /// (see ShurikenModule) and x00 is always the module's Enabled toggle. Values are serialized
    /// as ints, so existing numbers must never change; add new ones at the end of a module.
    /// </summary>
    public enum ShurikenAttribute
    {
        [InspectorName("Color over Lifetime/Enabled")] ColorOverLifetime_Enabled = 0,
        [InspectorName("Color over Lifetime/Color")] ColorOverLifetime_Color = 1,

        [InspectorName("Size over Lifetime/Enabled")] SizeOverLifetime_Enabled = 100,
        [InspectorName("Size over Lifetime/Separate Axes")] SizeOverLifetime_SeparateAxes = 101,
        [InspectorName("Size over Lifetime/Size")] SizeOverLifetime_Size = 102,
        [InspectorName("Size over Lifetime/X")] SizeOverLifetime_X = 103,
        [InspectorName("Size over Lifetime/Y")] SizeOverLifetime_Y = 104,
        [InspectorName("Size over Lifetime/Z")] SizeOverLifetime_Z = 105,

        [InspectorName("Velocity over Lifetime/Enabled")] VelocityOverLifetime_Enabled = 200,
        [InspectorName("Velocity over Lifetime/Space")] VelocityOverLifetime_Space = 201,
        [InspectorName("Velocity over Lifetime/Linear X")] VelocityOverLifetime_X = 202,
        [InspectorName("Velocity over Lifetime/Linear Y")] VelocityOverLifetime_Y = 203,
        [InspectorName("Velocity over Lifetime/Linear Z")] VelocityOverLifetime_Z = 204,
        [InspectorName("Velocity over Lifetime/Orbital X")] VelocityOverLifetime_OrbitalX = 205,
        [InspectorName("Velocity over Lifetime/Orbital Y")] VelocityOverLifetime_OrbitalY = 206,
        [InspectorName("Velocity over Lifetime/Orbital Z")] VelocityOverLifetime_OrbitalZ = 207,
        [InspectorName("Velocity over Lifetime/Radial")] VelocityOverLifetime_Radial = 208,
        [InspectorName("Velocity over Lifetime/Speed Modifier")] VelocityOverLifetime_SpeedModifier = 209,
        [InspectorName("Velocity over Lifetime/Offset X")] VelocityOverLifetime_OffsetX = 210,
        [InspectorName("Velocity over Lifetime/Offset Y")] VelocityOverLifetime_OffsetY = 211,
        [InspectorName("Velocity over Lifetime/Offset Z")] VelocityOverLifetime_OffsetZ = 212,

        [InspectorName("Limit Velocity over Lifetime/Enabled")] LimitVelocity_Enabled = 300,
        [InspectorName("Limit Velocity over Lifetime/Separate Axes")] LimitVelocity_SeparateAxes = 301,
        [InspectorName("Limit Velocity over Lifetime/Space")] LimitVelocity_Space = 302,
        [InspectorName("Limit Velocity over Lifetime/Speed")] LimitVelocity_Limit = 303,
        [InspectorName("Limit Velocity over Lifetime/X")] LimitVelocity_X = 304,
        [InspectorName("Limit Velocity over Lifetime/Y")] LimitVelocity_Y = 305,
        [InspectorName("Limit Velocity over Lifetime/Z")] LimitVelocity_Z = 306,
        [InspectorName("Limit Velocity over Lifetime/Dampen")] LimitVelocity_Dampen = 307,
        [InspectorName("Limit Velocity over Lifetime/Drag")] LimitVelocity_Drag = 308,
        [InspectorName("Limit Velocity over Lifetime/Multiply by Size")] LimitVelocity_MultiplyDragBySize = 309,
        [InspectorName("Limit Velocity over Lifetime/Multiply by Velocity")] LimitVelocity_MultiplyDragByVelocity = 310,

        [InspectorName("Force over Lifetime/Enabled")] ForceOverLifetime_Enabled = 400,
        [InspectorName("Force over Lifetime/X")] ForceOverLifetime_X = 401,
        [InspectorName("Force over Lifetime/Y")] ForceOverLifetime_Y = 402,
        [InspectorName("Force over Lifetime/Z")] ForceOverLifetime_Z = 403,
        [InspectorName("Force over Lifetime/Space")] ForceOverLifetime_Space = 404,
        [InspectorName("Force over Lifetime/Randomize")] ForceOverLifetime_Randomize = 405,

        [InspectorName("Color by Speed/Enabled")] ColorBySpeed_Enabled = 500,
        [InspectorName("Color by Speed/Color")] ColorBySpeed_Color = 501,
        [InspectorName("Color by Speed/Speed Range")] ColorBySpeed_Range = 502,

        [InspectorName("Size by Speed/Enabled")] SizeBySpeed_Enabled = 600,
        [InspectorName("Size by Speed/Separate Axes")] SizeBySpeed_SeparateAxes = 601,
        [InspectorName("Size by Speed/Size")] SizeBySpeed_Size = 602,
        [InspectorName("Size by Speed/X")] SizeBySpeed_X = 603,
        [InspectorName("Size by Speed/Y")] SizeBySpeed_Y = 604,
        [InspectorName("Size by Speed/Z")] SizeBySpeed_Z = 605,
        [InspectorName("Size by Speed/Speed Range")] SizeBySpeed_Range = 606,

        [InspectorName("Rotation over Lifetime/Enabled")] RotationOverLifetime_Enabled = 700,
        [InspectorName("Rotation over Lifetime/Separate Axes")] RotationOverLifetime_SeparateAxes = 701,
        [InspectorName("Rotation over Lifetime/Angular Velocity (Z)")] RotationOverLifetime_Z = 702,
        [InspectorName("Rotation over Lifetime/X")] RotationOverLifetime_X = 703,
        [InspectorName("Rotation over Lifetime/Y")] RotationOverLifetime_Y = 704,

        [InspectorName("Rotation by Speed/Enabled")] RotationBySpeed_Enabled = 800,
        [InspectorName("Rotation by Speed/Separate Axes")] RotationBySpeed_SeparateAxes = 801,
        [InspectorName("Rotation by Speed/Angular Velocity (Z)")] RotationBySpeed_Z = 802,
        [InspectorName("Rotation by Speed/X")] RotationBySpeed_X = 803,
        [InspectorName("Rotation by Speed/Y")] RotationBySpeed_Y = 804,
        [InspectorName("Rotation by Speed/Speed Range")] RotationBySpeed_Range = 805,
    }

    public enum ShurikenValueKind { Bool, Float, Curve, Gradient, Space, Range }

    /// <summary>Module ids match the hundreds digit of ShurikenAttribute.</summary>
    public enum ShurikenModule
    {
        ColorOverLifetime = 0,
        SizeOverLifetime = 1,
        VelocityOverLifetime = 2,
        LimitVelocityOverLifetime = 3,
        ForceOverLifetime = 4,
        ColorBySpeed = 5,
        SizeBySpeed = 6,
        RotationOverLifetime = 7,
        RotationBySpeed = 8,
    }

    /// <summary>The attribute table: value kind, owning module, and direct read/write against a ParticleSystem.</summary>
    public static class ShurikenAttributes
    {
        /// <summary>Module order for menus, matching the V1 parameter list.</summary>
        public static readonly ShurikenModule[] MenuOrder =
        {
            ShurikenModule.VelocityOverLifetime,
            ShurikenModule.LimitVelocityOverLifetime,
            ShurikenModule.ForceOverLifetime,
            ShurikenModule.ColorOverLifetime,
            ShurikenModule.ColorBySpeed,
            ShurikenModule.SizeOverLifetime,
            ShurikenModule.SizeBySpeed,
            ShurikenModule.RotationOverLifetime,
            ShurikenModule.RotationBySpeed,
        };

        public static ShurikenModule ModuleOf(ShurikenAttribute a) => (ShurikenModule)((int)a / 100);

        public static bool IsEnabledToggle(ShurikenAttribute a) => (int)a % 100 == 0;

        public static ShurikenAttribute EnabledAttributeOf(ShurikenModule m) => (ShurikenAttribute)((int)m * 100);

        public static bool IsModuleEnabled(ParticleSystem ps, ShurikenModule m) => Read(ps, EnabledAttributeOf(m)).toggle;

        public static void SetModuleEnabled(ParticleSystem ps, ShurikenModule m, bool on) =>
            Write(ps, EnabledAttributeOf(m), new ShurikenValue { toggle = on });

        /// <summary>
        /// Rotation values are shown and stored in degrees per second, like Unity's own Particle System
        /// inspector; the Shuriken API works in radians, so Read/Write convert units (no other change).
        /// </summary>
        public static bool IsAngular(ShurikenAttribute a)
        {
            var m = ModuleOf(a);
            return (m == ShurikenModule.RotationOverLifetime || m == ShurikenModule.RotationBySpeed) && KindOf(a) == ShurikenValueKind.Curve;
        }

        public static ShurikenValueKind KindOf(ShurikenAttribute a)
        {
            if (IsEnabledToggle(a)) return ShurikenValueKind.Bool;
            switch (a)
            {
                case ShurikenAttribute.SizeOverLifetime_SeparateAxes:
                case ShurikenAttribute.LimitVelocity_SeparateAxes:
                case ShurikenAttribute.LimitVelocity_MultiplyDragBySize:
                case ShurikenAttribute.LimitVelocity_MultiplyDragByVelocity:
                case ShurikenAttribute.ForceOverLifetime_Randomize:
                case ShurikenAttribute.SizeBySpeed_SeparateAxes:
                case ShurikenAttribute.RotationOverLifetime_SeparateAxes:
                case ShurikenAttribute.RotationBySpeed_SeparateAxes:
                    return ShurikenValueKind.Bool;
                case ShurikenAttribute.LimitVelocity_Dampen:
                    return ShurikenValueKind.Float;
                case ShurikenAttribute.ColorOverLifetime_Color:
                case ShurikenAttribute.ColorBySpeed_Color:
                    return ShurikenValueKind.Gradient;
                case ShurikenAttribute.VelocityOverLifetime_Space:
                case ShurikenAttribute.LimitVelocity_Space:
                case ShurikenAttribute.ForceOverLifetime_Space:
                    return ShurikenValueKind.Space;
                case ShurikenAttribute.ColorBySpeed_Range:
                case ShurikenAttribute.SizeBySpeed_Range:
                case ShurikenAttribute.RotationBySpeed_Range:
                    return ShurikenValueKind.Range;
                default:
                    return ShurikenValueKind.Curve;
            }
        }

        public static ShurikenValue Read(ParticleSystem ps, ShurikenAttribute a)
        {
            var v = new ShurikenValue();
            switch (ModuleOf(a))
            {
                case ShurikenModule.ColorOverLifetime:
                {
                    var m = ps.colorOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.ColorOverLifetime_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.ColorOverLifetime_Color: v.gradient = m.color; break;
                    }
                    break;
                }
                case ShurikenModule.SizeOverLifetime:
                {
                    var m = ps.sizeOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.SizeOverLifetime_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.SizeOverLifetime_SeparateAxes: v.toggle = m.separateAxes; break;
                        case ShurikenAttribute.SizeOverLifetime_Size: v.curve = m.size; break;
                        case ShurikenAttribute.SizeOverLifetime_X: v.curve = m.x; break;
                        case ShurikenAttribute.SizeOverLifetime_Y: v.curve = m.y; break;
                        case ShurikenAttribute.SizeOverLifetime_Z: v.curve = m.z; break;
                    }
                    break;
                }
                case ShurikenModule.VelocityOverLifetime:
                {
                    var m = ps.velocityOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.VelocityOverLifetime_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.VelocityOverLifetime_Space: v.space = m.space; break;
                        case ShurikenAttribute.VelocityOverLifetime_X: v.curve = m.x; break;
                        case ShurikenAttribute.VelocityOverLifetime_Y: v.curve = m.y; break;
                        case ShurikenAttribute.VelocityOverLifetime_Z: v.curve = m.z; break;
                        case ShurikenAttribute.VelocityOverLifetime_OrbitalX: v.curve = m.orbitalX; break;
                        case ShurikenAttribute.VelocityOverLifetime_OrbitalY: v.curve = m.orbitalY; break;
                        case ShurikenAttribute.VelocityOverLifetime_OrbitalZ: v.curve = m.orbitalZ; break;
                        case ShurikenAttribute.VelocityOverLifetime_Radial: v.curve = m.radial; break;
                        case ShurikenAttribute.VelocityOverLifetime_SpeedModifier: v.curve = m.speedModifier; break;
                        case ShurikenAttribute.VelocityOverLifetime_OffsetX: v.curve = m.orbitalOffsetX; break;
                        case ShurikenAttribute.VelocityOverLifetime_OffsetY: v.curve = m.orbitalOffsetY; break;
                        case ShurikenAttribute.VelocityOverLifetime_OffsetZ: v.curve = m.orbitalOffsetZ; break;
                    }
                    break;
                }
                case ShurikenModule.LimitVelocityOverLifetime:
                {
                    var m = ps.limitVelocityOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.LimitVelocity_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.LimitVelocity_SeparateAxes: v.toggle = m.separateAxes; break;
                        case ShurikenAttribute.LimitVelocity_Space: v.space = m.space; break;
                        case ShurikenAttribute.LimitVelocity_Limit: v.curve = m.limit; break;
                        case ShurikenAttribute.LimitVelocity_X: v.curve = m.limitX; break;
                        case ShurikenAttribute.LimitVelocity_Y: v.curve = m.limitY; break;
                        case ShurikenAttribute.LimitVelocity_Z: v.curve = m.limitZ; break;
                        case ShurikenAttribute.LimitVelocity_Dampen: v.number = m.dampen; break;
                        case ShurikenAttribute.LimitVelocity_Drag: v.curve = m.drag; break;
                        case ShurikenAttribute.LimitVelocity_MultiplyDragBySize: v.toggle = m.multiplyDragByParticleSize; break;
                        case ShurikenAttribute.LimitVelocity_MultiplyDragByVelocity: v.toggle = m.multiplyDragByParticleVelocity; break;
                    }
                    break;
                }
                case ShurikenModule.ForceOverLifetime:
                {
                    var m = ps.forceOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.ForceOverLifetime_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.ForceOverLifetime_X: v.curve = m.x; break;
                        case ShurikenAttribute.ForceOverLifetime_Y: v.curve = m.y; break;
                        case ShurikenAttribute.ForceOverLifetime_Z: v.curve = m.z; break;
                        case ShurikenAttribute.ForceOverLifetime_Space: v.space = m.space; break;
                        case ShurikenAttribute.ForceOverLifetime_Randomize: v.toggle = m.randomized; break;
                    }
                    break;
                }
                case ShurikenModule.ColorBySpeed:
                {
                    var m = ps.colorBySpeed;
                    switch (a)
                    {
                        case ShurikenAttribute.ColorBySpeed_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.ColorBySpeed_Color: v.gradient = m.color; break;
                        case ShurikenAttribute.ColorBySpeed_Range: v.range = m.range; break;
                    }
                    break;
                }
                case ShurikenModule.SizeBySpeed:
                {
                    var m = ps.sizeBySpeed;
                    switch (a)
                    {
                        case ShurikenAttribute.SizeBySpeed_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.SizeBySpeed_SeparateAxes: v.toggle = m.separateAxes; break;
                        case ShurikenAttribute.SizeBySpeed_Size: v.curve = m.size; break;
                        case ShurikenAttribute.SizeBySpeed_X: v.curve = m.x; break;
                        case ShurikenAttribute.SizeBySpeed_Y: v.curve = m.y; break;
                        case ShurikenAttribute.SizeBySpeed_Z: v.curve = m.z; break;
                        case ShurikenAttribute.SizeBySpeed_Range: v.range = m.range; break;
                    }
                    break;
                }
                case ShurikenModule.RotationOverLifetime:
                {
                    var m = ps.rotationOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.RotationOverLifetime_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.RotationOverLifetime_SeparateAxes: v.toggle = m.separateAxes; break;
                        case ShurikenAttribute.RotationOverLifetime_Z: v.curve = m.z; break;
                        case ShurikenAttribute.RotationOverLifetime_X: v.curve = m.x; break;
                        case ShurikenAttribute.RotationOverLifetime_Y: v.curve = m.y; break;
                    }
                    break;
                }
                case ShurikenModule.RotationBySpeed:
                {
                    var m = ps.rotationBySpeed;
                    switch (a)
                    {
                        case ShurikenAttribute.RotationBySpeed_Enabled: v.toggle = m.enabled; break;
                        case ShurikenAttribute.RotationBySpeed_SeparateAxes: v.toggle = m.separateAxes; break;
                        case ShurikenAttribute.RotationBySpeed_Z: v.curve = m.z; break;
                        case ShurikenAttribute.RotationBySpeed_X: v.curve = m.x; break;
                        case ShurikenAttribute.RotationBySpeed_Y: v.curve = m.y; break;
                        case ShurikenAttribute.RotationBySpeed_Range: v.range = m.range; break;
                    }
                    break;
                }
            }
            if (IsAngular(a)) v.curve = ScaleCurve(v.curve, Mathf.Rad2Deg);
            return v;
        }

        public static void Write(ParticleSystem ps, ShurikenAttribute a, ShurikenValue v)
        {
            if (IsAngular(a)) v.curve = ScaleCurve(v.curve, Mathf.Deg2Rad);
            switch (ModuleOf(a))
            {
                case ShurikenModule.ColorOverLifetime:
                {
                    var m = ps.colorOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.ColorOverLifetime_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.ColorOverLifetime_Color: m.color = v.gradient; break;
                    }
                    break;
                }
                case ShurikenModule.SizeOverLifetime:
                {
                    var m = ps.sizeOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.SizeOverLifetime_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.SizeOverLifetime_SeparateAxes: m.separateAxes = v.toggle; break;
                        case ShurikenAttribute.SizeOverLifetime_Size: m.size = v.curve; break;
                        case ShurikenAttribute.SizeOverLifetime_X: m.x = v.curve; break;
                        case ShurikenAttribute.SizeOverLifetime_Y: m.y = v.curve; break;
                        case ShurikenAttribute.SizeOverLifetime_Z: m.z = v.curve; break;
                    }
                    break;
                }
                case ShurikenModule.VelocityOverLifetime:
                {
                    var m = ps.velocityOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.VelocityOverLifetime_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.VelocityOverLifetime_Space: m.space = v.space; break;
                        case ShurikenAttribute.VelocityOverLifetime_X: m.x = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_Y: m.y = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_Z: m.z = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_OrbitalX: m.orbitalX = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_OrbitalY: m.orbitalY = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_OrbitalZ: m.orbitalZ = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_Radial: m.radial = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_SpeedModifier: m.speedModifier = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_OffsetX: m.orbitalOffsetX = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_OffsetY: m.orbitalOffsetY = v.curve; break;
                        case ShurikenAttribute.VelocityOverLifetime_OffsetZ: m.orbitalOffsetZ = v.curve; break;
                    }
                    break;
                }
                case ShurikenModule.LimitVelocityOverLifetime:
                {
                    var m = ps.limitVelocityOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.LimitVelocity_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.LimitVelocity_SeparateAxes: m.separateAxes = v.toggle; break;
                        case ShurikenAttribute.LimitVelocity_Space: m.space = v.space; break;
                        case ShurikenAttribute.LimitVelocity_Limit: m.limit = v.curve; break;
                        case ShurikenAttribute.LimitVelocity_X: m.limitX = v.curve; break;
                        case ShurikenAttribute.LimitVelocity_Y: m.limitY = v.curve; break;
                        case ShurikenAttribute.LimitVelocity_Z: m.limitZ = v.curve; break;
                        case ShurikenAttribute.LimitVelocity_Dampen: m.dampen = v.number; break;
                        case ShurikenAttribute.LimitVelocity_Drag: m.drag = v.curve; break;
                        case ShurikenAttribute.LimitVelocity_MultiplyDragBySize: m.multiplyDragByParticleSize = v.toggle; break;
                        case ShurikenAttribute.LimitVelocity_MultiplyDragByVelocity: m.multiplyDragByParticleVelocity = v.toggle; break;
                    }
                    break;
                }
                case ShurikenModule.ForceOverLifetime:
                {
                    var m = ps.forceOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.ForceOverLifetime_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.ForceOverLifetime_X: m.x = v.curve; break;
                        case ShurikenAttribute.ForceOverLifetime_Y: m.y = v.curve; break;
                        case ShurikenAttribute.ForceOverLifetime_Z: m.z = v.curve; break;
                        case ShurikenAttribute.ForceOverLifetime_Space: m.space = v.space; break;
                        case ShurikenAttribute.ForceOverLifetime_Randomize: m.randomized = v.toggle; break;
                    }
                    break;
                }
                case ShurikenModule.ColorBySpeed:
                {
                    var m = ps.colorBySpeed;
                    switch (a)
                    {
                        case ShurikenAttribute.ColorBySpeed_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.ColorBySpeed_Color: m.color = v.gradient; break;
                        case ShurikenAttribute.ColorBySpeed_Range: m.range = v.range; break;
                    }
                    break;
                }
                case ShurikenModule.SizeBySpeed:
                {
                    var m = ps.sizeBySpeed;
                    switch (a)
                    {
                        case ShurikenAttribute.SizeBySpeed_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.SizeBySpeed_SeparateAxes: m.separateAxes = v.toggle; break;
                        case ShurikenAttribute.SizeBySpeed_Size: m.size = v.curve; break;
                        case ShurikenAttribute.SizeBySpeed_X: m.x = v.curve; break;
                        case ShurikenAttribute.SizeBySpeed_Y: m.y = v.curve; break;
                        case ShurikenAttribute.SizeBySpeed_Z: m.z = v.curve; break;
                        case ShurikenAttribute.SizeBySpeed_Range: m.range = v.range; break;
                    }
                    break;
                }
                case ShurikenModule.RotationOverLifetime:
                {
                    var m = ps.rotationOverLifetime;
                    switch (a)
                    {
                        case ShurikenAttribute.RotationOverLifetime_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.RotationOverLifetime_SeparateAxes: m.separateAxes = v.toggle; break;
                        case ShurikenAttribute.RotationOverLifetime_Z: m.z = v.curve; break;
                        case ShurikenAttribute.RotationOverLifetime_X: m.x = v.curve; break;
                        case ShurikenAttribute.RotationOverLifetime_Y: m.y = v.curve; break;
                    }
                    break;
                }
                case ShurikenModule.RotationBySpeed:
                {
                    var m = ps.rotationBySpeed;
                    switch (a)
                    {
                        case ShurikenAttribute.RotationBySpeed_Enabled: m.enabled = v.toggle; break;
                        case ShurikenAttribute.RotationBySpeed_SeparateAxes: m.separateAxes = v.toggle; break;
                        case ShurikenAttribute.RotationBySpeed_Z: m.z = v.curve; break;
                        case ShurikenAttribute.RotationBySpeed_X: m.x = v.curve; break;
                        case ShurikenAttribute.RotationBySpeed_Y: m.y = v.curve; break;
                        case ShurikenAttribute.RotationBySpeed_Range: m.range = v.range; break;
                    }
                    break;
                }
            }
        }

        // Unit conversion only: scales constants or the curve multiplier, leaving curve shapes untouched.
        static ParticleSystem.MinMaxCurve ScaleCurve(ParticleSystem.MinMaxCurve c, float k)
        {
            switch (c.mode)
            {
                case ParticleSystemCurveMode.Constant: c.constant *= k; break;
                case ParticleSystemCurveMode.TwoConstants: c.constantMin *= k; c.constantMax *= k; break;
                default: c.curveMultiplier *= k; break;
            }
            return c;
        }
    }
}
