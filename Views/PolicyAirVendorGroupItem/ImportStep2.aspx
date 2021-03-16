<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirVendorImportStep1VM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Policy AirVendor Group
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Vendor Group Items</div></div>
    <div id="content">
     <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Policy Air Vendor Group Item Import Summary</th> 
		    </tr>  
            <tr>
                <td valign="top" colspan="3"<%if(Model != null && Model.ImportStep2VM  != null && Model.ImportStep2VM.IsValidData == false){%> style="color:red;"<%} %>>
                    <%if(Model != null && Model.ImportStep2VM  != null && Model.ImportStep2VM.ReturnMessages != null) {
                            foreach (string msg in Model.ImportStep2VM.ReturnMessages){
                                Response.Write(msg); %>
                        <br/><br/>
                        <%}%>
                    <%}%>
                </td>
            </tr>  
            <%if(Model != null && Model.ImportStep2VM  != null && Model.ImportStep2VM.IsValidData){%>
            <tr>
                <td valign="top" colspan="3">
                 <br/><br/>Upon selecting Submit, the data will be imported to the Policy Air Vendor Group Items of the Policy Group
                </td>
            </tr>  
             <%}%>
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2">
                    <a href="javascript:history.back();" class="red" title="Back">Back</a>
                    <%if (Model.ImportStep2VM.IsValidData == false)
                                                     {%>

                    <input type="submit" value="Export" title="Export" class="red"/><%}%>
                </td>

                <td class="row_footer_blank_right"><%if (Model.ImportStep2VM.IsValidData)
                                                     {%><input type="submit" value="Submit" title="Submit" class="red"/>
                <%= Html.ActionLink("Cancel", "List", "PolicyAirVendorGroupItem", new { id = Model.PolicyGroupId }, new { @Class = "red" })%><%}%></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.ImportStep2VM.FileBytes)%>       
        <%= Html.HiddenFor(model => model.ImportStep2VM.IsValidData)%>  
        <%= Html.Hidden("ImportStep2VM.ReturnMessages", Newtonsoft.Json.JsonConvert.SerializeObject(Model.ImportStep2VM.ReturnMessages))%> 
        <%= Html.HiddenFor(model => model.PolicyGroupId) %>
	<% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName + " Items", "Main", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName + " Items" })%> &gt;
<%=Html.RouteLink("Policy Air Vendor Group Items", "Main", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Air Vendor Group Items" })%> &gt;
<%=Html.RouteLink("Import", "Main", new { controller = "PolicyAirVendorGroupItem", action = "ImportStep1", id = Model.PolicyGroupId }, new { title = "Import" })%>
</asp:Content>

