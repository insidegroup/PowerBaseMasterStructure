<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ControlValue>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Control Values</div></div>
        <div id="content">
       <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Control Value</th> 
		        </tr> 
                <tr>
                    <td><label for="ControlValue1">Control Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.ControlValue1, new { maxlength="256"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ControlValue1)%></td>
                </tr>   
                <tr>
                    <td><label for="ControlPropertyId">Property</label></td>
                    <td><%= Html.DropDownList("ControlPropertyId", ViewData["ControlProperties"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ControlPropertyId)%></td>
                </tr> 
                  <tr>
                    <td><label for="ControlNameId">Name</label></td>
                    <td><%= Html.DropDownList("ControlNameId", ViewData["ControlNames"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ControlNameId)%></td>
                </tr> 
               <tr>
                    <td><label for="DefaultValueFlag">Default Value?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.DefaultValueFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.DefaultValueFlag)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Control Value" title="Create Control Value" class="red"/></td>
                </tr>
            </table>
    <% } %>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
            $('#menu_admin').click();
				$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
        })

 </script>
</asp:Content>



