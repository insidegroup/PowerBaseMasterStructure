<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TripType>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Trip Type Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Create Trip Type</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
        <%= Html.AntiForgeryToken() %>
        
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create TripType</th> 
		        </tr> 
                <tr>
                    <td><label for="TripTypeDescription">Trip Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.TripTypeDescription, new { maxlength = "50", autocomplete="off"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TripTypeDescription)%></td>
                </tr>  
                <tr>
                    <td><label for="BackOfficeTripTypeCode">BackOffice Trip Type Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.BackOfficeTripTypeCode, new { maxlength = "10", autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.BackOfficeTripTypeCode)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Trip Type" title="Create Trip Type" class="red"/></td>
                </tr>
            </table>
    <% } %>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_triptypegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

