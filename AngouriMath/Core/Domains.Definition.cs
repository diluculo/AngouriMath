﻿
/* Copyright (c) 2019-2020 Angourisoft
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using AngouriMath.Core;

namespace AngouriMath
{
    partial record Entity
    {
        private protected abstract Domain DefaultCodomain { get; }

        /// <summary>
        /// Coomain of an expression
        /// If its node value is outside of the domain,
        /// it's converted to NaN
        /// </summary>
        public Domain Codomain 
        {
            // TODO: move to abstract getter & abstract setter
            get => codomain ??= DefaultCodomain;
            protected init => codomain = value;
        }
        private Domain? codomain = null;
        

        protected Entity InnerWithNewDomain(Domain newDomain)
            => Codomain == newDomain ? this : this with { Codomain = newDomain };

        /// <summary>
        /// Changes nodes of one domain to another one
        /// </summary>
        public Entity DomainChange(Domain domainFrom, Domain domainTo)
            => Replace(ent =>
            {
                if (ent.Codomain != domainFrom)
                    return ent;
                if (ent is Boolean or Number)
                    return ent;
                return ent with { Codomain = domainTo };
            });

        /// <summary>
        /// Moves all real domains to the complex ones
        /// </summary>
        public Entity DomainFromRealToComplex()
            => DomainChange(Domain.Real, Domain.Complex);

        /// <summary>
        /// Moves all real domains to the complex ones
        /// </summary>
        public Entity DomainFromComplexToReal()
            => DomainChange(Domain.Complex, Domain.Real);
    }
}
