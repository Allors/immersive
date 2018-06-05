// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutableClass.cs" company="allors bvba">
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

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public class SubstitutableClass
    {
        public SubstitutableClass(ModuleWeaver moduleWeaver, TypeDefinition typeDefinition)
        {
            this.ModuleWeaver = moduleWeaver;
            this.TypeDefinition = typeDefinition;

            this.ModuleWeaver.LogInfo($"Substitutable: ${this.TypeDefinition.FullName}");
        }

        public ModuleWeaver ModuleWeaver { get; }

        public ModuleDefinition ModuleDefinition => this.ModuleWeaver.ModuleDefinition;

        public TypeDefinition TypeDefinition { get; }

        public override string ToString()
        {
            return this.TypeDefinition.ToString();
        }

        internal void Substitute(Substitutes substitutes)
        {
            if (this.TypeDefinition.Name != "<Module>")
            {
                this.SubstituteConstructors(substitutes);
                this.SubstituteBaseType(substitutes);
                this.SubstituteMethods(substitutes);
                this.SubstituteFields(substitutes);
                this.SubstituteLocalVariables(substitutes);
                this.SubstituteReturnTypes(substitutes);
            }
        }

        private void SubstituteConstructors(Substitutes substitutes)
        {
            foreach (var constructor in Helper.GetContructors(this.TypeDefinition))
            {
                if (constructor.HasBody)
                {
                    foreach (var instruction in constructor.Body.Instructions)
                    {
                        if (instruction.OpCode.Equals(OpCodes.Call))
                        {
                            var operand = (MethodReference)instruction.Operand;
                            var operandDeclaringType = operand.DeclaringType;

                            var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(operandDeclaringType.FullName);

                            if (substitute != null)
                            {
                                // TODO: Migration check
                                if (operand.Name.Equals(".ctor"))
                                {
                                    var substituteConstructorDefinition = substitute.Contructor;
                                    var substituteConstructorReference = this.ModuleDefinition.ImportReference(substituteConstructorDefinition);
                                    instruction.Operand = substituteConstructorReference;
                                }
                            }
                        }
                    }
                }
            }

            var constructorsAndMethods = new ArrayList(Helper.GetContructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDefinition method in constructorsAndMethods)
            {
                if (method.HasBody)
                {
                    foreach (var instruction in method.Body.Instructions)
                    {
                        if (instruction.OpCode.Equals(OpCodes.Newobj))
                        {
                            var operand = (MethodReference)instruction.Operand;
                            var operandDeclaringType = operand.DeclaringType;

                            var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(operandDeclaringType.FullName);

                            if (substitute != null)
                            {
                                var substituteConstructorDefinition = substitute.Contructor;
                                var substituteConstructorReference = this.ModuleDefinition.ImportReference(substituteConstructorDefinition);
                                instruction.Operand = substituteConstructorReference;
                            }
                        }
                    }
                }
            }
        }

        private void SubstituteBaseType(Substitutes substitutes)
        {
            var baseTypeReference = this.TypeDefinition.BaseType;
            var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(baseTypeReference.FullName);

            if (substitute != null)
            {
                var substituteTypeReference = this.ModuleDefinition.ImportReference(substitute.TypeDefinition);
                this.TypeDefinition.BaseType = substituteTypeReference;
            }
        }

        private void SubstituteMethods(Substitutes substitutes)
        {
            var constructorsAndMethods = new ArrayList(Helper.GetContructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDefinition method in constructorsAndMethods)
            {
                if (method.HasBody)
                {
                    foreach (var instruction in method.Body.Instructions)
                    {
                        if (instruction.OpCode.Equals(OpCodes.Call) ||
                            instruction.OpCode.Equals(OpCodes.Calli) ||
                            instruction.OpCode.Equals(OpCodes.Callvirt))
                        {
                            var methodReference = (MethodReference)instruction.Operand;

                            var substitute = substitutes.SubstituteMethods.Lookup(methodReference);
                            if (substitute != null)
                            {
                                var substituteMethodReference = this.ModuleDefinition.ImportReference(substitute.MethodDefinition);
                                instruction.Operand = substituteMethodReference;
                            }
                            else
                            {
                                var declaringClassSubstitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(methodReference.DeclaringType.FullName);
                                if (declaringClassSubstitute != null && !declaringClassSubstitute.IsBaseSubsitution)
                                {
                                    var classSubstitueReference = this.ModuleDefinition.ImportReference(declaringClassSubstitute.TypeDefinition);
                                    methodReference.DeclaringType = classSubstitueReference;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SubstituteFields(Substitutes substitutes)
        {
            foreach (var field in this.TypeDefinition.Fields)
            {
                var fieldTypeReference = field.FieldType;
                var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(fieldTypeReference.FullName);

                if (substitute != null && !substitute.IsBaseSubsitution)
                {
                    var substituteTypeReference = this.ModuleDefinition.ImportReference(substitute.TypeDefinition);
                    field.FieldType = substituteTypeReference;
                }
            }
        }

        private void SubstituteLocalVariables(Substitutes substitutes)
        {
            var constructorsAndMethods = new ArrayList(Helper.GetContructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDefinition method in constructorsAndMethods)
            {
                if (method.HasBody)
                {
                    foreach (var variableDefinition in method.Body.Variables)
                    {
                        SubstituteClass substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(variableDefinition.VariableType.FullName);

                        if (substitute != null && !substitute.IsBaseSubsitution)
                        {
                            var substituteTypeReference = this.ModuleDefinition.ImportReference(substitute.TypeDefinition);
                            variableDefinition.VariableType = substituteTypeReference;
                        }
                    }
                }
            }
        }

        private void SubstituteReturnTypes(Substitutes substitutes)
        {
            var constructorsAndMethods = new ArrayList(Helper.GetContructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDefinition method in constructorsAndMethods)
            {
                var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(method.ReturnType.FullName);

                if (substitute != null && !substitute.IsBaseSubsitution)
                {
                    var substituteTypeReference = this.ModuleDefinition.ImportReference(substitute.TypeDefinition);
                    method.ReturnType = substituteTypeReference;
                }
            }
        }
    }
}