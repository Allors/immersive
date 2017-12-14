// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestForm.cs" company="allors bvba">
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
namespace AssemblyToProcess
{
    public partial class TestForm : AssemblyReferenced.Form
    {
        public bool constructorCalled = false;

        private AssemblyReferenced.Button button1;
        private AssemblyReferenced.TextBox textBox1;
        private AssemblyReferenced.Nada nada;
        private AssemblyReferenced.SealedSingle sealedSingle;
        private AssemblyReferenced.SealedHierarchy sealedHierarchy;

        public TestForm()
        {
            constructorCalled = true;

            button1 = new AssemblyReferenced.Button();
            textBox1 = new AssemblyReferenced.TextBox();
            nada = new AssemblyReferenced.Nada();
            sealedSingle = new AssemblyReferenced.SealedSingle();
            sealedHierarchy = new AssemblyReferenced.SealedHierarchy();
        }

        public static string ShowMessageBox(bool boolean)
        {
            return AssemblyReferenced.MessageBox.Show(boolean);
        }

        public static string ShowMessageBox(string text)
        {
            return AssemblyReferenced.MessageBox.Show(text);
        }

        public static string ShowMessageBox(int integer)
        {
            return AssemblyReferenced.MessageBox.Show(integer);
        }

        public static string ShowMessageBox(string text, int integer)
        {
            return AssemblyReferenced.MessageBox.Show(text, integer);
        }


        public static string ShowMessageBox2(bool boolean)
        {
            return AssemblyReferenced.MessageBox.Show2(boolean);
        }

        public static string ShowMessageBox2(string text)
        {
            return AssemblyReferenced.MessageBox.Show2(text);
        }

        public static string ShowMessageBox2(int integer)
        {
            return AssemblyReferenced.MessageBox.Show2(integer);
        }

        public static string ShowMessageBox2(string text, int integer)
        {
            return AssemblyReferenced.MessageBox.Show2(text, integer);
        }

        #region Properties
        public AssemblyReferenced.Button Button1
        {
            get { return button1; }
        }

        public AssemblyReferenced.TextBox TextBox1
        {
            get { return textBox1; }
        }

        public AssemblyReferenced.Nada Nada
        {
            get { return nada; }
        }

        public AssemblyReferenced.SealedSingle SealedSingle
        {
            get { return sealedSingle; }
        }

        public AssemblyReferenced.SealedHierarchy SealedHierarchy
        {
            get { return sealedHierarchy; }
        }
        #endregion

        public string CallShowDialog()
        {
            return this.ShowDialog();
        }
    }
}