<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTelephonyVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">ClientSubUnit - Telephony</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
	    <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Telephony</th> 
		        </tr> 
                <tr>
                    <td><label for="ClientSubUnitTelephony_DialedNumber">Dialled Number</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientSubUnitTelephony.DialedNumber, new { maxlength = "15" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientSubUnitTelephony.DialedNumber)%></td>
                </tr> 
                 <tr>
                    <td><label for="ClientSubUnitTelephony_CallerEnteredDigitDefinitionTypeId">Identifier</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientSubUnitTelephony.CallerEnteredDigitDefinitionTypeId, Model.CallerEnteredDigitDefinitionTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientSubUnitTelephony.CallerEnteredDigitDefinitionTypeId)%></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Telephony" class="red" title="Create Telephony"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientSubUnitTelephony.ClientSubUnitGuid) %>
    <% } %>


    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Model.ClientSubUnit.ClientSubUnitName%>
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
Telephony
</asp:Content>