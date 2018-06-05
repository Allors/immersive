// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyToProcessTests.cs" company="allors bvba">
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
    using System;

    using Xunit;

    public class AssemblyToProcessTests
    {
        [Fact]
        public void Default()
        {
            var testFormType = Fixture.BeforeAssembly.GetType("AssemblyToProcess.TestForm");
            var testForm = (dynamic)Activator.CreateInstance(testFormType);

            Assert.Equal("AssemblyToProcess.TestForm", testForm.GetType().FullName);

            Assert.Equal("AssemblyReferenced.Button", testForm.Button1.GetType().FullName);
            Assert.Equal("AssemblyReferenced.TextBox", testForm.TextBox1.GetType().FullName);
            Assert.Equal("AssemblyReferenced.SealedSingle", testForm.SealedSingle.GetType().FullName);
            Assert.Equal("AssemblyReferenced.SealedHierarchy", testForm.SealedHierarchy.GetType().FullName);

            var nadaType = Fixture.BeforeAssembly.GetType("AssemblyToProcess.Nada");
            var nada = (dynamic)Activator.CreateInstance(nadaType);

            Assert.Throws<Exception>(() => nada.TestSealedSingle("1"));
            Assert.Throws<Exception>(() => nada.TestSealedHierarchy("2"));
            Assert.Throws<Exception>(() => nada.TestSealedHierarchyAbstract("3"));
        }
    }
}