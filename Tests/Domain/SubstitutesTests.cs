// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutesTests.cs" company="allors bvba">
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
namespace Immersive.Tests
{
    using System.IO;

    using Immersive.Fody;

    using Mono.Cecil;

    using Xunit;

    public class SubstitutesTests
    {
        private readonly Substitutes substitutes;

        public SubstitutesTests()
        {
            var assemblyPath = new DirectoryInfo("AssemblyToImmerse.dll").FullName;
            using (var assemblyResolver = new DefaultAssemblyResolver())
            {
                var directoryName = Path.GetDirectoryName(assemblyPath);
                assemblyResolver.AddSearchDirectory(directoryName);

                using (var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath))
                {
                    this.substitutes = new Substitutes(Fixture.ModuleWeaver, moduleDefinition);
                }
            }
        }

        [Fact]
        public void Default()
        {
            Assert.Equal(5, this.substitutes.SubstituteClasses.Count);

            var formSubstitute = this.substitutes.SubstituteClasses["AssemblyToImmerse.Form"];
            var buttonSubstitute = this.substitutes.SubstituteClasses["AssemblyToImmerse.Button"];
            var sealedSingleSubstitute = this.substitutes.SubstituteClasses["AssemblyToImmerse.SealedSingle"];
            var sealedHierarchySubstitute = this.substitutes.SubstituteClasses["AssemblyToImmerse.SealedHierarchy"];
            var sealedAbstractClassSubstitute = this.substitutes.SubstituteClasses["AssemblyToImmerse.SealedAbstractClass"];

            Assert.NotNull(formSubstitute);
            Assert.NotNull(buttonSubstitute);
            Assert.NotNull(sealedSingleSubstitute);
            Assert.NotNull(sealedHierarchySubstitute);
            Assert.NotNull(sealedAbstractClassSubstitute);
        }
    }
}