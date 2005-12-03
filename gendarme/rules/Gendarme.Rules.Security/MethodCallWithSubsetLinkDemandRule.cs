//
// Gendarme.Rules.Security.MethodCallWithSubsetLinkDemandRule
//
// Authors:
//	Sebastien Pouliot <sebastien@ximian.com>
//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Security;
using System.Security.Permissions;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Gendarme.Framework;

namespace Gendarme.Rules.Security {

	public class MethodCallWithSubsetLinkDemandRule : IMethodRule {

		private PermissionSet GetLinkDemand (IMethodDefinition method)
		{
			foreach (ISecurityDeclaration declsec in method.SecurityDeclarations) {
				switch (declsec.Action) {
				case Mono.Cecil.SecurityAction.LinkDemand:
				case Mono.Cecil.SecurityAction.NonCasLinkDemand:
					return declsec.PermissionSet;
				}
			}
			return null;
		}

		private bool Check (IMethodDefinition caller, IMethodDefinition callee)
		{
			// 1 - look if the callee has a LinkDemand
			PermissionSet calleeLinkDemand = GetLinkDemand (callee);
			if (calleeLinkDemand == null)
				return true;

			// 2 - Ensure the caller requires a superset (or the same) permissions
			return calleeLinkDemand.IsSubsetOf (GetLinkDemand (caller));
		}

		public bool CheckMethod (IAssemblyDefinition assembly, IModuleDefinition module, ITypeDefinition type, IMethodDefinition method)
		{
			// #1 - rule apply to methods are publicly accessible
			//	note that the type doesn't have to be public (indirect access)
			if ((method.Attributes & MethodAttributes.Public) != MethodAttributes.Public)
				return true;

			// #2 - rule apply only if the method has a body (e.g. p/invokes, icalls don't)
			//	otherwise we don't know what it's calling
			if (method.Body == null)
				return true;

			// *** ok, the rule applies! ***

			// #3 - look for every method we call
			foreach (IInstruction ins in method.Body.Instructions) {
				switch (ins.OpCode.Name) {
				case "call":
				case "callvirt":
					IMethodDefinition callee = AssemblyManager.GetMethod (ins.Operand);
					if (callee == null) {
						return true; // ignore (missing reference)
					}
					// 4 - and if it has security, ensure we don't reduce it's strength
					if ((callee.SecurityDeclarations.Count > 0) && !Check (method, callee))
						return false;
					break;
				}
			}
			return true;
		}
	}
}