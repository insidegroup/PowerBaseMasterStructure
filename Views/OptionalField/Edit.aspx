<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Definitions</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Definitions</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Optional Field</th> 
		        </tr>  
                <tr>
                    <td><label for="OptionalField_OptionalFieldName">Optional Field Name</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalField.OptionalFieldName, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalField.OptionalFieldName)%></td>
                </tr> 
                 <tr>
                    <td><label for="OptionalField_OptionalFieldStyleId">Style</label></td>
                    <td><%= Html.DropDownListFor(model => model.OptionalField.OptionalFieldStyleId, Model.OptionalFieldStyles, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalField.OptionalFieldStyleId)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Optional Field" title="Edit Optional Field" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.OptionalField.OptionalFieldId)%>
			<%= Html.HiddenFor(model => model.OptionalField.VersionNumber)%>
    <% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_passivesegmentbuilder').click();
    	$('#menu_passivesegmentbuilder_optionalfields').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })

 </script>
 </asp:Content>
 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Definitions", "Main", new { controller = "OptionalField", action = "List", }, new { title = "Optional Field Definitions" })%> &gt;
<%=Model.OptionalField.OptionalFieldName%>
</asp:Content>


