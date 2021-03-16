<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitImportStep1VM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Client Top Unit Locations
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Top Unit Locations</div></div>
    <div id="content">
     <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Client Top Unit Location Import Summary</th> 
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
                 <br/><br/>Upon selecting Submit, the data will be imported to the locations of the Client Top Unit
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
                <%= Html.ActionLink("Cancel", "List", "ClientTopUnitClientLocation", new { id = Model.ClientTopUnit.ClientTopUnitGuid }, new { @Class = "red" })%><%}%></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.ImportStep2VM.FileBytes)%>       
        <%= Html.HiddenFor(model => model.ImportStep2VM.IsValidData)%>  
        <%= Html.Hidden("ImportStep2VM.ReturnMessages", Newtonsoft.Json.JsonConvert.SerializeObject(Model.ImportStep2VM.ReturnMessages))%> 
        <%= Html.HiddenFor(model => model.ClientTopUnit.ClientTopUnitGuid) %>
	<% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
Client Locations
</asp:Content>