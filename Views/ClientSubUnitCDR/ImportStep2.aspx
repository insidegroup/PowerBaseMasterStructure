<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.CDRImportStep1VM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link</div></div>
    <div id="content">
     <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">CDR Link Import Summary</th> 
		    </tr>  
            <tr>
                <td valign="top" colspan="3"<%if(Model != null && Model.CDRImportStep2VM  != null && Model.CDRImportStep2VM.IsValidData == false){%> style="color:red;"<%} %>>
                    <%if(Model != null && Model.CDRImportStep2VM  != null && Model.CDRImportStep2VM.ReturnMessages != null) {
                            foreach (string msg in Model.CDRImportStep2VM.ReturnMessages){
                                Response.Write(msg); %>
                        <br/><br/>
                        <%}%>
					<%}else{%>
						<p>Sorry an error occured. Please try again.</p>
					<%}%>
                </td>
            </tr>  
            <%if(Model != null && Model.CDRImportStep2VM  != null && Model.CDRImportStep2VM.IsValidData){%>
            <tr>
                <td valign="top" colspan="3">
                 <br/><br/>Upon selecting Submit, the data will be imported to CDR '<%=Model.DisplayName%>' in '<%=Model.ClientSubUnit.ClientSubUnitDisplayName%>'
                </td>
            </tr>  
             <%}%>
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right">
					<%if (Model != null) { %> 
						<%if (Model.CDRImportStep2VM.IsValidData){%>
							<input type="submit" value="Submit" title="Submit" class="red"/>
							<%= Html.ActionLink("Cancel", "List", "ClientSUbUnitCDR", new { id = Model.ClientSubUnitGuid }, new { @Class = "red" })%>
						<%}%>
					<%}%>
                </td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.CDRImportStep2VM.FileBytes)%>
        <%= Html.HiddenFor(model => model.ClientSubUnitGuid) %>
        <%= Html.HiddenFor(model => model.ClientDefinedReferenceItemId) %>
        <%= Html.HiddenFor(model => model.DisplayName) %>
		<%= Html.HiddenFor(model => model.RelatedToDisplayName) %>
	<% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

