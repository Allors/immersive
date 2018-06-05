// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Substitutes.cs" company="allors bvba">
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
    using Mono.Cecil;

    public class Substitutes
    {
        public Substitutes(ModuleWeaver moduleWeaver, ModuleDefinition moduleDefinition)
        {
            this.ModuleWeaver = moduleWeaver;

            this.ModuleWeaver.LogInfo($"Substitutes: ${moduleDefinition.Assembly.FullName}");
            
            this.SubstituteClasses = new SubstituteClasses(moduleWeaver, moduleDefinition);
            this.SubstituteMethods = new SubstituteMethods(moduleWeaver, moduleDefinition);
        }

        public ModuleWeaver ModuleWeaver { get; set; }

        public SubstituteClasses SubstituteClasses { get; }

        public SubstituteMethods SubstituteMethods { get; }
    }
}