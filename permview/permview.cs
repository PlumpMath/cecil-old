//
// permview.cs: Managed Permission Viewer for .NET assemblies
//
// Author:
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
//

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;

using Mono.Cecil;

[assembly: AssemblyTitle ("Mono PermView")]
[assembly: AssemblyDescription ("Managed Permission Viewer for .NET assemblies")]

namespace Mono.Tools {

	class PermView {

		private const string NotSpecified = "\tNot specified.";

		static private void Help () 
		{
			Console.WriteLine ("Usage: permview [options] assembly{0}", Environment.NewLine);
			Console.WriteLine ("where options are:");
			Console.WriteLine (" -output filename  Output information into specified file.");
			Console.WriteLine (" -decl             Show declarative security attributes on classes and methods.");
			Console.WriteLine (" -xml              Output in XML format");
			Console.WriteLine (" -help             Show help informations (this text)");
			Console.WriteLine ();
		}

		static bool declarative = false;
		static bool xmloutput = false;

		static void ShowPermissionSet (TextWriter tw, string header, PermissionSet ps)
		{
			if (header != null)
				tw.WriteLine (header);

			if ((ps == null) || ((ps.Count == 0) && !ps.IsUnrestricted ())) {
				tw.WriteLine ("\tNone");
			} else {
				tw.WriteLine (ps.ToString ());
			}

			tw.WriteLine ();
		}

		static TextWriter ProcessOptions (string[] args)
		{
			TextWriter tw = Console.Out;
			for (int i=0; i < args.Length - 1; i++) {
				switch (args [i].ToUpper ()) {
				case "/DECL":
				case "-DECL":
				case "--DECL":
					declarative = true;
					break;
				case "/OUTPUT":
				case "-OUTPUT":
				case "--OUTPUT":
					tw = (TextWriter) new StreamWriter (args [++i]);
					break;
				case "/XML":
				case "-XML":
				case "--XML":
					xmloutput = true;
					break;
				case "/HELP":
				case "/H":
				case "-HELP":
				case "-H":
				case "--HELP":
				case "--H":
				case "-?":
				case "--?":
					Help ();
					return null;
				}
			}
			return tw;
		}

		static bool ProcessAssemblyOnly (TextWriter tw, IAssemblyDefinition ad) 
		{
			bool result = true;
			string minimal = NotSpecified + Environment.NewLine;
			string optional = NotSpecified + Environment.NewLine;
			string refused = NotSpecified + Environment.NewLine;

			foreach (ISecurityDeclaration decl in ad.SecurityDeclarations) {
				switch (decl.Action) {
				case Mono.Cecil.SecurityAction.RequestMinimum:
					minimal = decl.PermissionSet.ToString ();
					break;
				case Mono.Cecil.SecurityAction.RequestOptional:
					optional = decl.PermissionSet.ToString ();
					break;
				case Mono.Cecil.SecurityAction.RequestRefuse:
					refused = decl.PermissionSet.ToString ();
					break;
				default:
					tw.WriteLine ("Invalid assembly level declaration {0}{1}{2}",
						decl.Action, Environment.NewLine, decl.PermissionSet);
					result = false;
					break;
				}
			}

			tw.WriteLine ("Minimal Permission Set:");
			tw.WriteLine (minimal);
			tw.WriteLine ("Optional Permission Set:");
			tw.WriteLine (optional);
			tw.WriteLine ("Refused Permission Set:");
			tw.WriteLine (refused);
			return result;
		}

		static void ShowSecurity (TextWriter tw, string header, ISecurityDeclarationCollection declarations)
		{
			foreach (ISecurityDeclaration declsec in declarations) {
				tw.WriteLine ("{0} {1} Permission Set:{2}{3}", header,
					declsec.Action, Environment.NewLine, declsec.PermissionSet);
			}
		}

