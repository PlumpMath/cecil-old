// 
// Unit tests for MethodRocks
//
// Authors:
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2007-2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Reflection;

using Gendarme.Framework;
using Gendarme.Framework.Rocks;

using Mono.Cecil;
using NUnit.Framework;

namespace Test.Framework.Rocks {

	[TestFixture]
	public class MethodRocksTest {

		class MainClassVoidVoid {
			static void Main ()
			{
			}
		}

		class MainClassIntVoid {
			static int Main ()
			{
				return 42;
			}
		}

		class MainClassVoidStrings {
			static void Main (string[] args)
			{
			}
		}

		class MainClassIntStrings {
			static int Main (string [] args)
			{
				return 42;
			}
		}

		public int Value {
			get { return 42; }
			set { throw new NotSupportedException (); }
		}

		private AssemblyDefinition assembly;

		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
		}

		private MethodDefinition GetMethod (string typeName, string methodName)
		{
			TypeDefinition type = assembly.MainModule.Types [typeName];
			foreach (MethodDefinition method in type.Methods) {
				if (method.Name == methodName)
					return method;
			}
			Assert.Fail ("Method {0} was not found.", methodName);
			return null;
		}

		private MethodDefinition GetMethod (string name)
		{
			return GetMethod ("Test.Framework.Rocks.MethodRocksTest", name);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void HasAttribute_Null ()
		{
			CustomAttributeCollection cac = new CustomAttributeCollection (null);
			cac.Contains ((string) null);
		}

		[Test]
		public void HasAttribute ()
		{
			CustomAttributeCollection cac = GetMethod ("FixtureSetUp").CustomAttributes;
			Assert.IsTrue (cac.Contains ("NUnit.Framework.TestFixtureSetUpAttribute"), "NUnit.Framework.TestFixtureSetUpAttribute");
			Assert.IsFalse (cac.Contains ("NUnit.Framework.TestFixtureSetUp"), "NUnit.Framework.TestFixtureSetUp");
		}

		[Test]
		public void IsEntryPoint ()
		{
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsEntryPoint (), "FixtureSetUp");
		}

		[Test]
		[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
		public void IsGeneratedCode_CompilerGenerated ()
		{
			Assert.IsTrue (GetMethod ("IsGeneratedCode_CompilerGenerated").IsGeneratedCode (), "IsCompilerGenerated");
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsGeneratedCode (), "FixtureSetUp");
		}

		[Test]
		[System.CodeDom.Compiler.GeneratedCodeAttribute ("unit test", "1.0")]
		public void IsGeneratedCode_GeneratedCode ()
		{
			Assert.IsTrue (GetMethod ("IsGeneratedCode_GeneratedCode").IsGeneratedCode (), "IsCompilerGenerated");
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsGeneratedCode (), "FixtureSetUp");
		}

		[Test]
		public void IsGetter ()
		{
			Assert.IsTrue (GetMethod ("get_Value").IsGetter (), "get_Value");
			Assert.IsFalse (GetMethod ("set_Value").IsGetter (), "set_Value");
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsGetter (), "FixtureSetUp");
		}

		[Test]
		public void IsMain ()
		{
			Assert.IsTrue (GetMethod ("Test.Framework.Rocks.MethodRocksTest/MainClassVoidVoid", "Main").IsMain (), "MainClassVoidVoid");
			Assert.IsTrue (GetMethod ("Test.Framework.Rocks.MethodRocksTest/MainClassIntVoid", "Main").IsMain (), "MainClassIntVoid");
			Assert.IsTrue (GetMethod ("Test.Framework.Rocks.MethodRocksTest/MainClassVoidStrings", "Main").IsMain (), "MainClassVoidStrings");
			Assert.IsTrue (GetMethod ("Test.Framework.Rocks.MethodRocksTest/MainClassIntStrings", "Main").IsMain (), "MainClassIntStrings");
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsMain (), "FixtureSetUp");
		}

		[Test]
		public void IsProperty ()
		{
			Assert.IsTrue (GetMethod ("get_Value").IsProperty (), "get_Value");
			Assert.IsTrue (GetMethod ("set_Value").IsProperty (), "set_Value");
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsProperty (), "FixtureSetUp");
		}

		[Test]
		public void IsSetter ()
		{
			Assert.IsFalse (GetMethod ("get_Value").IsSetter (), "get_Value");
			Assert.IsTrue (GetMethod ("set_Value").IsSetter (), "set_Value");
			Assert.IsFalse (GetMethod ("FixtureSetUp").IsSetter (), "FixtureSetUp");
		}
	}
}
