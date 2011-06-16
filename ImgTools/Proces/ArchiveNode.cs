/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       ArchiveNode.cs
 *  Author:     CARLOSX
 *  ----------------------------------------------------------------------------
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ImgTools
{
    public class ArchiveNode : TreeNode
    {

        private Archive m_Archive;

        public Archive Archive
        {
            get
            {
                return m_Archive;
            }
        }

        public ArchiveNode(Archive archive)
            : base(archive.Name)
        {
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
                folderNode1.Files.Add(new FileNode(archivedFile, i4));
            }
            for (int i5 = 0; i5 < arrayList1.Count; i5++)
            {
                FolderNode folderNode4 = (FolderNode)arrayList1[i5];
                folderNode4.RecurseSort();
            }
            if (arrayList1.Count == 1)
            {
                TreeNode treeNode = (TreeNode)arrayList1[0];
                for (int i6 = 0; i6 < treeNode.Nodes.Count; i6++)
                {
                    Nodes.Add(treeNode.Nodes[i6]);
                }
            }
            else
            {
                for (int i7 = 0; i7 < arrayList1.Count; i7++)
                {
                    Nodes.Add((TreeNode)arrayList1[i7]);
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


    } // class ArchiveNode
}