		static bool ProcessAssemblyComplete (TextWriter tw, IAssemblyDefinition ad)
		{
			if (ad.SecurityDeclarations.Count > 0) {
				ShowSecurity (tw, "Assembly", ad.SecurityDeclarations);
			}

			foreach (IModuleDefinition module in ad.Modules) {

				foreach (ITypeDefinition type in module.Types) {

					if (type.SecurityDeclarations.Count > 0) {
						ShowSecurity (tw, "Class " + type.ToString (), ad.SecurityDeclarations);
					}

					foreach (IMethodDefinition method in type.Methods) {
						if (method.SecurityDeclarations.Count > 0) {
							ShowSecurity (tw, "Method " + method.ToString (), method.SecurityDeclarations);
						}
					}
				}
			}
			return true;
		}

		static void AddAttribute (SecurityElement se, string attr, string value)
		{
			value = value.Replace ("&", "&amp;");
			se.AddAttribute (attr, value);
		}

		static SecurityElement AddSecurityXml (ISecurityDeclarationCollection declarations)
		{
			SecurityElement se = new SecurityElement ("Actions");
			foreach (ISecurityDeclaration declsec in declarations) {
				SecurityElement child = new SecurityElement ("Action");
				AddAttribute (child, "Name", declsec.Action.ToString ());
				child.AddChild (declsec.PermissionSet.ToXml ());
				se.AddChild (child);
			}
			return se;
		}

		static bool ProcessAssemblyXml (TextWriter tw, IAssemblyDefinition ad)
		{
			SecurityElement se = new SecurityElement ("DeclarativeSecurityPermissions");
			se.AddAttribute ("AssemblyName", ad.Name.FullName);

			if (ad.SecurityDeclarations.Count > 0) {
				SecurityElement assembly = new SecurityElement ("Assembly");
				assembly.AddChild (AddSecurityXml (ad.SecurityDeclarations));
				se.AddChild (assembly);
			}

			foreach (IModuleDefinition module in ad.Modules) {

				foreach (ITypeDefinition type in module.Types) {

					SecurityElement klass = new SecurityElement ("Class");
					SecurityElement methods = new SecurityElement ("Methods");

					SecurityElement typelem = null;
					if (type.SecurityDeclarations.Count > 0) {
						typelem = AddSecurityXml (type.SecurityDeclarations);
					}

					foreach (IMethodDefinition method in type.Methods) {
						if (method.SecurityDeclarations.Count > 0) {
							if (typelem == null)
								typelem = AddSecurityXml (type.SecurityDeclarations);

							SecurityElement meth = new SecurityElement ("Method");
							AddAttribute (meth, "Name", method.ToString ());
							meth.AddChild (AddSecurityXml (method.SecurityDeclarations));
							methods.AddChild (meth);
						}
					}

					if (typelem != null) {
						AddAttribute (klass, "Name", type.ToString ());
						klass.AddChild (typelem);
						if ((methods.Children != null) && (methods.Children.Count > 0))
							klass.AddChild (methods);
						se.AddChild (klass);
					}
				}
			}

			tw.WriteLine (se.ToString ());
			return true;
		}

		[STAThread]
		static int Main (string[] args) 
		{
			try {
				Console.WriteLine ("Mono PermView - Special Cecil-based version{0}", Environment.NewLine);
				if (args.Length == 0) {
					Help ();
					return 0;
				}

				TextWriter tw = ProcessOptions (args);
				if (tw == null)
					return 0;

				string assemblyName = args [args.Length - 1];
				IAssemblyDefinition ad = AssemblyFactory.GetAssembly (assemblyName, LoadingType.Lazy);
				if (ad != null) {
					bool complete = false;
					
					if (declarative) {
						// full output (assembly+classes+methods)
						complete = ProcessAssemblyComplete (tw, ad);
					} else if (xmloutput) {
						// full output in XML (for easier diffs after c14n)
						complete = ProcessAssemblyXml (tw, ad);
					} else {
						// default (assembly only)
						complete = ProcessAssemblyOnly (tw, ad);
					}

					if (!complete) {
						Console.Error.WriteLine ("Couldn't reflect informations.");
						return 1;
					}
				} else {
					Console.Error.WriteLine ("Couldn't load assembly '{0}'.", assemblyName);
					return 2;
				}
				tw.Close ();
			}
			catch (Exception e) {
				Console.Error.WriteLine ("Error: " + e.ToString ());
				Help ();
				return 3;
			}
			return 0;
		}
	}
}