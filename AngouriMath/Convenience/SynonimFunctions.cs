﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AngouriMath
{
    using FuncTable = Dictionary<string, Func<List<Entity>, Entity>>;
    using SynTable = Dictionary<string, string>;
    internal static class SynonimFunctions
    {
        internal static readonly FuncTable SynFunctions = new FuncTable
        {
            { "sqrtf", args => MathS.Pow(args[0], 0.5) },
            { "sqrf", args => MathS.Pow(args[0], 2) },
            { "tanf", args => MathS.Tan(args[0]) },
            { "cotanf", args => MathS.Cotan(args[0]) },
            { "bf", args => args[0] * MathS.Sin(args[0]) },
            { "tbf", args => args[0] * MathS.Cos(args[0]) },
            { "lnf", args => MathS.Log(args[0], MathS.e) },
            { "secf", args => MathS.Sec(args[0]) },
            { "cosecf", args => MathS.Cosec(args[0]) },
        };
        internal static Entity Synonimize(Entity tree)
        {
            for (int i = 0; i < tree.Children.Count; i++)
            {
                tree.Children[i] = Synonimize(tree.Children[i]);
            }
            if (SynFunctions.ContainsKey(tree.Name))
                return SynFunctions[tree.Name](tree.Children);
            else
                return tree;
        }
    }
}