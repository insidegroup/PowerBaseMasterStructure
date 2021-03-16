<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ConfigurationParameterVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Configuration Parameters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Configuration Parameter</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Configuration Parameter</th> 
		        </tr>  
                 <tr>
                    <td>Configuration Parameter</td>
                    <td><%= Html.Encode(Model.ConfigurationParameter.ConfigurationParameterName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Application</td>
                    <td><%= Html.Encode(Model.ConfigurationParameter.Context.ContextName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="ConfigurationParameter_ConfigurationParameterValue">Value</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ConfigurationParameter.ConfigurationParameterValue, new { maxlength = "250"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ConfigurationParameter.ConfigurationParameterValue)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Configuration Parametere" title="Edit Configuration Parameter" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ConfigurationParameter.ConfigurationParameterName)%>
            <%= Html.HiddenFor(model => model.ConfigurationParameter.VersionNumber)%>
    <% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_configparameters').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })

 </script>
 </asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Configuration Parameters", "Main", new { controller = "ConfigurationParameter", action = "List" }, new { title = "Configuration Parameters" })%> &gt;
<%=Html.Encode(Model.ConfigurationParameter.ConfigurationParameterName)%>
</asp:Content>

