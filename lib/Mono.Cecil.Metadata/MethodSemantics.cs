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
 * Wed Jan 19 14:28:45 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    using Mono.Cecil;

    [RId (0x18)]
    internal sealed class MethodSemanticsTable : IMetadataTable {

        private RowCollection m_rows;

        public MethodSemanticsRow this [int index] {
            get { return m_rows [index] as MethodSemanticsRow; }
            set { m_rows [index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        public void Accept (IMetadataTableVisitor visitor)
        {
            visitor.Visit (this);
            this.Rows.Accept (visitor.GetRowVisitor ());
        }
    }

    internal sealed class MethodSemanticsRow : IMetadataRow {

        public static readonly int RowSize = 10;
        public static readonly int RowColumns = 3;

        [Column] private MethodSemanticsAttributes m_semantics;
        [Column] private uint m_method;
        [Column] private MetadataToken m_association;

        public MethodSemanticsAttributes Semantics {
            get { return m_semantics; }
            set { m_semantics = value; }
        }

        public uint Method {
            get { return m_method; }
            set { m_method = value; }
        }

        public MetadataToken Association {
            get { return m_association; }
            set { m_association = value; }
        }

        public int Size {
            get { return MethodSemanticsRow.RowSize; }
        }

        public int Columns {
            get { return MethodSemanticsRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}