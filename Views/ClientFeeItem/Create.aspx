<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Fee Items</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Client Fee Item</th> 
		        </tr> 
                 <tr>
                    <td>Fee Type</td>
                    <td><%=Model.FeeType.FeeTypeDescription %></td>
                    <td></td>
                </tr>   
                 <tr>
                    <td><label for="ClientFeeItem_ClientFeeId"><%=Model.FeeType.FeeTypeDescription %></label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientFeeItem.ClientFeeId, Model.ClientFees, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeItem.ClientFeeId)%></td>
                </tr>  
 
                 <tr>
                    <td><label for="ClientFeeItem_ValueAmount">Value Amount</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFeeItem.ValueAmount, new { maxlength = "50", autocomplete = "off", style = "width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeItem.ValueAmount)%></td>
                </tr> 
                  <tr>
                    <td><label for="ClientFeeItem_ValuePercentage">Value Percentage</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFeeItem.ValuePercentage, new { maxlength = "50", autocomplete = "off", style = "width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeItem.ValuePercentage)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Client Fee Item" title="Create Client Fee Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientFeeItem.ClientFeeGroupId  ) %>
            <%= Html.HiddenFor(model => model.FeeType.FeeTypeId)%>
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
<%=Html.RouteLink(Model.FeeType.FeeTypeDescription + " Groups", "Main", new { controller="ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeType.FeeTypeId }, new { title = Model.FeeType.FeeTypeDescription })%> &gt;
<%=Html.RouteLink(Model.ClientFeeItem.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeItem.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeItem.ClientFeeGroup.ClientFeeGroupName })%> &gt;
Client Fee Items
</asp:Content>

