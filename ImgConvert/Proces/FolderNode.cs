using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ImgConvert
{
    public class FolderNode : TreeNode
    {

        private class InternalComparer : IComparer
        {

            public static readonly FolderNode.InternalComparer Instance;

            private InternalComparer()
            {
            }

            static InternalComparer()
            {
                FolderNode.InternalComparer.Instance = new FolderNode.InternalComparer();
            }

            public int Compare(object x, object y)
            {
                bool flag1 = x is FolderNode;
                bool flag2 = y is FolderNode;
                if (flag1 && !flag2)
                {
                    return -1;
                }
                if (flag2 && !flag1)
                {
                    return 1;
                }
                return ((TreeNode)x).Text.CompareTo(((TreeNode)y).Text);
            }

        } // class InternalComparer

        private Archive m_Archive;
        private ArrayList m_Files;
        private ArrayList m_Folders;

        public Archive Archive
        {
            get
            {
                return m_Archive;
            }
        }

        public ArrayList Files
        {
            get
            {
                return m_Files;
            }
        }

        public ArrayList Folders
        {
            get
            {
                return m_Folders;
            }
        }

        public FolderNode(string name)
            : base(name)
        {
            ImageIndex = 0;
            SelectedImageIndex = 1;
            m_Folders = new ArrayList();
            m_Files = new ArrayList();
        }

        public FolderNode(Archive archive)
            : base(archive.Name)
        {
            m_Folders = new ArrayList();
            m_Files = new ArrayList();
            ImageIndex = 0;
            SelectedImageIndex = 1;
            m_Archive = archive;
            ArrayList arrayList1 = new ArrayList();
            ArchivedFile[] archivedFileArr = archive.Files;
            for (int i1 = 0; i1 < archivedFileArr.Length; i1++)
            {
                ArchivedFile archivedFile = archivedFileArr[i1];
                string s1 = Path.GetExtension(archivedFile.FileName);
                string[] sArr1 = null;
                FolderInfo folderInfo = FolderInfo.GetFolder(archivedFile.FileName);
                if (folderInfo != null)
                {
                    sArr1 = folderInfo.Path;
                }
                else
                {
                    string[] sArr2 = new string[] { s1 };
                    sArr1 = sArr2;
                }
                ArrayList arrayList2 = arrayList1;
                FolderNode folderNode1 = null;
                for (int i2 = 0; i2 < sArr1.Length; i2++)
                {
                    string s2 = sArr1[i2];
                    FolderNode folderNode2 = null;
                    for (int i3 = 0; i3 < arrayList2.Count; i3++)
                    {
                        FolderNode folderNode3 = (FolderNode)arrayList2[i3];
                        if (folderNode3.Text == s2)
                        {
                            folderNode2 = folderNode3;
                            break;
                        }
                    }
                    if (folderNode2 == null)
                    {
                        folderNode2 = new FolderNode(s2);
                        arrayList2.Add(folderNode2);
                        if (folderNode1 != null)
                            folderNode1.Nodes.Add(folderNode2);
                    }
                    folderNode1 = folderNode2;
                    arrayList2 = folderNode1.Folders;
                }
                int i4 = GetFileIcon(s1);
                archivedFile.Icon = i4;
                archivedFile.Folder = folderNode1;
                TreeNode treeNode = new FileNode(archivedFile, i4);
                archivedFile.Node = new FileNode(archivedFile, i4);
                folderNode1.Files.Add(treeNode);
            }
            for (int i5 = 0; i5 < arrayList1.Count; i5++)
            {
                FolderNode folderNode4 = (FolderNode)arrayList1[i5];
                folderNode4.RecurseSort();
            }
            if (arrayList1.Count == 1)
            {
                FolderNode folderNode5 = (FolderNode)arrayList1[0];
                m_Folders = folderNode5.Folders;
                m_Files = folderNode5.Files;
                for (int i6 = 0; i6 < folderNode5.Nodes.Count; i6++)
                {
                    FolderNode folderNode6 = (FolderNode)folderNode5.Nodes[i6];
                    for (int i7 = 0; i7 < folderNode6.Files.Count; i7++)
                    {
                        ((FileNode)folderNode6.Files[i7]).ArchivedFile.Folder = this;
                    }
                    Nodes.Add(folderNode6);
                }
            }
            else
            {
                m_Folders = arrayList1;
                for (int i8 = 0; i8 < arrayList1.Count; i8++)
                {
                    Nodes.Add((TreeNode)arrayList1[i8]);
                }
            }
        }

        public int GetFileIcon(string extension)
        {
            switch (extension)
            {
                case ".img":
                    return 0;

                case ".xes":
                    return 1;

                case ".xtf":
                    return 0;

                case ".epa":
                    return 0;

                case ".mp3":
                    return 2;

                case ".txt":
                    return 3;

                case ".fnt":
                    return 4;

                case ".spr":
                    return 0;

                case ".lnd":
                    return 0;

                case ".dat":
                    return 5;
            }
            return 5;
        }

        public void RecurseSort()
        {
            ArrayList arrayList = new ArrayList(Nodes);
            arrayList.Sort(FolderNode.InternalComparer.Instance);
            Nodes.Clear();
            Nodes.AddRange((TreeNode[])arrayList.ToArray(typeof(TreeNode)));
            for (int i = 0; i < m_Folders.Count; i++)
            {
                ((FolderNode)m_Folders[i]).RecurseSort();
            }
        }

    } // class FolderNode
}
