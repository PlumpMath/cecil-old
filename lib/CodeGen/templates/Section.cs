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
<% header = $headers["Section"] %>
namespace Mono.Cecil.Binary {

    internal sealed class Section : IHeader, IBinaryVisitable {

<% header.fields.each { |f| %>        private <%=f.type%> <%=f.field_name%>;<% print("\n") } %>
        private string m_name;

        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }

<% header.fields.each { |f| %>        public <%=f.type%> <%=f.property_name%> {
            get { return <%=f.field_name%>; }
            set { <%=f.field_name%> = value; }
        }
<% print("\n") } %>        public Section ()
        {
        }

        public void SetDefaultValues ()
        {<% header.fields.each { |f| print("\n            " +  f.field_name + " = " + f.default + ";") unless f.default.nil? } %>
        }

        public void Accept (IBinaryVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
