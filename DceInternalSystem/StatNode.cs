using System;
using DCEAccessLib;
using System.Data;

namespace DCEInternalSystem
{
   /// <summary>
   /// ���� ����������
   /// </summary>
   public class StatNode : ChildNodeListControl
   {
      public StatNode (NodeControl parent)
         : base(parent)
      {
         new StatsTrainingListNode(this);
         new StatsStudentGroupsNode(this);
      }
      public override string GetCaption()
      {
         return "����������";
      }
      public override bool HaveChildNodes() { return false; }
   }
}