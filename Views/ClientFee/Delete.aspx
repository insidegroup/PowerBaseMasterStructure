<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Base Client Fees</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Base Client Fee</th> 
		    </tr> 
            <tr>
                <td>Description</td>
                <td><%= Html.Encode(Model.ClientFee.ClientFeeDescription)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Fee Type</td>
                <td><%= Html.Encode(Model.ClientFee.FeeType.FeeTypeDescription)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>GDS</td>
                <td><%= Html.Encode(Model.ClientFee.GDS.GDSName)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Context</td>
                <td><%= Html.Encode(Model.ClientFee.Context.ContextName)%></td>
                <td></td>
            </tr>   
            <tr>
                <td>Output Description</td>
                <td><%= Html.Encode(Model.ClientFeeOutput.OutputDescription)%></td>
                <td></td>
            </tr>     
             <tr>
                <td>Output Format</td>
                <td><%= Html.Encode(Model.ClientFeeOutput.OutputFormat)%></td>
                <td></td>
            </tr>     
             <tr>
                <td>Output Placeholder</td>
                <td><%= Html.Encode(Model.ClientFeeOutput.OutputPlaceholder)%></td>
                <td></td>
            </tr>       
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
			<tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                <td class="row_footer_blank_right">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%= Html.HiddenFor(model => model.ClientFee.VersionNumber) %>
                    <%= Html.HiddenFor(model => model.ClientFee.ClientFeeId) %>
                <%}%>
                </td>  
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Fees Base Definitions", "Main", new { controller = "ClientFee", action = "List", }, new { title = "Client Fees Base Definitions" })%> &gt;
<%=Model.ClientFee.ClientFeeDescription%>
</asp:Content>

