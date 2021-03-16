<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Base Client Fees</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Base Client Fee</th> 
		        </tr>  
                <tr>
                    <td><label for="ClientFee_ClientFeeDescription">Description</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFee.ClientFeeDescription, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFee.ClientFeeDescription)%></td>
                </tr> 
                 <tr>
                    <td>Fee Type</td>
                    <td><%= Html.Encode(Model.ClientFee.FeeType.FeeTypeDescription)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td><label for="ClientFee_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientFee.GDSCode, Model.GDSs, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFee.GDSCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="ClientFee_ContextId">Context</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientFee.ContextId, Model.Contexts, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFee.ContextId)%></td>
                </tr>  
                 <tr>
                    <td><label for="ClientFeeOutput_OutputDescription">Output Description</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFeeOutput.OutputDescription, new { maxlength = "50", autocomplete = "off", style = "width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeOutput.OutputDescription)%></td>
                </tr> 
                  <tr>
                    <td><label for="ClientFeeOutput_OutputFormat">Output Format</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFeeOutput.OutputFormat, new { maxlength = "50", autocomplete = "off", style = "width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeOutput.OutputFormat)%></td>
                </tr> 
                  <tr>
                    <td><label for="ClientFeeOutput_OutputPlaceholder">Output Placeholder</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFeeOutput.OutputPlaceholder, new { maxlength = "50", autocomplete = "off", style = "width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeOutput.OutputPlaceholder)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Base Client Fee" title="Edit Base Client Fee" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientFee.FeeTypeId)%>
            <%= Html.HiddenFor(model => model.ClientFee.ClientFeeId)%>
            <%= Html.HiddenFor(model => model.ClientFee.VersionNumber)%>
    <% } %>
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


