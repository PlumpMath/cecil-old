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
 * Mon Jan 10 00:15:50 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Binary {

    internal sealed class CLIHeader : IHeader, IBinaryVisitable {

        private uint m_cb;
        private ushort m_majorRuntimeVersion;
        private ushort m_minorRuntimeVersion;
        private DataDirectory m_metadata;
        private RuntimeImage m_flags;
        private uint m_entryPointToken;
        private DataDirectory m_resources;
        private DataDirectory m_strongNameSignature;
        private DataDirectory m_codeManagerTable;
        private DataDirectory m_vTableFixups;
        private DataDirectory m_exportAddressTableJumps;
        private DataDirectory m_managedNativeHeader;

        private byte[] m_imageHash;

        public uint Cb {
            get { return m_cb; }
            set { m_cb = value; }
        }

        public ushort MajorRuntimeVersion {
            get { return m_majorRuntimeVersion; }
            set { m_majorRuntimeVersion = value; }
        }

        public ushort MinorRuntimeVersion {
            get { return m_minorRuntimeVersion; }
            set { m_minorRuntimeVersion = value; }
        }

        public DataDirectory Metadata {
            get { return m_metadata; }
            set { m_metadata = value; }
        }

        public RuntimeImage Flags {
            get { return m_flags; }
            set { m_flags = value; }
        }

        public uint EntryPointToken {
            get { return m_entryPointToken; }
            set { m_entryPointToken = value; }
        }

        public DataDirectory Resources {
            get { return m_resources; }
            set { m_resources = value; }
        }

        public DataDirectory StrongNameSignature {
            get { return m_strongNameSignature; }
            set { m_strongNameSignature = value; }
        }

        public DataDirectory CodeManagerTable {
            get { return m_codeManagerTable; }
            set { m_codeManagerTable = value; }
        }

        public DataDirectory VTableFixups {
            get { return m_vTableFixups; }
            set { m_vTableFixups = value; }
        }

        public DataDirectory ExportAddressTableJumps {
            get { return m_exportAddressTableJumps; }
            set { m_exportAddressTableJumps = value; }
        }

        public DataDirectory ManagedNativeHeader {
            get { return m_managedNativeHeader; }
            set { m_managedNativeHeader = value; }
        }

        public byte[] ImageHash {
            get { return m_imageHash; }
            set { m_imageHash = value; }
        }

        public CLIHeader() {}

        public void SetDefaultValues() {
            m_codeManagerTable = DataDirectory.Zero;
            m_exportAddressTableJumps = DataDirectory.Zero;
            m_managedNativeHeader = DataDirectory.Zero;
        }

        public void Accept(IBinaryVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
