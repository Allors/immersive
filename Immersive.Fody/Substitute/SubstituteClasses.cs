// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteClasses.cs" company="allors bvba">
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
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Mono.Cecil;

    public class SubstituteClasses : CollectionBase
    {
        private readonly ModuleDefinition moduleDefinition;

        internal SubstituteClasses(ModuleDefinition moduleDefinition)
        {
            this.moduleDefinition = moduleDefinition;

            foreach (TypeDefinition typeDefinition in Helper.GetAllTypes(moduleDefinition))
            {
                var attribute = typeDefinition.CustomAttributes.FirstOrDefault(v => v.AttributeType.Name.Equals("SubstituteClassAttribute"));
                if (attribute != null)
                {
                    SubstituteClass substitute = new SubstituteClass(typeDefinition);
                    InnerList.Add(substitute);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            foreach (SubstituteClass substituteClass in this)
            {
                toString.Append(substituteClass.ToString());
                toString.Append("\n");
            }

            return toString.ToString();
        }

        public SubstituteClass this[int index]
        {
            get { return (SubstituteClass)List[index]; }
        }

        public SubstituteClass this[string typeFullName]
        {
            get
            {
                foreach (SubstituteClass substitute in this)
                {
                    if (string.Equals(typeFullName, substitute.TypeDefinition.FullName))
                    {
                        return substitute;
                    }
                }

                return null;
            }
        }

        public SubstituteClass LookupBySubstitutableFullName(string substitutableFullName)
        {
            foreach (SubstituteClass substitute in this)
            {
                if (substitutableFullName != null && substitutableFullName.Equals(substitute.SubstitutableFullName))
                {
                    return substitute;
                }
            }

            return null;
        }
    }
}