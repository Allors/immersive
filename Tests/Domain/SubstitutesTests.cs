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
namespace Allors.Binary.Tests
{
    using System.IO;

    using Immersive.Fody;

    using Mono.Cecil;

    using NUnit.Framework;

    public class SubstitutesTests
    {
        private readonly Substitutes substitutes;

        public SubstitutesTests()
        {
            var assemblyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Tests.Immersive.dll");
            using (var assemblyResolver = new DefaultAssemblyResolver())
            {
                var directoryName = Path.GetDirectoryName(assemblyPath);
                assemblyResolver.AddSearchDirectory(directoryName);

                using (var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath))
                {
                    this.substitutes = new Substitutes(moduleDefinition);
                }
            }
        }

        [Test]
        public void Default()
        {
            Assert.AreEqual(5, this.substitutes.SubstituteClasses.Count);

            var formSubstitute = this.substitutes.SubstituteClasses["Tests.Immersive.Form"];
            var buttonSubstitute = this.substitutes.SubstituteClasses["Tests.Immersive.Button"];
            var sealedSingleSubstitute = this.substitutes.SubstituteClasses["Tests.Immersive.SealedSingle"];
            var sealedHierarchySubstitute = this.substitutes.SubstituteClasses["Tests.Immersive.SealedHierarchy"];
            var sealedAbstractClassSubstitute = this.substitutes.SubstituteClasses["Tests.Immersive.SealedAbstractClass"];

            Assert.IsNotNull(formSubstitute);
            Assert.IsNotNull(buttonSubstitute);
            Assert.IsNotNull(sealedSingleSubstitute);
            Assert.IsNotNull(sealedHierarchySubstitute);
            Assert.IsNotNull(sealedAbstractClassSubstitute);
        }
    }
}