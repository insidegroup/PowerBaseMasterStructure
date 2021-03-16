<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldLookupValueVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Look Up Values</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Optional Field Look Up Values</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Optional Field Look Up Values</th> 
		    </tr> 
            <tr>
                <td>Optional Field Look Up Value</td>
                <td><%= Html.Encode(Model.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueLabel)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.OptionalFieldLookupValueLanguage.Language.LanguageName)%></td>
                <td></td>
            </tr>
			<tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
            </tr>
        </table>
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
<%=Html.RouteLink(ViewData["OptionalFieldName"].ToString(), new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLookupValue.OptionalFieldId }, new { title = ViewData["OptionalFieldName"] })%>  &gt;
<%=Html.RouteLink("Definition Values", new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLookupValue.OptionalFieldId }, new { title = "Definition Values" })%>  &gt;
<%=Model.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueLabel %>
</asp:Content>