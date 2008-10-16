<%@ Page Language="C#" MasterPageFile="~/Reporting/UI/Layouts/Top+SubMenu.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Reporting_UI_Report" %>
<%@ Import Namespace="System.Linq" %>

<asp:Content  ID="Content1" ContentPlaceHolderID="TextContent" Runat="Server">

    <div style="width:100%">
        <table>
            <tr>
                <td align="left">
                    <asp:DropDownList ID="ddlReportType" runat="server">
                        <asp:ListItem value="erv" selected="True">
                            �������������� ����������� ���������
                        </asp:ListItem>
                        <asp:ListItem value="svrs">
                            ������� ��������� �� ����������� ������
                        </asp:ListItem>
                        <asp:ListItem value="oaz">
                            ������ �� ������������� ���������������
                        </asp:ListItem>
                        <asp:ListItem value="pv">
                            ��������������� ���������
                        </asp:ListItem>
                        <asp:ListItem value="oes">
                            ������ �� ��������������� �������
                        </asp:ListItem>
                        <asp:ListItem value="oz">
                            ����� �� �������
                        </asp:ListItem>
                        <asp:ListItem value="irp">
                            ���������� � ������������� �������
                        </asp:ListItem>
                     </asp:DropDownList>
                     
                     <asp:Button ID="btnGet" runat="server" Text="������������" 
                        onclick="btnGet_Click" />

                </td>
            </tr>
            
        </table>
        <br />
        <asp:Label ID="lbl" runat="server" Text="-----------"></asp:Label>
        
        
 	<% if (this.Requests.Any())
    { %>
    <table>
        <tr><th>��������</th></tr>
	<% foreach (var _req in this.Requests)
    { %>
        <tr>
            <td>
            
           <a href='<%= _req.TemplateUrl %>'><%= _req.Title%></a>
       
            </td>
        </tr>    
             
	<% } %>
	</table>
	<% }
    else
    { %>
    no Requests
	<% } %>       
        
        
        
        
        </div>

</asp:Content>