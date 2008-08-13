using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DCEAccessLib
{
   /// <summary>
   /// ������� ����� ����
   /// </summary>
   public class NodeControl : IDisposable
	{
      /// <summary>
      /// ������� �����, ������������ ������� �������� ����
      /// </summary>
      public static System.Windows.Forms.Label NodeLabel = null;
      /// <summary>
      /// ������ �� ������� ���������� ���� � ������
      /// </summary>
      protected static NodeControl selectedNode = null;
      /// <summary>
      /// ������� ���������� ���� � ������
      /// </summary>
      public static NodeControl SelectedNode
      {
         get
         {
            return selectedNode;
         }
         set
         {
            selectedNode = value;

            if (selectedNode != null)
               selectedNode.CaptionChanged();
         }
      }

      /// <summary>
      /// ������, ������������ � ���� �� ����� ���� �������������
      /// </summary>
      protected bool rdonly = true;
      public bool CanModify
      {
         get 
         {
            return !rdonly;
         }
      }

      public bool Readonly 
      {
         get 
         {
            return rdonly;
         }
      }

      /// <summary>
      /// ����������� ���� � ����� ��������� (������)
      /// </summary>
      public virtual void CaptionChanged()
      {
         if (NodeControl.SelectedNode == this)
         {
            if (NodeControl.NodeLabel != null)
               NodeControl.NodeLabel.Text = this.GetCaption();
         }
         if (this.treeNode != null)
         {
            this.treeNode.Text = this.GetCaption();
         }
      }
      /// <summary>
      /// ������ �� ������������ ����
      /// </summary>
      public NodeControl NodeParent;
      /// <summary>
      /// ������ �� �������, ��������� � �����
      /// </summary>
      protected System.Windows.Forms.UserControl fControl;
      /// <summary>
      /// ������ �� ������ �������� ���
      /// </summary>
      private System.Collections.ArrayList nodes;
      /// <summary>
      /// �������� ����
      /// </summary>
      public System.Collections.ArrayList Nodes
      {
         get
         {
            return nodes;
         }
      }

      /// <summary>
      /// ������ �� ���� �������� ���������� Treeview
      /// </summary>
      public TreeNode treeNode = null;

		public NodeControl( NodeControl parent)
		{
         nodes = new System.Collections.ArrayList();
         this.NodeParent = parent;
         
         if (!this.HaveChildNodes())
             IsExpandedOnce = true;

         if (parent != null)
         {
            this.treeNode = new System.Windows.Forms.TreeNode();
            treeNode.Tag = this;
            parent.Nodes.Add(this);
            parent.treeNode.Nodes.Add(this.treeNode);
            
            if (this.HaveChildNodes())
               treeNode.Nodes.Add("");
         }
         this.CaptionChanged();
      }
      
      /// <summary>
      /// ����������� ����� ������������ �������� ��������� � �����
      /// </summary>
      protected virtual void DoDispose()
      {
      }

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      public void Dispose( )
      {
         this.DoDispose();
         if (fControl != null)
         {
            fControl.Dispose();
            fControl = null;
         }
         if (NodeParent != null)
         {
            // Comment by dya
            //NodeParent.Select();

            NodeParent.nodes.Remove(this);
            NodeParent.treeNode.Nodes.Remove(this.treeNode);
            NodeParent = null;
         }
         if (nodes != null)
         {
            while (nodes.Count>0)
            {
               ((NodeControl)nodes[0]).Dispose();
            }
            nodes.Clear(); //already removed in foreach loop, but in case...
            nodes = null;
         }

         if (treeNode !=null)
         {
            treeNode.Nodes.Clear();
            treeNode.Tag = null;
            treeNode = null;
         }
      }

      /// <summary>
      /// ����, ����������� ���� �� ���� ���� ���� ��� ����������
      /// </summary>
      protected bool IsExpandedOnce = false;

      /// <summary>
      /// ������������� ���� �������� ����������
      /// </summary>
      public void ExpandTreeNode()
      {
         if (!this.treeNode.IsExpanded)
            this.treeNode.Expand();
      }
      
      /// <summary>
      /// ������������� ����, �������� ���� �������� ��� (���� ��� ����������)
      /// </summary>
      public void Expand()
      {
         if (!IsExpandedOnce)
         {
            IsExpandedOnce = true;
            for (int i=this.treeNode.Nodes.Count-1; i>=0; i--)
            {
               if (this.treeNode.Nodes[i].Tag == null)
               {
                  this.treeNode.Nodes.RemoveAt(i);
                  break;
               }
            }
            this.CreateChilds();
            ExpandTreeNode();
         }
      }

      /// <summary>
      /// ����������� ����� ��� �������� ���� �������� ��� 
      /// ��� ������� ��������� ������������ ����, ���� ���� 
      /// �� ���� ���������� �� ����� �������
      /// </summary>
      public virtual void CreateChilds()
      {

      }
      
      /// <summary>
      /// �������� � ��� �������� �� ������ ���� ������������, 
      /// �.�. ����� �� �� ������� ������� �������� ����
      /// </summary>
      public virtual bool CanClose()
      {
         return false;
      }

      /// <summary>
      /// ����, ����������� ������� ����������� ������������� ����������� ���� ��� ������
      /// </summary>
      protected bool needRefresh = false;

      /// <summary>
      /// ����������� ������������ ���� � ������������� 
      /// ����������� �������� ����, ��������� � ���������
      /// </summary>
      public virtual void ChildChanged(NodeControl child)
      {
         this.needRefresh = true;
      }
      
      /// <summary>
      /// ����������� �������� ���� � ������������� 
      /// �������� ���� ���������, ��������� �� ������������ ����
      /// </summary>
      public virtual void ParentChanged()
      {
         this.needRefresh = true;
      }
      
      /// <summary>
      /// ���������� ����, ��� �� ������� ��� 
      /// ��������� ������ ������ ���������� ������
      /// </summary>
      public void NeedRefresh()
      {
         needRefresh = true;
      }

      /// <summary>
      /// ��������� ����, ������� ���������� ������ ����
      /// </summary>
      public void Select()
      {
         if (this.treeNode !=null)
         {
            if (this.treeNode.TreeView !=null)
               this.treeNode.TreeView.SelectedNode = this.treeNode;
         }
         NodeControl.SelectedNode = this;
      }
      
      /// <summary>
      /// �������� ������� ���� ��� ����������� � �����
      /// </summary>
      /// 
      public virtual System.Windows.Forms.UserControl GetControl()
      {
         return null;
      }

      /// <summary>
      /// ���� ����������� ���� ���������� ������� ��������� � ���������
      /// ��� ��� �� ����� �� �����
      /// </summary>
      public virtual void ReleaseControl()
      {
         if (this.fControl != null)
         {
            this.fControl.Dispose();
            this.fControl = null;
         }
      }

      /// <summary>
      /// ����������� ����� ����������� �������
      /// </summary>
      public virtual string GetCaption()
      {
         return "����������� ����";
      }

      public string Caption
      {
         get 
         {
            return GetCaption();
         }
      }
   
      /// <summary>
      /// ����������� ����� ���������� � ��� �������� �� ������ ����������
      /// �������� ����, ���� ��� ����� ���������� �����������������
      /// � ���������� �����
      /// </summary>
      public virtual bool HaveChildNodes() { return false; }

      /// <summary>
      /// ��������� �������, ������������ �� ������� ������ ������� ���� �� ����
      /// </summary>
      protected ArrayList menuItemCollection;

      /// <summary>
      /// ���������� ����������� ����
      /// </summary>
      public virtual ArrayList GetPopupMenu()
      {
         if (menuItemCollection == null)
            menuItemCollection = new ArrayList();

         menuItemCollection.Clear();

         return menuItemCollection;
      }

      /// <summary>
      /// ����������� ����������� ����
      /// </summary>
      public virtual void ReleasePopupMenu()
      {
      }
      
      /// <summary>
      /// Get parent node with type Type
      /// </summary>
      public NodeControl GetSpecifiedParentNode(string type)
      {
         NodeControl node = this.NodeParent;

         while (node != null && node.GetType().ToString() != type)
            node = node.NodeParent;

         return node;
      }
   }
}
