<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TripTypeItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - TripTypeGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Trip Type Items</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		       <tr> 
			        <th class="row_header" colspan="3">Edit Trip Type Item</th> 
		        </tr>  
                <tr>
                    <td>Trip Type Group</td>
                    <td colspan="2"><%= Html.Encode(Model.TripTypeGroupName)%></td>
                </tr> 
                <tr>
                    <td>Trip Type</td>
                    <td><%= Html.Encode(Model.TripTypeDescription)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="DefaultTripTypeFlag">Default Trip Type</label></td>
                    <td><%= Html.CheckBox("DefaultTripTypeFlag", Model.DefaultTripTypeFlag ?? false)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.DefaultTripTypeFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Trip Type Item" title="Edit Trip Type Item" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.VersionNumber) %>
    <% } %>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_triptypegroups').click();
    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script></asp:Content>

