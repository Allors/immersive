// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteClass.cs" company="allors bvba">
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
    using System.Linq;
    using Mono.Cecil;

    public class SubstituteClass
    {
        public SubstituteClass(TypeDefinition typeDefinition)
        {
            this.TypeDefinition = typeDefinition;

            var attribute = this.TypeDefinition.CustomAttributes.FirstOrDefault(v => v.AttributeType.FullName.Equals(Attributes.SubstituteClassAttribute));

            if (attribute != null)
            {
                if (attribute.HasConstructorArguments)
                {
                    this.IsBaseSubsitution = false;

                    var baseType = (TypeReference)attribute.ConstructorArguments[0].Value;
                    this.SubstitutableFullName = baseType.FullName;
                }
                else
                {
                    if (typeDefinition.BaseType == null)
                    {
                        throw new Exception("Base type is required");
                    }

                    this.IsBaseSubsitution = true;
                    this.SubstitutableFullName = typeDefinition.BaseType.FullName;
                }
            }
        }

        public TypeDefinition TypeDefinition { get; }

        public string SubstitutableFullName { get; }

        public bool IsBaseSubsitution { get; }

        public MethodDefinition Contructor => Helper.GetFirstContructor(this.TypeDefinition);

        public override string ToString()
        {
            return this.TypeDefinition.ToString();
        }
    }
}