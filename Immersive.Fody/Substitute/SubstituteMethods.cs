// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteMethods.cs" company="allors bvba">
//   Copyright 2008-2014 Allors bvba.
//   
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
//   
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see http://www.gnu.org/licenses.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Immersive.Fody
{
    using System.Collections;
    using System.Text;

    using Mono.Cecil;

    public class SubstituteMethods : CollectionBase
    {
        private readonly ModuleDefinition moduleDefinition;

        internal SubstituteMethods(ModuleDefinition moduleDefinition)
        {
            this.moduleDefinition = moduleDefinition;

            foreach (TypeDefinition typeDefinitoin in Helper.GetAllTypes(this.moduleDefinition))
            {
                foreach (MethodDefinition methodDefinition in typeDefinitoin.Methods)
                {
                    foreach (CustomAttribute customAttribute in methodDefinition.CustomAttributes)
                    {
                        if (customAttribute.Constructor.DeclaringType.FullName.Equals(Attributes.SubstituteMethodAttribute))
                        {
                            SubstituteMethod substitute = new SubstituteMethod(methodDefinition, customAttribute);
                            List.Add(substitute);
                        }
                    }
                }
            }            
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            foreach (SubstituteMethod substituteMethod in this)
            {
                toString.Append(substituteMethod.ToString());
                toString.Append("\n");
            }

            return toString.ToString();
        }

        public SubstituteMethod this[int index]
        {
            get { return (SubstituteMethod)List[index]; }
        }

        internal SubstituteMethod Lookup(MethodReference operand)
        {
            foreach (SubstituteMethod substitute in this)
            {
                if (substitute.Matches(operand))
                {
                    return substitute;
                }
            }

            return null;
        }
    }
}