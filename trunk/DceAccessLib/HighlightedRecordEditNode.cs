using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DCEAccessLib
{
   /// <summary>
   /// ���� ��� �������������� �������� ������ � ������������ ��������������� 
   /// ���������� ������ � ���������� �������� ��������� ����
   /// </summary>
   public class HighlightedRecordEditNode : RecordEditNode
   {/*
      public HighlightedRecordEditNode(NodeControl parent, EditRowContent[] editcontents, bool readOnly)
         : base(parent, editcontents)
      {
         this.rdonly = readOnly;
      }*/
      
      public HighlightedRecordEditNode(NodeControl parent, string query, string tablename, string IdField, string _id, bool readOnly)
         : base(parent, query, tablename, IdField, _id)
      {
         this.rdonly = readOnly;
      }

      public bool IsNodeDirty
      {
         get { return this.Changed; }
         set 
         { 
            this.Changed = value;
         }
      }
      
      public override ArrayList GetPopupMenu()
      {
         int Count = base.GetPopupMenu().Count;

         #region initmenu code

         MenuItem SaveItem = new MenuItem("���������", new EventHandler(SaveItem_Click));
         MenuItem ResetItem = new MenuItem("��������", new EventHandler(ResetItem_Click));
         MenuItem Separator = new MenuItem("-");

         if (Count != 0)
            this.menuItemCollection.Add(Separator);

         this.menuItemCollection.Add(SaveItem);
         this.menuItemCollection.Add(ResetItem);
         
         if (this.CanModify)
            SaveItem.Visible = false;

         if (rdonly)
            SaveItem.Enabled = false;
            
         #endregion
         
         return this.menuItemCollection;
      }
      
      private void SaveItem_Click(object sender, System.EventArgs e)
      {
         Save();
      }         
      
      private void ResetItem_Click(object sender, System.EventArgs e)
      {
         Reset();
      }

      /// <summary>
      /// ���������� ������ ���� 
      /// </summary>
      public void Save()
      {
         if (this.CanModify)
         {
            this.EndEdit(false, false);
            IsNodeDirty = false;
         }
         else
         {
            Reset();
         }
      }
      
      /// <summary>
      /// ���������� ������ ���� (��������� ��������� �� �����������)
      /// </summary>
      public void Reset()
      {
         if (this.IsNew && this.CanClose())
            this.EndEdit(true, true);
         else
         {
            this.EndEdit(true, false);
            IsNodeDirty = false;
         }
      }

      /// <summary>
      /// ���������� ���� "�������" �������� ���
      /// </summary>
      public void SaveChilds()
      {
         foreach (NodeControl node in Nodes)
         {
            if (node is HighlightedRecordEditNode)
            {
               HighlightedRecordEditNode hrenode = node as HighlightedRecordEditNode;
               
               hrenode.SaveChilds();
               
               if (hrenode.IsNodeDirty)
                  hrenode.Save();
            }
         }
      }
      
      /// <summary>
      /// ���������� ���� "�������" �������� ��� (��������� ��������� �� �����������)
      /// </summary>
      public void ResetChilds()
      {
         foreach (NodeControl node in Nodes)
         {
            if (node is HighlightedRecordEditNode)
            {
               HighlightedRecordEditNode hrenode = node as HighlightedRecordEditNode;
               
               hrenode.ResetChilds();
               
               hrenode.Reset();
            }
         }
      }

      /// <summary>
      /// �������� ���� �� ����� �������� ��� "�������"
      /// </summary>
      public bool IsDirtyChilds()
      {
         foreach (NodeControl node in Nodes)
         {
            if (node is HighlightedRecordEditNode)
            {
               HighlightedRecordEditNode hrenode = node as HighlightedRecordEditNode;
               
               if (hrenode.IsDirtyChilds())
                  return true;
               
               if (hrenode.IsNodeDirty) 
                  return true;
            }
         }
         
         return false;
      }
   }
}
