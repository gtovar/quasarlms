<?xml version="1.0" encoding="windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" encoding="unicode" omit-xml-declaration="yes" indent="no"/>


   <xsl:template name="formatDate">
	   <xsl:param name="strDate"/>
	   <!--2003-06-11T17:41:37.0630000+03:00
		    123456789012345678901234567890123
		    0        1         2         3-->
      <nobr>
	   <xsl:value-of select="substring($strDate, 9, 2)"/>.<xsl:value-of select="substring($strDate, 6, 2)"/>.<xsl:value-of select="substring($strDate, 1, 4)"/></nobr>
   </xsl:template>
   
   <xsl:template name="formatDateTime">
	   <xsl:param name="strDate"/>
	   <!--2003-06-11T17:41:37.0630000+03:00
		   123456789012345678901234567890123
		   0        1         2         3-->
      <nobr>
	   <xsl:value-of select="substring($strDate, 9, 2)"/>.<xsl:value-of select="substring($strDate, 6, 2)"/>.<xsl:value-of select="substring($strDate, 1, 4)"/>
	   &#160;<xsl:value-of select="substring($strDate, 12, 8)"/>
	   </nobr>
   </xsl:template>


   <xsl:template name="Student" match="DataSet/Student[id]">
      �������: 
      <xsl:value-of select="LastName"/>&#160;
      <xsl:value-of select="FirstName"/>&#160;
      <xsl:value-of select="Patronymic"/>&#160;
      <br/>��������� ����: 
      <xsl:call-template name="formatDateTime"> 
         <xsl:with-param name="strDate" select="LastLogin" />
      </xsl:call-template>
      <br/>����� ���������� ������: <xsl:value-of select="TotalLogins"/>
   </xsl:template>
   
   <xsl:template name="Training" match="/xml/DataSet/Training">
      �������:
      <xsl:value-of select="TName"/>&#160;
      <br/> 
      ��� ��������:
      <xsl:value-of select="Code"/>
   </xsl:template>


   <xsl:template name="boolval">
      <xsl:param name="node" />
      <xsl:for-each select="$node">
         <xsl:choose>
         <xsl:when test="$node='true'">��</xsl:when>
         <xsl:otherwise>���</xsl:otherwise>
         </xsl:choose>
      </xsl:for-each>
   </xsl:template>
   
   <xsl:template name="Test">
      <tr bgColor="#FFFFFF">
         <td>
         <xsl:if test= "Parent = /xml/DataSet/Training/Course" >
            �������� ����
         </xsl:if>
         <xsl:if test= "Parent != /xml/DataSet/Training/Course" >
            <xsl:value-of select="TestName"/>
         </xsl:if>
         </td>
         <td>
            <xsl:call-template name="boolval">
            <xsl:with-param name="node" select="Complete"/>
            </xsl:call-template>
         </td>
         <td>
            <xsl:call-template name="boolval">
            <xsl:with-param name="node" select="Skipped"/>
            </xsl:call-template>
         </td>
         <td><xsl:value-of select="Tries"/></td>
         <td><xsl:value-of select="AllowTries"/></td>
         <td>
            <xsl:call-template name="formatDateTime"> 
               <xsl:with-param name="strDate" select="TryStart" />
            </xsl:call-template>
         </td>
         <td><xsl:value-of select="Points"/></td>
      </tr>
   </xsl:template>
   
   <xsl:template name="Practice">
      <tr bgColor="#FFFFFF">
         <td><xsl:value-of select="TestName"/></td>
         <td>
            <xsl:call-template name="boolval">
            <xsl:with-param name="node" select="Complete"/>
            </xsl:call-template>
         </td>
         <td>
            <xsl:call-template name="formatDateTime"> 
               <xsl:with-param name="strDate" select="CompletionDate" />
            </xsl:call-template>
         </td>
      </tr>
   </xsl:template>

   
   <xsl:template name="Task">
      <tr  bgColor="#FFFFFF">
         <td><xsl:value-of select="TaskName"/></td>
         <td>
            <xsl:call-template name="formatDate"> 
               <xsl:with-param name="strDate" select="TaskTime" />
            </xsl:call-template>
         </td>
         <td>
            <xsl:call-template name="formatDateTime"> 
               <xsl:with-param name="strDate" select="SDate" />
            </xsl:call-template>
         </td>
         <td>
            <xsl:choose>
               <xsl:when test="Complete='0'">�� ���������</xsl:when>
               <xsl:when test="Complete='1'">�� ���������</xsl:when>
               <xsl:when test="Complete='2'">�����������</xsl:when>
               <xsl:when test="Complete='3'">��������</xsl:when>
               <xsl:when test="Complete='4'">� ��������</xsl:when>
               <xsl:when test="Complete='5'">���������</xsl:when>
            </xsl:choose>
         </td>
      </tr>
   </xsl:template>
 
   
   <xsl:template match="/">
      
         <H1>���������� ��������</H1>
         ���� ��������� ������: <xsl:value-of select="xml/StatDate"/>
         <br/>   <br/>
         <xsl:apply-templates select="/xml/DataSet/Training"/>
         

         <xsl:for-each select="xml/StudentReport">
            <br/>
            <br/>
            <hr noshade="true"/>
            <xsl:apply-templates select="DataSet/Student[id]"/>
            <br/>
            <br/>

            <xsl:if test="count(DataSet/Tests[Type='1']) >0">    
                  <table cellspacing="1" cellpadding="2" border="0" width="100%" bgColor="#000000">
                  <tr ><th colspan="7"><font color="#FFFFFF">�����</font></th></tr>
                  <tr bgColor="#BFBFBF"><th rowspan="2">����</th><th rowspan="2">����</th><th rowspan="2">��������</th><th rowspan="2">�������</th>
                     <th rowspan="2">�������� �������</th><th colspan="2">��������� �������</th></tr>
                     <tr bgColor="#BFBFBF"><th>����</th><th>�����</th></tr>
                     <xsl:for-each select="DataSet/Tests[Type='1']">
                        <xsl:call-template name="Test"/>
                     </xsl:for-each>
                  </table>
                  
            <br/><br/>
            </xsl:if>

            <xsl:if test="count(DataSet/Tests[Type='2']) >0">
                  <table cellspacing="1" cellpadding="2" border="0" width="100%" bgColor="#000000">
                  <tr><th colspan="4"><font color="#FFFFFF">������������ ������</font></th></tr>
                  <tr bgColor="#BFBFBF">
                     <th>����</th><th>���������</th><th>���� ����������</th>
                  </tr>
                     <xsl:for-each select="DataSet/Tests[Type='2']">
                        <xsl:call-template name="Practice"/>
                     </xsl:for-each>
                  </table>
            <br/><br/>
            </xsl:if>

            <xsl:if test="count(DataSet/Tasks) >0">
                  <table cellspacing="1" cellpadding="2" border="0" width="100%" bgColor="#000000">
                  <tr ><th colspan="4" ><font color="#FFFFFF">�������</font></th></tr>
                  <tr bgColor="#BFBFBF">
                     <th>��������</th><th>��������� ��..</th><th>���� ����������</th><th>������ ����������</th>
                  </tr>
                     <xsl:for-each select="DataSet/Tasks">
                        <xsl:call-template name="Task"/>
                     </xsl:for-each>
                  </table>
            </xsl:if>
<!-- Forum stats -->
            <br/><br/>
            <table cellspacing="1" cellpadding="2" border="0" width="100%" bgColor="#000000">
            <tr ><th colspan="2" ><font color="#FFFFFF">����� ��������</font></th></tr>
            <tr bgColor="#BFBFBF"><th>���������� ������� �����</th><th>���������� �������</th></tr>
            <tr bgColor="#FFFFFF"><td><xsl:value-of select="ForumTopicsStarted"/></td><td><xsl:value-of select="ForumReplies"/></td></tr>
            </table>
                        
         </xsl:for-each>

   </xsl:template>
</xsl:stylesheet>
