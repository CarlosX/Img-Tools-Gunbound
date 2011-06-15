using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImgConvert
{
    public class FolderListItem : ListViewItem
    {

        private TreeNode m_Node;

        public TreeNode Node
        {
            get
            {
                return m_Node;
            }
        }

        public FolderListItem(TreeNode node)
            : base(new string[] { node.Text, "", "File Folder" })
        {
            string[] sArr = new string[] {
                                           node.Text, 
                                           "", 
                                           "File Folder" };
            ImageIndex = 0;
            m_Node = node;
        }

    } // class FolderListItem

}
