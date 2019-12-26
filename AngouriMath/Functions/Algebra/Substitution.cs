﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AngouriMath
{
    public abstract partial class Entity
    {
        /// <summary>
        /// Substitute a variable with an expression
        /// </summary>
        /// <param name="x">
        /// A variable to substitute
        /// </param>
        /// <param name="value">
        /// The value we replace variable with
        /// </param>
        /// <returns></returns>
        public Entity Substitute(VariableEntity x, Entity value)
            => Substitute(x, value, false);
        public Entity Substitute(VariableEntity x, Entity value, bool inPlace)
        {
            Entity res;
            if (inPlace)
                res = this;
            else
                res = DeepCopy();
            if (res == x)
                return value;
            for (int i = 0; i < res.Children.Count; i++)
                if (res.Children[i] is VariableEntity && (res.Children[i] as VariableEntity).Name == x.Name)
                    res.Children[i] = value;
                else
                    res.Children[i] = res.Children[i].Substitute(x, value, true);
            return res;
        }
    }
}