<%@ Page Language="C#" MasterPageFile="~/Reporting/UI/Layouts/Top+SubMenu.Master"
    AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Reporting_UI_Report" %>

<%@ Import Namespace="System.Linq" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="N2.Futures" Namespace="N2.Web.UI.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="N2.Futures" Namespace="N2.Web.UI.WebControls.Test" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TextContent" runat="Server">
    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="false">

       <script type="text/javascript" src="~/Lms/UI/Js/jQuery.intellisense.js"></script>

    </asp:PlaceHolder>

    <script type="text/javascript">

        $(function() {
            if(<%= (!IsPostBack).ToString().ToLower() %>) 
            {
            $('#su').hide();
            $('#<%= this.btnGet.ClientID %>').hide();
            }

            $('#<%= this.ddlReportType.ClientID %>').click(
            function() {
                $('#<%= this.btnGet.ClientID %>').hide();
                $('#<%= this.hlnkReport.ClientID %>').hide();  
                $('#su').show('slow');

            });
            
             $('#<%= this.SelectUser.ClientID %>').click(
            function() {
                $('#<%= this.btnGet.ClientID %>').show('slow');
            });
           
//              $('#<%= this.btnGet.ClientID %>').click(
//            function() {
//                $('#<%= this.btnGet.ClientID %>').hide('slow');
//                $('#su').hide('slow');
//            });
//           
               $('#<%= this.hlnkReport.ClientID %>').change(
            function() {
                $('#<%= this.btnGet.ClientID %>').hide('slow');
                $('#su').hide('slow');
            });

            //$('#ddlRT').change(function() {
            //onchange = "$('#<%= this.btnGet.ClientID %>').show();"


        });


//        function reptype() {
//            $('#hidableTr').show();
//        };

//        function arg() {
//            var dd = document.getElementById("ddlRT");
//            return dd.value;
//        };

        //
    </script>

    <div style="width: 100%">
        <table>
            <tr>
                <td align="left">
                    <b><span style="font-size: medium">��� ������: </span></b>&nbsp;&nbsp;
<%--                    <select id="ddlRT" onchange="reptype();" >
                        <option value="�������������� ����������� ���������">�������������� ����������� ���������
                        </option>
                        <option value="��������������� ������">��������������� ������</option>
                        <option value="��������, ������">��������, ������</option>
                        <option value="��������� ����������">��������� ����������</option>
                        <option value="�������������� ��������">�������������� ��������</option>
                        <option value="�����">�����</option>
                    </select>
--%>                    
                    <asp:DropDownList ID="ddlReportType" runat="server"  OnSelectedIndexChanged=" ddlReportType_SelectedIndexChanged">
                        <asp:ListItem Value="erv">
                            �������������� ����������� ���������
                        </asp:ListItem>
                        <asp:ListItem Value="svrs">
                            ������� ��������� �� ����������� ������
                        </asp:ListItem>
                        <asp:ListItem Value="oaz">
                            ������ �� ������������� ���������������
                        </asp:ListItem>
                        <asp:ListItem Value="pv">
                            ��������������� ���������
                        </asp:ListItem>
                        <asp:ListItem Value="oes">
                            ������ �� ��������������� �������
                        </asp:ListItem>
                        <asp:ListItem Value="oz">
                            ����� �� �������
                        </asp:ListItem>
                        <asp:ListItem Value="irp">
                            ���������� � ������������� �������
                        </asp:ListItem>
                    </asp:DropDownList>
                    <%--<cc2:UserTreeTestBed ID="SelectUsertest" runat="server" />
                 
                    <asp:Button ID="btnGet" runat="server" Text="������������" OnClick="btnGet_Click" />
 --%>
                </td>
            </tr>
            <tr id="su">
                <td align="left">
                    <b><span style="font-size: medium">�������/������: </span></b>
                    <cc1:SelectUser ID="SelectUser" runat="server" AllowMultipleSelection="false" DisplayMode='Roles'
                        SelectionMode='Roles' />
                    &nbsp;
                    <%--                        ;   --%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnGet" runat="server" Text="������������" OnClick="btnGet_Click"
                         Width="95px" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:HyperLink ID="hlnkReport" runat="server"></asp:HyperLink>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
