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

    [RId (0x00)]
    internal sealed class ModuleTable : IMetadataTable {

        private RowCollection m_rows;

        public ModuleRow this [int index] {
            get { return m_rows [index] as ModuleRow; }
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

    internal sealed class ModuleRow : IMetadataRow {

        public static readonly int RowSize = 18;
        public static readonly int RowColumns = 5;

        [Column] private ushort m_generation;
        [Column] private uint m_name;
        [Column] private uint m_mvid;
        [Column] private uint m_encId;
        [Column] private uint m_encBaseId;

        public ushort Generation {
            get { return m_generation; }
            set { m_generation = value; }
        }

        public uint Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public uint Mvid {
            get { return m_mvid; }
            set { m_mvid = value; }
        }

        public uint EncId {
            get { return m_encId; }
            set { m_encId = value; }
        }

        public uint EncBaseId {
            get { return m_encBaseId; }
            set { m_encBaseId = value; }
        }

        public int Size {
            get { return ModuleRow.RowSize; }
        }

        public int Columns {
            get { return ModuleRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}