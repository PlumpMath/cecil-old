/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Tue Jan 25 18:18:55 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Mono.Cecil;

    internal class AssemblyNameReferenceCollection : IAssemblyNameReferenceCollection {

        private IDictionary m_items;
        private IModuleDefinition m_container;

        public IAssemblyNameReference this [string name] {
            get { return m_items [name] as IAssemblyNameReference; }
            set { m_items [name] = value; }
        }

        public IModuleDefinition Container {
            get { return m_container; }
        }

        public int Count {
            get { return m_items.Count; }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return this; }
        }

        public AssemblyNameReferenceCollection (IModuleDefinition container)
        {
            m_container = container;
            m_items = new ListDictionary ();
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (IAssemblyNameReference value)
        {
            return m_items.Contains (value);
        }

        public void Remove (IAssemblyNameReference value)
        {
            m_items.Remove (value);
        }

        public void CopyTo (Array ary, int index)
        {
            m_items.Values.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            return m_items.Values.GetEnumerator ();
        }

        public void Accept (IReflectionStructureVisitor visitor)
        {
            visitor.Visit (this);
            IAssemblyNameReference [] items = new IAssemblyNameReference [m_items.Count];
            m_items.Values.CopyTo (items, 0);
            for (int i = 0; i < items.Length; i++)
                items [i].Accept (visitor);
        }
    }
}