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

    [RId (0x19)]
    internal sealed class MethodImplTable : IMetadataTable {

        private RowCollection m_rows;

        public MethodImplRow this [int index] {
            get { return m_rows [index] as MethodImplRow; }
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

    internal sealed class MethodImplRow : IMetadataRow {

        public static readonly int RowSize = 12;
        public static readonly int RowColumns = 3;

        [Column] private uint m_class;
        [Column] private MetadataToken m_methodBody;
        [Column] private MetadataToken m_methodDeclaration;

        public uint Class {
            get { return m_class; }
            set { m_class = value; }
        }

        public MetadataToken MethodBody {
            get { return m_methodBody; }
            set { m_methodBody = value; }
        }

        public MetadataToken MethodDeclaration {
            get { return m_methodDeclaration; }
            set { m_methodDeclaration = value; }
        }

        public int Size {
            get { return MethodImplRow.RowSize; }
        }

        public int Columns {
            get { return MethodImplRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}