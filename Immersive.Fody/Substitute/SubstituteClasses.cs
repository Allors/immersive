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
    using System.Collections;
    using System.Linq;
    using System.Text;

    using Mono.Cecil;

    public class SubstituteClasses : CollectionBase
    {
        internal SubstituteClasses(ModuleWeaver moduleWeaver, ModuleDefinition moduleDefinition)
        {
            this.ModuleWeaver = moduleWeaver;
            this.Definition = moduleDefinition;

            foreach (var typeDefinition in Helper.GetAllTypes(moduleDefinition))
            {
                var attribute = typeDefinition.CustomAttributes.FirstOrDefault(v => v.AttributeType.FullName.Equals(Attributes.SubstituteClassAttribute));
                if (attribute != null)
                {
                    var substitute = new SubstituteClass(moduleWeaver, typeDefinition);
                    this.InnerList.Add(substitute);
                }
            }
        }

        public ModuleWeaver ModuleWeaver { get; set; }

        public ModuleDefinition Definition { get; }

        public SubstituteClass this[int index] => (SubstituteClass)this.List[index];

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

        public override string ToString()
        {
            var toString = new StringBuilder();
            foreach (SubstituteClass substituteClass in this)
            {
                toString.Append(substituteClass);
                toString.Append("\n");
            }

            return toString.ToString();
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