// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutableAssembly.cs" company="allors bvba">
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

    public class Substitutables : CollectionBase
    {
        public Substitutables(ModuleWeaver moduleWeaver)
        {
            this.ModuleWeaver = moduleWeaver;
            var moduleDefinition = moduleWeaver.ModuleDefinition;

            this.ModuleWeaver.LogInfo($"Substitutables: ${moduleDefinition.Assembly.FullName}");
            
            foreach (var typeDefinition in Helper.GetAllTypes(moduleDefinition))
            {
                if (!typeDefinition.IsInterface)
                {
                    var substitutableClass = new SubstitutableClass(moduleWeaver, typeDefinition);
                    this.List.Add(substitutableClass);
                }
            }
        }

        public ModuleWeaver ModuleWeaver { get; }

        public SubstitutableClass this[int index] => (SubstitutableClass)this.List[index];

        public void Substitute(Substitutes substitutes)
        {
            foreach (SubstitutableClass substitutableClass in this)
            {
                substitutableClass.Substitute(substitutes);
            }
        }
    }
}