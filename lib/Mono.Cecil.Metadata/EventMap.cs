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

    [RId (0x12)]
    internal sealed class EventMapTable : IMetadataTable {

        private RowCollection m_rows;

        public EventMapRow this [int index] {
            get { return m_rows [index] as EventMapRow; }
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

    internal sealed class EventMapRow : IMetadataRow {

        public static readonly int RowSize = 8;
        public static readonly int RowColumns = 2;

        [Column] private uint m_parent;
        [Column] private uint m_eventList;

        public uint Parent {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public uint EventList {
            get { return m_eventList; }
            set { m_eventList = value; }
        }

        public int Size {
            get { return EventMapRow.RowSize; }
        }

        public int Columns {
            get { return EventMapRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}