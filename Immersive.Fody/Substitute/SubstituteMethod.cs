// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteMethod.cs" company="allors bvba">
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

    public class SubstituteMethod
    {
        public SubstituteMethod(ModuleWeaver moduleWeaver, MethodDefinition methodDefinition, CustomAttribute customAttribute)
        {
            this.ModuleWeaver = moduleWeaver;

            this.Definition = methodDefinition;
            var typeDefinition = (TypeReference)customAttribute.ConstructorArguments[0].Value;
            this.FullTypeName = typeDefinition.FullName;
            
            this.MethodName = methodDefinition.Name;
            if (customAttribute.ConstructorArguments.Count > 1)
            {
                this.MethodName = (string)customAttribute.ConstructorArguments[1].Value;
            }

            this.ModuleWeaver.LogInfo($"SubstituteMethod: ${this.FullTypeName}.${this.MethodName}");
        }

        public ModuleWeaver ModuleWeaver { get; }

        public MethodDefinition Definition { get; }

        public string FullTypeName { get; }

        public string MethodName { get; }

        public MethodDefinition MethodDefinition => this.Definition;

        public override string ToString()
        {
            return this.Definition.ToString();
        }

        internal bool Matches(MethodReference operand)
        {
            if (this.MethodName.Equals(operand.Name))
            {
                if (this.FullTypeName.Equals(operand.DeclaringType.FullName))
                {
                    if (this.Definition.Parameters.Count == operand.Parameters.Count)
                    {
                        for (int i = 0; i < this.Definition.Parameters.Count; i++)
                        {
                            if (!this.Definition.Parameters[i].ParameterType.FullName.Equals(operand.Parameters[i].ParameterType.FullName))
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}