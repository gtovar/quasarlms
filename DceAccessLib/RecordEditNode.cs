using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DCEAccessLib
{
   public interface IEditNode
   {
      event RecordEditNode.EnumDataSetsHandler OnUpdateDataSet;
      event RecordEditNode.ReloadContentHandler OnReloadContent;
      void ChangeNotify();
   }

	public class EditRowContentBase
	{
	}
   /// <summary>
   /// ������������� ������, ���������������, ������� ������
   /// </summary>
   public class EditRowContent<TKey>
   {
      public EditRowContent(
         string querystring, 
         string tablename, 
         string idfield,
         TKey ident,
         string qualif)
      {
         queryString = querystring;
         tableName = tablename;
         id = ident;
         qualifier = qualif;
         idField = idfield;
      }

      public DataRowView editRow = null;
      public DataSet ds = null;
      public DataView dv = null;
      public bool IsNew = false;
      public string queryString;
      public string tableName;
		public TKey id;
      public string qualifier = string.Empty;
      public string idField = "id";
   }

   /// <summary>
   /// ���� ��� �������������� �������� ������
   /// </summary>
   public class RecordEditNode : NodeControl, IEditNode
   {
      /// <summary>
      /// ������� ��� ������������ ��������� � ������������� ������
      /// </summary>
      protected DataColumnChangeEventHandler RowChangeHandler;
      protected void OnRowChanging(object obj, DataColumnChangeEventArgs e)
      {
         if (!this.Changed)
         {
            this.Changed = true;
         }
      }
      public void ChangeNotify()
      {
         this.Changed=true;
      }

      private bool changed = false;
      public bool Changed {
         get {
            return this.changed;
         }
         set {
            if (changed != value) {
               changed = value;
               if (this.treeNode!=null) {
					  this.treeNode.ForeColor = changed ? System.Drawing.Color.DarkRed : System.Drawing.Color.Black;
               }
               CaptionChanged();
            }
         }
      }

      /// <summary>
      /// ���������� ����� ����� ������� Caption
      /// </summary>
      public override void CaptionChanged()
      {
         string caption = this.GetCaption();
         if (this.Changed && !this.IsNew)
            caption = "*" + caption;

         if (NodeControl.SelectedNode == this)
         {
            if (NodeControl.NodeLabel != null)
               NodeControl.NodeLabel.Text = caption;
         }
         if (this.treeNode != null)
         {
            this.treeNode.Text = caption;
         }
      }

      /// <summary>
      /// ������ �������� EditRowContent
      /// </summary>
      private ArrayList EditRowContents = new ArrayList();
      
      /// <summary>
      /// �� ������������� ���������� ������� �������������� �������
      /// </summary>
      public EditRowContent<string> GetEditContent(string qualif)
      {
         foreach(EditRowContent<string> cont in this.EditRowContents)
         {
            if (cont.qualifier == qualif)
               return cont;
         }
         return null;
      }

      /// <summary>
      /// ������� ������ ������ (������)
      /// </summary>
      public DataRowView EditRow
      {
         get 
         { 
            if (EditRowContents.Count == 0)
               return null;
            else
               return ((EditRowContent<string>)EditRowContents[0]).editRow; 
         }
      }
      
      /// <summary>
      /// ������������� ������� ������ (������)
      /// </summary>
      public string Id 
      {
         get 
         { 
            if (EditRowContents.Count == 0)
               return "";
            else
               return ((EditRowContent<string>)EditRowContents[0]).id; 
         }
      }

      public bool IsNew 
      {
         get 
         { 
            if (EditRowContents.Count == 0)
               return false;
            else
               return ((EditRowContent<string>)EditRowContents[0]).IsNew; 
         }
      }

      /// <summary>
      /// ������� ��� �������������� ��� ���������� ��������
      /// </summary>
      public delegate void ReloadContentHandler();

      /// <summary>
      /// ������� ��� �������������� ��� ���������� ��������� ��������
      /// </summary>
      public delegate bool EnumDataSetsHandler(string TransactionId, ref DCEAccessLib.DataSetBatchUpdate dataSet);

      /// <summary>
      /// ������� ��������� ��� ���������� ������ ����
      /// </summary>
      public event ReloadContentHandler OnReloadContent;

      /// <summary>
      /// ��������� ��������� ��������� 
      /// </summary>
      System.Collections.ArrayList EventTable = new System.Collections.ArrayList();
      
      /// <summary>
      /// ������� ������������� ��� ���������� ���� ��������� � ��� ������ (����������� ���������)
      /// ����� ������ �������� ����� �� ��������� ��� ������ ��� ���������� ������ ����
      /// </summary>
      public event EnumDataSetsHandler OnUpdateDataSet
      {
         add
         {
            EventTable.Add( value ); 
         }
         remove
         {
            EventTable.Remove( value );
         }
      }

      /// <summary>
      /// Initialize new record for editing (EditRow)
      /// </summary>
      protected virtual void InitNewRecord()
      {
      }
      
      /// <summary>
      /// ������������� ������� ������ ��� ��� ����������
      /// </summary>
      protected virtual void InitNewRecord(string qualifier)
      {
      }

      /// <summary>
      /// ���������� ������ ����
      /// </summary>
      public void EndEdit(bool cancel)
      {
         EndEdit(cancel,true);
      }

      /// <summary>
      /// Finish editing, closing the node
      /// </summary>
      public void EndEdit(bool cancel, bool close)
      {
         // �������� ���� �������� ������ �� ���������� ��������� ���������
         foreach (EditRowContent<string> cont in EditRowContents)
         {
            if (!cont.editRow.IsEdit && !cancel)
            {
//               throw new DCEException("Row " + this.EditRowContents.IndexOf(cont) 
//                  + " is not in edit state.", DCEException.ExceptionLevel.InvalidAction);
            }
         }
      
         if (cancel)
         {
            // ����� ��������� �������������� ��� ���� �������� ������ 
            // (��� ���������� �����������)
            foreach (EditRowContent<string> cont in EditRowContents)
            {
               cont.editRow.CancelEdit();
               if (cont.IsNew) cont.id = "";
            }
         }
         else
         {
            // ����� ��������� �������������� ��� ���� �������� ������ 
            // (� ����������� �����������)
            foreach (EditRowContent<string> cont in EditRowContents)
            {
               cont.editRow.EndEdit();
            }
            
            System.Collections.ArrayList  updates = new  System.Collections.ArrayList();
            
            // ��������� ���� �������� � ������ ��� �������� ���������
            foreach (EditRowContent<string> cont in EditRowContents)
            {
               DCEAccessLib.DataSetBatchUpdate dataSet = new DCEAccessLib.DataSetBatchUpdate();

               dataSet.sql = cont.queryString;
               dataSet.tableName = cont.tableName;
               dataSet.dataSet = cont.ds;
               updates.Add(dataSet);
               
               // ���������� ���� ��������� �������� ��� �������� ���������
               foreach (EnumDataSetsHandler handler in EventTable)
               {
                  dataSet = new DCEAccessLib.DataSetBatchUpdate();
                  if (handler(cont.qualifier, ref dataSet))
                  {
                     updates.Add(dataSet);               
                  }
               }
            }

            string[] sqls = new string[updates.Count];
            string[] tables = new string[updates.Count];
            DataSet[] datasets = new DataSet[updates.Count];

            for (int i=0; i<updates.Count;i++)
            {
               sqls[i] = ((DCEAccessLib.DataSetBatchUpdate)updates[i]).sql;
               tables[i] = ((DCEAccessLib.DataSetBatchUpdate)updates[i]).tableName;
               datasets[i] = ((DCEAccessLib.DataSetBatchUpdate)updates[i]).dataSet;
            }

            DCEAccessLib.DCEWebAccess.WebAccess.UpdateDataSets(sqls,tables,datasets);
            
            this.NodeParent.NeedRefresh();
         }
         
         if (close)
            Dispose();
         else
         {
            // ���������� ������� ������ � ������������� ������ ��� ���� ��������
            foreach (EditRowContent<string> cont in EditRowContents)
            {
               cont.ds = DCEAccessLib.DCEWebAccess.WebAccess.GetDataSet(cont.queryString, cont.tableName);
               cont.dv.Table = cont.ds.Tables[cont.tableName];
            }
            
            this.RebindEditRows();
            
            // ����������� �� ���������� ������ ����
            if (this.OnReloadContent != null)
               this.OnReloadContent();

            // ����������� ������������ ���� � ��� ��� ������ �������� ���� ����������
            if (this.NodeParent != null)
               this.NodeParent.ChildChanged(this);

            // ����������� �������� �� ��������� ������������ ����
            if (this.Nodes != null)
               foreach(NodeControl node in Nodes)
                  node.ParentChanged();

            // ���������� �������� ����
            if (this.treeNode !=null)
               this.CaptionChanged();
         }
      }

      public void RebindEditRows()
      {
         this.Changed = false;
         foreach (EditRowContent<string> cont in EditRowContents)
         {
			if (!string.IsNullOrEmpty(cont.id)) {
               cont.IsNew = false;
               cont.dv.Sort = cont.idField;
               DataRowView[] rows = cont.dv.FindRows(cont.id);
               
               if (rows.Length == 1)
               {
                  cont.editRow = rows[0];
                  cont.editRow.BeginEdit();
               }
               else
               {
                  if (rows.Length > 1)
                  {
                     throw new DCEException("���������� ������� ������������� ����� ��������� �������", DCEException.ExceptionLevel.InvalidAction);
                  }
                  else
                  {
                     MessageBox.Show("��� ���������� ������ ��������� ����� �������, ��������������� �������������. �������� ������ ���� ������� �� ���� ������.",
                        "��������������",
                        MessageBoxButtons.OK,MessageBoxIcon.Warning);
                     this.Dispose();
                     return;
                     //throw new DCEException(, DCEException.ExceptionLevel.InvalidAction);
                  }
               }
            }
            else
            {
               cont.IsNew = true;
               cont.editRow = cont.dv.AddNew();
            
               if (oldstyle)
                  InitNewRecord();
               else
                  InitNewRecord(cont.qualifier);
               
               cont.id = cont.editRow[cont.idField].ToString();
            }
            
            if (cont.editRow == null)
               throw new System.ApplicationException(); 

            cont.ds.Tables[cont.tableName].ColumnChanging += this.RowChangeHandler;
         }
      }

      private bool oldstyle = false;
      
      public static string GetFieldValue(string query, string tablename, string field, string id, string retfield)
      {
         DataSet s = DCEAccessLib.DCEWebAccess.WebAccess.GetDataSet(query, tablename);
         DataView v = new DataView(s.Tables[tablename]);
         v.Sort = field;
         DataRowView[] rows = v.FindRows(id);
         
         if (rows.Length > 0)
            return rows[0][retfield].ToString();
         else 
            return "";
      }

	public RecordEditNode(NodeControl parent, string query, string tablename, Guid _id)
			:base(parent)
	{
		oldstyle = true;
		this.EditRowContents.Add(new EditRowContent<string>(query, tablename, "id", _id.ToString(), string.Empty));
		InitializeRecordEditNode();
	}
      public RecordEditNode(NodeControl parent, string query, string tablename, string IdField, string _id)
         : base(parent)
      {
         oldstyle = true;

         this.EditRowContents.Add(new EditRowContent<string>(query, tablename, IdField, _id, string.Empty));

         InitializeRecordEditNode();
      }
      
      public RecordEditNode(NodeControl parent, EditRowContent<string>[] editcontents)
         : base(parent)
      {
         this.EditRowContents.AddRange(editcontents);

         InitializeRecordEditNode();
      }
      
      protected void InitializeRecordEditNode()
      {
         this.RowChangeHandler = new DataColumnChangeEventHandler(OnRowChanging);
         foreach (EditRowContent<string> cont in EditRowContents)
         {
            cont.ds = DCEAccessLib.DCEWebAccess.WebAccess.GetDataSet(cont.queryString, cont.tableName);
            cont.dv = new DataView(cont.ds.Tables[cont.tableName]);
         }
         this.RebindEditRows();
      }

      /// <summary>
      /// ���������� ������� � ������������ ��������� ��������
      /// </summary>
      protected override void DoDispose()
      {
         if (this.EditRowContents !=null)
         {
            foreach (EditRowContent<string> cont in EditRowContents)
            {
               if (cont.editRow != null)
               {
                  if (cont.editRow.IsEdit)
                     cont.editRow.CancelEdit();

                  cont.editRow = null;
               }
            
               if (cont.dv != null)
               {
                  cont.dv.Dispose();
                  cont.ds.Dispose();
                  cont.dv = null;
                  cont.ds = null;
                  cont.queryString = null;
                  cont.tableName = null;
                  cont.id = null;
               }
            }
            this.EditRowContents.Clear();
         }
         
         if ( EventTable != null)
         {
            EventTable.Clear();
            EventTable = null;
         }

         OnReloadContent = null;
         base.DoDispose();
      }
   }
}
