﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Reporting/UI/Layouts/Top+SubMenu.Master"
    CodeBehind="CurriculumList.aspx.cs" Inherits="N2.Calendar.Curriculum.UI.Views.CurriculumList" %>
<%@ Register src="Curriculum.ascx" tagname="Curriculum" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TextContent" runat="Server">

    <script type="text/javascript">

        $(function() {
           if(<%= (!IsPostBack).ToString().ToLower() %>) 
            {
            }
            
             $('#add').hide();

             $('#shAdd').click(
            function() {
               $('#add').show('slow');
            });           

            $('#<%= this.btnAddTUP.ClientID %>').click(
            function() {
               $('#add').hide();
            });


        });
     
    </script>

    <% if (this.CourseInfos.Any() )
{%>
<table>
<tr>
<td >
 <asp:DropDownList ID="ddltup" runat="server" AutoPostBack=true
       onselectedindexchanged="ddltup_SelectedIndexChanged">
    </asp:DropDownList>  
    <asp:ImageButton ID="btSave" runat="server" onclick="btnSave_Click" 
        ImageUrl="../../../Edit/img/ico/png/accept.png" Visible="False"/>
    <asp:ImageButton ID="btnDelTUP" runat="server" onclick="btnDelTUP_Click" ImageUrl="../../../Edit/img/ico/png/delete.png"/>
    <img id= "shAdd" src="../../../Edit/img/ico/png/add.png" />
 
 
            
<%--    <img id="added" src="../../../Lms/UI/Img/database.png" />
--%>    


</td>
<td id = "add">
       <asp:TextBox ID="txtAddTUP" runat="server"></asp:TextBox>
       <asp:Button ID="btnAddTUP" runat="server"  Text="OK" 
        onclick="btnAddTUP_Click" />

</td>
</tr>
 <tr>
<td colspan=2>
                <uc1:Curriculum ID="CurrentCurriculum" runat="server" OnChanged = "cc_Changed" />
</td>
</tr>
<%-- <tr>
<td colspan=2>
      <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" 
        Text="Сохранить" />
</td>
</tr>
--%>  
    </table>
    <% }
       else
       {
    %>
    <div>
        не найдены курсы
        <br />
        <br />
        <br />
    </div>
    <%  }    %> 
 


</asp:Content>
