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
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    using System;
    using System.Collections;
    using System.IO;

    using Mono.Cecil.Binary;

    internal sealed class MetadataRowReader : IMetadataRowVisitor {

        private MetadataTableReader m_mtrv;
        private BinaryReader m_binaryReader;
        private MetadataRoot m_metadataRoot;
        private IDictionary m_codedIndexCache;

        public MetadataRowReader (MetadataTableReader mtrv)
        {
            m_mtrv = mtrv;
            m_binaryReader = mtrv.GetReader ();
            m_metadataRoot = mtrv.GetMetadataRoot ();
            m_codedIndexCache = new Hashtable ();
        }

        private int GetIndexSize (Type table)
        {
            return m_mtrv.GetNumberOfRows (table) < (1 << 16) ? 2 : 4;
        }

        private int GetCodedIndexSize (CodedIndex ci)
        {
            int bits = 0, max = 0;
            if (m_codedIndexCache [ci] != null)
                return (int) m_codedIndexCache [ci];

            int res = 0;
            ArrayList tables = new ArrayList ();
            switch (ci) {
<% $coded_indexes.each { |ci| %>            case CodedIndex.<%=ci.name%> :
                bits = <%=ci.size%>;
<%    ci.tables.each { |tbl|
%>                tables.Add (typeof (<%=tbl.name%>Table));
<%    }
%>                break;
<% } %>           }
            foreach (Type t in tables) {
                int rows = m_mtrv.GetNumberOfRows (t);
                if (rows > max) max = rows;
            }
            res = max < (1 << (16 - bits)) ? 2 : 4;
            m_codedIndexCache [ci] = res;
            return res;

        }

        private uint ReadByIndexSize (int size)
        {
            if (size == 2) {
                return (uint) m_binaryReader.ReadUInt16 ();
            } else if (size == 4) {
                return m_binaryReader.ReadUInt32 ();
            } else {
                throw new MetadataFormatException ("Non valid size for indexing");
            }
        }

        private MetadataToken GetMetadataToken (CodedIndex cidx, uint data)
        {
            uint rid = 0;
            switch (cidx) {
<% $coded_indexes.each { |ci| %>            case CodedIndex.<%=ci.name%> :
                rid = data >> <%=ci.size%>;
                switch (data & <%=(2 ** ci.size.to_i - 1).to_s%>) {
<%    ci.tables.each { |tbl|
        name = tbl.name
        if (name == "DeclSecurity")
            name = "Permission"
        elsif (name == "StandAloneSig")
            name = "Signature"
        end
%>                case <%=tbl.tag%> :
                    return new MetadataToken (TokenType.<%=name%>, rid);
<%    }
%>                default :
                    throw new MetadataFormatException("Non valid tag for <%=ci.name%>");
                }
<% } %>            default :
                throw new MetadataFormatException ("Non valid CodedIndex");
            }
        }

        public void Visit (RowCollection coll) {}

<% $tables.each { |table| %>        public void Visit (<%=table.row_name%> row)
        {
<% table.columns.each { |col|
 if (col.target.nil?)
%>            row.<%=col.property_name%> = <%=col.read_binary("m_binaryReader")%>;
<% elsif (col.target == "BlobHeap")
%>            row.<%=col.property_name%> = ReadByIndexSize (m_metadataRoot.Streams.BlobHeap.IndexSize);
<% elsif (col.target == "StringsHeap")
%>            row.<%=col.property_name%> = ReadByIndexSize (m_metadataRoot.Streams.StringsHeap.IndexSize);
<% elsif (col.target == "GuidHeap")
%>            row.<%=col.property_name%> = ReadByIndexSize (m_metadataRoot.Streams.GuidHeap.IndexSize);
<% elsif (col.type == "MetadataToken")
%>            row.<%=col.property_name%> = GetMetadataToken (CodedIndex.<%=col.target%>,
                ReadByIndexSize (GetCodedIndexSize (CodedIndex.<%=col.target%>)));
<% else
%>            row.<%=col.property_name%> = ReadByIndexSize (GetIndexSize (typeof (<%=col.target%>Table)));
<% end
}%>        }
<%  print("\n") ; } %>        public void Terminate (RowCollection coll)
        {
        }
    }
}